using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTests;

[TestFixture]
public class LabelRepositoryTests : BaseTest
{
    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _labelRepository = _scope.Resolve<ILabelRepository>();
    }

    private ILabelRepository _labelRepository;

    [Test]
    public async Task Add_label_should_work()
    {
        var lbl = await _dataCreator.CreateLabel("Test label");
        Assert.IsTrue(lbl.Id > 0);
    }

    [Test]
    public async Task Update_label_should_work()
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
            Assert.AreEqual(updateLbl.Title, lbl.Title);
        });
    }

    [Test]
    public async Task Delete_label_should_work()
    {
        var lbl = await _dataCreator.CreateLabel("Dummy lable");

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () => { await _labelRepository.Delete(lbl); });

        _dataCreator.Remove(lbl);

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var flbl = await _labelRepository.GetByKey(lbl.Id);
            Assert.IsNull(flbl);
        });
    }
}