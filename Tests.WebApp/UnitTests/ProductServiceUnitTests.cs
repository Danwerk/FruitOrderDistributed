using App.BLL.Services;
using App.Contracts.DAL;
using Base.Contracts;
using Moq;
using Product = App.BLL.DTO.Product;


namespace Tests.WebApp.UnitTests;

public class ProductServiceUnitTests
{
    private readonly Mock<IMapper<App.BLL.DTO.Product, App.Domain.Product>> _mapperMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IAppUOW> _uowMock;


    public ProductServiceUnitTests()
    {
        _uowMock = new Mock<IAppUOW>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper<Product, App.Domain.Product>>();
    }


    [Fact]
    public async Task TestAllAsync()
    {
        // Arrange
        var expectedProducts = new List<App.Domain.Product>
        {
            new App.Domain.Product() { Id = Guid.NewGuid() },
            new App.Domain.Product() { Id = Guid.NewGuid() },
            new App.Domain.Product() { Id = Guid.NewGuid() }
        };

        _uowMock.Setup(x => x.ProductRepository).Returns(_productRepositoryMock.Object);
        _productRepositoryMock.Setup(x => x.AllAsync()).ReturnsAsync(expectedProducts);

        var productService = new ProductService(_uowMock.Object, _mapperMock.Object);

        var actualProducts = await productService.AllAsync();
        Assert.NotNull(actualProducts);
        Assert.Equal(expectedProducts.Count(), actualProducts.Count());
        _productRepositoryMock.Verify(
            x => x.AllAsync(), Times.Once);


        var expectedProductsDto = expectedProducts.Select(p => _mapperMock.Object.Map(p)).ToList();
        Assert.Equal(expectedProductsDto, actualProducts);
    }
    
    

    [Fact]
    public async Task TestFirstOrDefaultAsync_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new App.Domain.Product() { Id = productId };
        var bllProduct = new Product() { Id = productId };

        _uowMock.Setup(x => x.ProductRepository).Returns(_productRepositoryMock.Object);
        _productRepositoryMock.Setup(r => r.FirstOrDefaultAsync(productId))
            .ReturnsAsync(product);
        _mapperMock.Setup(x => x.Map(product)).Returns(bllProduct);

        var productService = new ProductService(_uowMock.Object, _mapperMock.Object);

        var result = await productService.FirstOrDefaultAsync(productId);


        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        _productRepositoryMock.Verify(x => x.FirstOrDefaultAsync(productId), Times.Once);
        _mapperMock.Verify(x => x.Map(product), Times.Once);
    }


    [Fact]
    public async Task TestFirstOrDefaultAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();
        App.Domain.Product? product = null;

        _uowMock.Setup(x => x.ProductRepository).Returns(_productRepositoryMock.Object);
        _productRepositoryMock.Setup(r => r.FirstOrDefaultAsync(productId))
            .ReturnsAsync(product);

        var productService = new ProductService(_uowMock.Object, _mapperMock.Object);
        // Act
        var result = await productService.FirstOrDefaultAsync(productId);

        // Assert
        Assert.Null(result);
        _productRepositoryMock.Verify(x => x.FirstOrDefaultAsync(productId), Times.Once);
        _mapperMock.Verify(x => x.Map(product), Times.Once);
    }
}