using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

public class LabelRepositoryTests : BaseRepoTest
{
    private ILabelRepository _labelRepository;

    [SetUp]
    public new void Setup()
    {
        _labelRepository = _scope.Resolve<ILabelRepository>();
    }

    [Test]
    public async Task Add_Label_Should_Work()
    {
        using var uow = _unitOfWorkProvider.Provide();
        await _labelRepository.Add(new Label{Title = "test"});
        await uow.CommitAsync();
    }
}