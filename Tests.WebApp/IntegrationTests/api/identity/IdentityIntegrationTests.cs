using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using App.Public.DTO.v1.Identity;


namespace Tests.WebApp.IntegrationTests.api.identity;

public class IdentityIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };


    public IdentityIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact(DisplayName = "POST - register new user")]
    public async Task RegisterNewUserTest()
    {
        // Arrange
        const string URL = "/api/v1/identity/account/register";
        const string email = "test@test.ee";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar.2";
        var registerData = new
        {
            Email = email,
            Password = password,
            Firstname = firstname,
            Lastname = lastname
        };
        var data = JsonContent.Create(registerData);

        // Act
        var response = await _client.PostAsync(URL, data);

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

        VerifyJwtContent(responseContent, email, firstname, lastname, DateTime.Now.AddSeconds(2).ToUniversalTime());
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
        int expiresInSeconds = 1)
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


    [Fact(DisplayName = "POST - login user")]
    public async Task LoginUserTest()
    {
        const string email = "login@test.ee";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar1";
        const int expiresInSeconds = 1;

        // Arrange
        await RegisterNewUser(email, password, firstname, lastname, expiresInSeconds);


        var URL = "/api/v1/identity/account/login?expiresInSeconds=1";

        var loginData = new
        {
            Email = email,
            Password = password,
        };

        var data = JsonContent.Create(loginData);

        // Act
        var response = await _client.PostAsync(URL, data);

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        VerifyJwtContent(responseContent, email, firstname, lastname,
            DateTime.Now.AddSeconds(expiresInSeconds + 1).ToUniversalTime());
    }
    

    [Fact(DisplayName = "POST - JWT expired")]
    public async Task JwtExpired()
    {
        const string email = "expired@test.ee";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar1";
        const int expiresInSeconds = 3;

        const string URL = "/api/v1/carts";

        // Arrange
        var jwt = await RegisterNewUser(email, password, firstname, lastname, expiresInSeconds);
        var jwtResponse = JsonSerializer.Deserialize<JwtResponse>(jwt, _camelCaseJsonSerializerOptions);


        var request = new HttpRequestMessage(HttpMethod.Get, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JwtToken);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        // Arrange
        await Task.Delay((expiresInSeconds + 2) * 1000);

        var request2 = new HttpRequestMessage(HttpMethod.Get, URL);
        request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JwtToken);

        // Act
        var response2 = await _client.SendAsync(request2);

        // Assert
        Assert.False(response2.IsSuccessStatusCode);
    }

    [Fact(DisplayName = "POST - JWT renewal")]
    public async Task JwtRenewal()
    {
        const string email = "renewal@test.ee";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar1";
        const int expiresInSeconds = 3;

        const string URL = "/api/v1/carts";

        // Arrange
        var jwt = await RegisterNewUser(email, password, firstname, lastname, expiresInSeconds);
        var jwtResponse = JsonSerializer.Deserialize<JwtResponse>(jwt, _camelCaseJsonSerializerOptions);

        // let the jwt expire
        await Task.Delay((expiresInSeconds + 2) * 1000);

        var request = new HttpRequestMessage(HttpMethod.Get, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JwtToken);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);

        // Arrange
        var REFRESH_URL = $"/api/v1/identity/account/refreshtoken?expiresInSeconds={expiresInSeconds}";
        var refreshData = new
        {
            JWT = jwtResponse.JwtToken,
            RefreshToken = jwtResponse.RefreshToken,
        };

        var data = JsonContent.Create(refreshData);

        var response2 = await _client.PostAsync(REFRESH_URL, data);
        var responseContent2 = await response2.Content.ReadAsStringAsync();

        Assert.True(response2.IsSuccessStatusCode);

        jwtResponse = JsonSerializer.Deserialize<JwtResponse>(responseContent2, _camelCaseJsonSerializerOptions);

        var request3 = new HttpRequestMessage(HttpMethod.Get, URL);
        request3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JwtToken);

        // Act
        var response3 = await _client.SendAsync(request3);
        // Assert
        Assert.True(response3.IsSuccessStatusCode);
    }
}