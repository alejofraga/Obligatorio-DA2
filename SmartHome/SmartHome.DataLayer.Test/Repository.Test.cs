using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class RepositoryTest
{
    private DbContext _context = null!;
    private Repository<TestDbContext.DummyEntity> _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildTestDbContext();
        _repository = new Repository<TestDbContext.DummyEntity>(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Exist_WhenValuesAreCorrect_ShouldReturnTrue()
    {
        var expectedValue = "Test";
        var dummy = new TestDbContext.DummyEntity { Value = expectedValue };
        _repository.Add(dummy);

        var result = _repository.Exist(x => x.Value == expectedValue);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void Add_WhenValuesAreCorrect_ShouldAdd()
    {
        var expectedValue = "Test";
        var dummy = new TestDbContext.DummyEntity { Value = expectedValue };

        _repository.Add(dummy);

        var result = _repository.GetOrDefault(x => x.Value == expectedValue);
        result.Should().NotBeNull();
        result!.Value.Should().Be(expectedValue);
    }

    [TestMethod]
    public void GetAll_WhenValuesAreCorrect_ShouldGetAll()
    {
        var dummy1 = new TestDbContext.DummyEntity { Value = "Test1" };
        var dummy2 = new TestDbContext.DummyEntity { Value = "Test2" };
        var expectedLength = 2;
        _repository.Add(dummy1);
        _repository.Add(dummy2);

        var result = _repository.GetAll();

        result.Count.Should().Be(expectedLength);
    }

    [TestMethod]
    public void GetOrDefault_WhenValuesAreCorrect_ShouldReturnGet()
    {
        var expectedValue = "newTest";
        var dummy = new TestDbContext.DummyEntity() { Value = expectedValue };
        _repository.Add(dummy);

        var result = _repository.GetOrDefault(x => x.Value == expectedValue);

        result.Should().NotBeNull();
        result!.Value.Should().Be(expectedValue);
    }

    [TestMethod]
    public void Remove_WhenValuesAreCorrect_ShouldRemove()
    {
        var expectedValue = "Test";
        var dummy = new TestDbContext.DummyEntity { Value = expectedValue };
        _repository.Add(dummy);

        _repository.Remove(dummy);

        var result = _repository.Exist(x => x.Value == expectedValue);
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Update_WhenValuesAreCorrect_ShouldUpdate()
    {
        var dummy = new TestDbContext.DummyEntity { Value = "Test" };
        _repository.Add(dummy);

        _repository.Update(dummy);

        var result = _repository.Exist(x => x.Value == "Test");
        result.Should().BeTrue();
    }
}
