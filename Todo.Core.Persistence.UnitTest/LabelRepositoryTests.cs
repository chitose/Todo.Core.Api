using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Todo.Core.Persistence.Entities;
using Todo.Core.Persistence.Repositories;

namespace Todo.Core.Persistence.UnitTest;

public class LabelRepositoryTests : BaseTest
{
    private ILabelRepository _labelRepository;

    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _labelRepository = _scope.Resolve<ILabelRepository>();
    }

    [Test]
    public async Task Add_label_should_work()
    {
        var lbl = new Label { Title = "test" };
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _labelRepository.Add(lbl));
        Assert.IsTrue(lbl.Id > 0);
    }

    [Test]
    public async Task Update_label_should_work()
    {
        var lbl = new Label { Title = "test title" };
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _labelRepository.Add(lbl));

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
        var lbl = new Label { Title = "dummy label" };
        await _unitOfWorkProvider.PerformActionInUnitOfWork(() => _labelRepository.Add(lbl));
        Assert.IsTrue(lbl.Id > 0);

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () => { await _labelRepository.Delete(lbl); });

        await _unitOfWorkProvider.PerformActionInUnitOfWork(async () =>
        {
            var flbl = await _labelRepository.GetByKey(lbl.Id);
            Assert.IsNull(flbl);
        });
    }
}