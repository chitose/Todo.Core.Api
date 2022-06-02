using Todo.Core.Common.Context;
using Todo.Core.Persistence.Entities;

namespace Todo.Core.Persistence.Repositories;

public class LabelRepository : GenericEntityRepository<Label>, ILabelRepository
{
    private readonly IUserRepository _userRepository;

    public LabelRepository(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<Label> Add(Label entity, CancellationToken cancellationToken = default)
    {
        entity.Owner = await _userRepository.FindByUserName(UserContext.UserName!);
        return await base.Add(entity, cancellationToken);
    }
}