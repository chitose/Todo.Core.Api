using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.Tests;

[TestFixture]
public class LabelRepositoryTests : BaseRepositoryTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _labelRepository = _scope.Resolve<ILabelRepository>();
    }

    private ILabelRepository _labelRepository;

    [Test]
    public async Task Add_label()
    {
        var lbl = await _dataCreator.CreateLabel("Test label");
        lbl.Id.Should().BePositive();
    }

    [Test]
    public async Task Update_label()
    {
        var lbl = await _dataCreator.CreateLabel("Test lbl");

        await _unitOfWorkProvider.PerformActionInUnitOfWork(() =>
        {
            lbl.Title = "update title";
            return _labelRepository.Save(lbl);
        });

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var updateLbl = await _labelRepository.GetByKey(lbl.Id);
            updateLbl.Title.Should().BeEquivalentTo(lbl.Title);
        });
    }

    [Test]
    public async Task Delete_label()
    {
        var lbl = await _dataCreator.CreateLabel("Dummy lable");

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () => { await _labelRepository.Delete(lbl); });

        _dataCreator.Remove(lbl);

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var flbl = await _labelRepository.GetByKey(lbl.Id);
            flbl.Should().BeNull();
        });
    }
}