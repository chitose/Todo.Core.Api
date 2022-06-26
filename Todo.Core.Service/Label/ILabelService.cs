using Todo.Core.Domain.Project;

namespace Todo.Core.Service.Label;

public interface ILabelService
{
    Task<IList<Persistence.Entities.Label>> ResolveLabels(IEnumerable<LabelAssignment> labelAssignments,
        CancellationToken cancellationToken = default);

    Task<Persistence.Entities.Label> CreateLabel(string title, bool shared,
        CancellationToken cancellationToken = default);

    Task<Persistence.Entities.Label> UpdateLabel(int id, string newTitle, bool shared,
        CancellationToken cancellationToken = default);
}