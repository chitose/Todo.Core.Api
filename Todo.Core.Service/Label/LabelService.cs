using NHibernate.Linq;
using Todo.Core.Common.Context;
using Todo.Core.Common.UnitOfWork;
using Todo.Core.Domain.Project;
using Todo.Core.Persistence.Exceptions;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Service.Label;

public class LabelService : ILabelService
{
    private readonly ILabelRepository _labelRepository;
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;

    public LabelService(ILabelRepository labelRepository, IUnitOfWorkProvider unitOfWorkProvider)
    {
        _labelRepository = labelRepository;
        _unitOfWorkProvider = unitOfWorkProvider;
    }

    public async Task<IList<Persistence.Entities.Label>> ResolveLabels(IEnumerable<LabelAssignment> labelAssignments,
        CancellationToken cancellationToken = default)
    {
        var userCtx = UserContext.UserName;
        return await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            var lblIds = labelAssignments.Where(x => x.Id.HasValue)
                .Select(x => x.Id).ToList();
            var lblTexts = labelAssignments.Where(x => !x.Id.HasValue
                                                       && !string.IsNullOrWhiteSpace(x.Title))
                .Select(x => x.Title).ToList();

            return _labelRepository.GetQuery()
                .Where(x => (x.Owner.UserName == userCtx || x.Shared)
                            && (lblIds.Contains(x.Id) || lblTexts.Exists(lt => lt == x.Title)))
                .ToListAsync(cancellationToken);
        });
    }

    public Task<Persistence.Entities.Label> CreateLabel(string title, bool shared,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () => await _labelRepository.Add(
            new Persistence.Entities.Label
            {
                Title = title,
                Shared = shared
            }, cancellationToken));
    }

    public Task<Persistence.Entities.Label> UpdateLabel(int id, string newTitle, bool shared,
        CancellationToken cancellationToken = default)
    {
        return _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var lbl = await _labelRepository.GetByKey(id, cancellationToken);
            if (lbl == null) throw new LabelNotFoundException(id);

            lbl.Title = newTitle;
            lbl.Shared = shared;

            return await _labelRepository.Save(lbl, cancellationToken);
        });
    }
}