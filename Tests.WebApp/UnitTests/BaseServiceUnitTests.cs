using Base.BLL;
using Base.Contracts;
using Base.Contracts.DAL;
using Domain.Contracts.Base;
using Moq;

namespace Tests.WebApp.UnitTests;

public class BaseServiceUnitTests
{
    private readonly Mock<IBaseRepository<DalEntity>> _repositoryMock;
    private readonly Mock<IMapper<BllEntity, DalEntity>> _mapperMock;
    private readonly BaseEntityService<BllEntity, DalEntity, IBaseRepository<DalEntity>> _service;

    public BaseServiceUnitTests()
    {
        _repositoryMock = new Mock<IBaseRepository<DalEntity>>();
        _mapperMock = new Mock<IMapper<BllEntity, DalEntity>>();
        _service = new BaseEntityService<BllEntity, DalEntity, IBaseRepository<DalEntity>>(_repositoryMock.Object,
            _mapperMock.Object);
    }


    [Fact]
    public async Task AllAsync_ReturnsMappedEntities()
    {
        // Arrange
        var dalEntities = new List<DalEntity>
        {
            new DalEntity
            {
                Id = Guid.NewGuid()
            },
            new DalEntity()
            {
                Id = Guid.NewGuid()
            }
        };
        _repositoryMock.Setup(x => x.AllAsync()).ReturnsAsync(dalEntities);

        var bllEntities = dalEntities.Select(x => new BllEntity { Id = x.Id });
        _mapperMock.Setup(x => x.Map(It.IsAny<DalEntity>()))
            .Returns<DalEntity>(x => new BllEntity { Id = x.Id });

        // Act
        var result = await _service.AllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bllEntities.Count(), result.Count());
        _repositoryMock.Verify(x => x.AllAsync(), Times.Once);

        foreach (var bllEntity in bllEntities)
        {
            Assert.Contains(result, x => x.Id == bllEntity.Id);
        }
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dalEntity = new DalEntity { Id = id };
        _repositoryMock.Setup(x => x.FindAsync(id)).ReturnsAsync(dalEntity);

        var bllEntity = new BllEntity { Id = dalEntity.Id };
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);

        // Act
        var result = await _service.FindAsync(id);

        // Assert
        Assert.Equal(bllEntity, result);
        _repositoryMock.Verify(x => x.FindAsync(id), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once());
    }


    [Fact]
    public async Task FirstOrDefaultAsync_ValidId_ReturnsMappedEntity()
    {
        // Arrange
        var id = Guid.NewGuid();

        var dalEntity = new DalEntity { Id = id };
        var bllEntity = new BllEntity { Id = id };
        _repositoryMock.Setup(x => x.FirstOrDefaultAsync(id)).ReturnsAsync(dalEntity);
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);


        // Act
        var result = await _service.FirstOrDefaultAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(result, bllEntity);
        _repositoryMock.Verify(x => x.FirstOrDefaultAsync(id), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once());
    }

    [Fact]
    public async Task FirstOrDefaultAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        DalEntity? dalEntity = null;
        BllEntity? bllEntity = null;

        _repositoryMock.Setup(x => x.FirstOrDefaultAsync(id)).ReturnsAsync(dalEntity);
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);


        // Act
        var result = await _service.FirstOrDefaultAsync(id);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(x => x.FirstOrDefaultAsync(id), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once());
    }


    [Fact]
    public void Add_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();

        // Arrange
        var bllEntity = new BllEntity { Id = id };
        var dalEntity = new DalEntity { Id = id };
        _repositoryMock.Setup(x => x.Add(dalEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(bllEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);

        // Act
        var result = _service.Add(bllEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(bllEntity, result);
        _repositoryMock.Verify(x => x.Add(dalEntity), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once);
        _mapperMock.Verify(x => x.Map(bllEntity), Times.Once);
    }


    [Fact]
    public void Update_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();

        // Arrange
        var bllEntity = new BllEntity { Id = id };
        var dalEntity = new DalEntity { Id = id };
        _repositoryMock.Setup(x => x.Update(dalEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(bllEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);

        // Act
        var result = _service.Update(bllEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        _repositoryMock.Verify(x => x.Update(dalEntity), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once);
        _mapperMock.Verify(x => x.Map(bllEntity), Times.Once);
    }


    [Fact]
    public void Remove_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();

        // Arrange
        var bllEntity = new BllEntity { Id = id };
        var dalEntity = new DalEntity { Id = id };
        _repositoryMock.Setup(x => x.Remove(dalEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(bllEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);

        // Act
        var result = _service.Remove(bllEntity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dalEntity.Id, result.Id);
        _repositoryMock.Verify(x => x.Remove(dalEntity), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once);
        _mapperMock.Verify(x => x.Map(bllEntity), Times.Once);
    }


    [Fact]
    public void RemoveAsync_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();

        // Arrange
        var bllEntity = new BllEntity { Id = id };
        var dalEntity = new DalEntity { Id = id };
        _repositoryMock.Setup(x => x.RemoveAsync(dalEntity.Id)).ReturnsAsync(dalEntity);
        _mapperMock.Setup(x => x.Map(bllEntity)).Returns(dalEntity);
        _mapperMock.Setup(x => x.Map(dalEntity)).Returns(bllEntity);

        // Act
        var result = _service.RemoveAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Result!.Id);
        _repositoryMock.Verify(x => x.RemoveAsync(dalEntity.Id), Times.Once);
        _mapperMock.Verify(x => x.Map(dalEntity), Times.Once);
    }
}

public class DalEntity : IDomainEntityId
{
    public Guid Id { get; set; } = new Guid();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; } = "";

    // Other properties of the entity
}

public class BllEntity : IDomainEntityId
{
    public Guid Id { get; set; } = new Guid();
}