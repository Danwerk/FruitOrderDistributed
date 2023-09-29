using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.WebApp.IntegrationTests.api;

public class ProductIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    
    public ProductIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false

        });
    }

   
    [Fact]
    public async Task UnregisteredUserCannotAddProduct()
    {
        // Arrange
        string URL = "/api/v1/products";

        var product = new App.Public.DTO.v1.Product()
        {
            Name = "Maasikas",
            Description = "magus",
            Quantity = 5,
            Image = ""

        };
        var data = JsonContent.Create(product);

        // Act
        var response = await _client.PostAsync(URL, data);

        // Assert
        Assert.False(response.StatusCode == HttpStatusCode.OK);
        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

    }
    
    [Fact]
    public async Task GetProducts_Success()
    {
        var response = await _client.GetAsync("/api/v1.0/Products");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetProducts_CorrectResponse()
    {
        var response = await _client.GetFromJsonAsync<IEnumerable<App.Public.DTO.v1.Product>>("api/v1.0/Products");

        Assert.Empty(response!);
    }

    
    [Fact(DisplayName = "create / get / edit / delete product")]
    public async Task TestProductCrud()
    {
        const string email = "crud@test.ee";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar.1";
        const int expiresInSeconds = 30;

        const string url = "/api/v1/Products";

        // Arrange
        var jwt = await RegisterNewUser(email, password, firstname, lastname, expiresInSeconds);
        var jwtResponse = JsonSerializer.Deserialize<JwtResponse>(jwt, _camelCaseJsonSerializerOptions);

        
        // CREATE product
        
        var productTypeId = Guid.Parse("0f6368ec-f708-48ea-ae64-be5ca7fbad75");
        var unitId = Guid.Parse("727b6f6d-6ddd-4820-8306-4628b5609893");
        
        var handler = new JwtSecurityTokenHandler();
        var claims = handler.ReadJwtToken(jwtResponse!.JwtToken).Claims;
        var userId = claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

        var product = new Product()
        {
            Description = "desc",
            Image = "img",
            Name = "test product",
            Quantity = 100,
            ActiveDiscount = 12,
            ActivePrice = 123,
            ProductTypeId = productTypeId,
            UnitId = unitId
        };


       await CreateProduct(product, jwtResponse.JwtToken);

        // GET all products
        
        var getProductsRequest = new HttpRequestMessage(HttpMethod.Get, url);
        getProductsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JwtToken);

        var getProductsResponse = await _client.SendAsync(getProductsRequest);

        Assert.True(getProductsResponse.StatusCode == HttpStatusCode.OK);
        Assert.NotNull(getProductsResponse.Content);

        var products = getProductsResponse.Content.ReadFromJsonAsync<List<Product>>().Result;
        
        Assert.NotNull(products);
        Assert.Single(products);
        Assert.Equal("test product", products[0].Name);
        Assert.Equal("img", products[0].Image);
        Assert.Equal("desc", products[0].Description);


        
        // EDIT product

        var editedProduct = products[0];
        editedProduct.Name = "edited name";
        editedProduct.Description = "qwerty";
        editedProduct.Image = "changed image";
        editedProduct.Quantity = 11234567;

        var putProductRequest = new HttpRequestMessage(HttpMethod.Put, url + $"/{products[0].Id}");
        putProductRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JwtToken);
        putProductRequest.Content = JsonContent.Create(editedProduct);

        var putProductResponse = await _client.SendAsync(putProductRequest);
        Assert.True(putProductResponse.StatusCode == HttpStatusCode.NoContent);


        // GET EDITED PRODUCT
        var getEditedProductRequest = new HttpRequestMessage(HttpMethod.Get, url);
        getEditedProductRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JwtToken);

        var getEditedProductResponse = await _client.SendAsync(getEditedProductRequest);

        Assert.True(getEditedProductResponse.StatusCode == HttpStatusCode.OK);
        Assert.NotNull(getEditedProductResponse.Content);

        products = getEditedProductResponse.Content.ReadFromJsonAsync<List<Product>>().Result;
        
        Assert.NotNull(products);
        Assert.Single(products);
        Assert.Equal("edited name", products[0].Name);

        
        
        // DELETE PRODUCT

        var deleteProductRequest = new HttpRequestMessage(HttpMethod.Delete, url + $"/{editedProduct.Id}");
        deleteProductRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JwtToken);

        var deleteProductResponse = await _client.SendAsync(deleteProductRequest);
        Assert.True(deleteProductResponse.StatusCode == HttpStatusCode.NoContent);

        
        // Assert that was deleted
        var getProductsAfterDeletionRequest = new HttpRequestMessage(HttpMethod.Get, url);
        getProductsAfterDeletionRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JwtToken);

        var getProductsAfterDeletionResponse = await _client.SendAsync(getProductsAfterDeletionRequest);

        Assert.True(getProductsAfterDeletionResponse.StatusCode == HttpStatusCode.OK);
        Assert.NotNull(getProductsAfterDeletionResponse.Content);

        products = getProductsAfterDeletionResponse.Content.ReadFromJsonAsync<List<Product>>().Result;
        
        Assert.NotNull(products);
        Assert.Empty(products);

    }
    
    private async Task<App.Domain.Product> CreateProduct(App.Public.DTO.v1.Product product, string jwtToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/products");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        request.Content = JsonContent.Create(product);
        
        var response = await _client.SendAsync(request);
        Assert.True(response.StatusCode == HttpStatusCode.Created);

        var createdProduct =
            await response.Content.ReadFromJsonAsync<App.Domain.Product>(options: _camelCaseJsonSerializerOptions);
        return createdProduct!;
    }
    
    
    private void VerifyJwtContent(string jwt, string email, string firstname, string lastname,
        DateTime validToIsSmallerThan)
    {
        var jwtResponse = JsonSerializer.Deserialize<JwtResponse>(jwt, _camelCaseJsonSerializerOptions);

        Assert.NotNull(jwtResponse);
        Assert.NotNull(jwtResponse.RefreshToken);
        Assert.NotNull(jwtResponse.JwtToken);

        // verify the actual JWT

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(jwtResponse.JwtToken);

        Assert.Equal(email, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
        Assert.Equal(firstname, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value);
        Assert.Equal(lastname, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value);
        // Assert.True(jwtToken.ValidTo < validToIsSmallerThan);
    }

    public async Task<string> RegisterNewUser(string email, string password, string firstname, string lastname,
        int expiresInSeconds = 100)
    {
        var URL = $"/api/v1/identity/account/register?expiresInSeconds={expiresInSeconds}";

        var registerData = new
        {
            Email = email,
            Password = password,
            Firstname = firstname,
            Lastname = lastname,
        };

        var data = JsonContent.Create(registerData);
        // Act
        var response = await _client.PostAsync(URL, data);

        var responseContent = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.True(response.IsSuccessStatusCode);

        VerifyJwtContent(responseContent, email, firstname, lastname,
            DateTime.Now.AddSeconds(expiresInSeconds + 1).ToUniversalTime());

        return responseContent;
    }
}