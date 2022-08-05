using CryptoAPI.Models;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace IntegrationTests.Tests;

public class UsersTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    //private short _userCurrentId = 4;
    static readonly JsonSerializerSettings _json = new JsonSerializerSettings()
    {
        // DateFormatHandling = DateFormatHandling.IsoDateFormat,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        //DefaultValueHandling = DefaultValueHandling.Ignore,
        //ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
    };

    public UsersTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var clientOptions = new WebApplicationFactoryClientOptions();
        clientOptions.AllowAutoRedirect = false;
        clientOptions.BaseAddress = new Uri("https://localhost:5000");
        _client = factory.CreateClient(clientOptions);
        //_client = factory.CreateDefaultClient();
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository")]
    //[InlineData("api/roles")]
    public async Task GetUsers_ReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);
        //var response = await _client.GetAsync(":5230" + url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }



    [Theory]
    [InlineData("api/UsersControllerWithRepository/0")]
    public async Task GetUserById_ReturnNotFound(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);
        //var response = await _client.GetAsync(":5230" + url);

        // Assert
        Assert.Equal(404, (int)response.StatusCode); // NotFound Status Code
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/2", 2)]
    [InlineData("api/UsersControllerWithRepository/3", 3)]
    [InlineData("api/UsersControllerWithRepository/1", 1)]
    public async Task GetUserById_ReturnUserAndCorrectContentType(string url, short id)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);
        //var response = await _client.GetAsync(":5230" + url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        if (response != null)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(jsonString);
            User expectedUser = Utilities.GetSeedingUsers()[id - 1];
            // Assert.Equal(expectedUser, user);  //----- if not using in memory DBContext
            Assert.Equal(expectedUser.Name, user.Name);
            Assert.Equal(expectedUser.Email, user.Email);
            Assert.Equal(expectedUser.Password, user.Password);
            Assert.Equal(expectedUser.RoleId, user.RoleId);
            Assert.Equal(expectedUser.ThemeId, user.ThemeId);
            //Role role = Utilities.GetSeedingRoles()[(int)user.RoleId - 1];
            //// Assert.Equal(role, user.Role);   //----- if not using in memory DBContext
            //Assert.Equal(role.RoleDesc, user.Role.RoleDesc);
            //Assert.Equal(role.RoleId, user.Role.RoleId);
            //var theme = Utilities.GetSeedingThemes()[(int)user.ThemeId - 1];
            //// Assert.Equal(theme, user.Theme);   //----- if not using in memory DBContext
            //Assert.Equal(theme.ThemeName, user.Theme.ThemeName);
            //Assert.Equal(theme.ThemeId, user.Theme.ThemeId);
        }
    }



    [Theory]
    [InlineData("api/UsersControllerWithRepository/GetUserDetails/8")]
    public async Task GetUserDetails_ReturnNotFound(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);
        //var response = await _client.GetAsync(":5230" + url);

        // Assert
        Assert.Equal(404, (int)response.StatusCode); // NotFound Status Code
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/GetUserDetails/2", 2)]
    [InlineData("api/UsersControllerWithRepository/GetUserDetails/3", 3)]
    [InlineData("api/UsersControllerWithRepository/GetUserDetails/1", 1)]
    public async Task GetUserDetails_ReturnUserWithRoleThemeAndCorrectContentType(string url, short id)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);
        //var response = await _client.GetAsync(":5230" + url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        if (response != null)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(jsonString);
            User expectedUser = Utilities.GetSeedingUsers()[id - 1];
            // Assert.Equal(expectedUser, user);  //----- if not using in memory DBContext
            Assert.Equal(expectedUser.Name, user.Name);
            Assert.Equal(expectedUser.Email, user.Email);
            Assert.Equal(expectedUser.Password, user.Password);
            Assert.Equal(expectedUser.RoleId, user.RoleId);
            Assert.Equal(expectedUser.ThemeId, user.ThemeId);
            Role role = Utilities.GetSeedingRoles()[(int)user.RoleId - 1];
            // Assert.Equal(role, user.Role);   //----- if not using in memory DBContext
            Assert.Equal(role.RoleDesc, user.Role.RoleDesc);
            Assert.Equal(role.RoleId, user.Role.RoleId);
            Theme theme = Utilities.GetSeedingThemes()[(int)user.ThemeId - 1];
            // Assert.Equal(theme, user.Theme);   //----- if not using in memory DBContext
            Assert.Equal(theme.ThemeName, user.Theme.ThemeName);
            Assert.Equal(theme.ThemeId, user.Theme.ThemeId);
        }
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/1", 1)]
    public async Task PutUser_ReturnUserAndCorrectContentType(string url, short id)
    {
        // Arrange
        User updatedExpectedUser = new User()
        {
            UserId = 1,
            Email = "UpdateOscar@gmail.com",
            Name = "UpdateOscar",
            RoleId = 2,
            ThemeId = 1,
            Password = "Pa$$word1Update"
        };
        string contentAsString = JsonConvert.SerializeObject(updatedExpectedUser);

        var buffer = System.Text.Encoding.UTF8.GetBytes(contentAsString);
        var byteContent = new ByteArrayContent(buffer);

        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


        // Act
        // Save the User so it this test doesnt impact other tests
        var backUpResponse = await _client.GetAsync(url);

        //backUpResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        //Assert.Equal("application/json; charset=utf-8",
        //    backUpResponse.Content.Headers.ContentType.ToString());
        //User backUpUser = new User();
        //if (backUpResponse != null)
        //{
        //    var jsonString = await backUpResponse.Content.ReadAsStringAsync();
        //    backUpUser = JsonConvert.DeserializeObject<User>(jsonString);
        //}

        // Now we update the user
        var response = await _client.PutAsync(url, byteContent);
        var Updatedresponse = await _client.GetAsync(url);


        // Assert
        //response.EnsureSuccessStatusCode(); // Status Code 200-299
        //Assert.Equal("application/json; charset=utf-8",
        //    response.Content.Headers.ContentType.ToString());
        Assert.Equal(204, (int)response.StatusCode);
        if (response != null && Updatedresponse != null)
        {
            var UpdatedjsonString = await Updatedresponse.Content.ReadAsStringAsync();
            var Updateduser = JsonConvert.DeserializeObject<User>(UpdatedjsonString);
            //var jsonString = await response.Content.ReadAsStringAsync();
            //var user = JsonConvert.DeserializeObject<User>(jsonString);

            // Assert.Equal(expectedUser, user);  //----- if not using in memory DBContext
            Assert.Equal(updatedExpectedUser.Name, Updateduser.Name);
            Assert.Equal(updatedExpectedUser.Email, Updateduser.Email);
            Assert.Equal(updatedExpectedUser.Password, Updateduser.Password);
            Assert.Equal(updatedExpectedUser.RoleId, Updateduser.RoleId);
            Assert.Equal(updatedExpectedUser.ThemeId, Updateduser.ThemeId);

            //Setting Back the old user so we other tests dont fail
            //String backUpcontentAsString = JsonConvert.SerializeObject(backUpUser);

            //var backUpbuffer = System.Text.Encoding.UTF8.GetBytes(backUpcontentAsString);
            //var backUpbyteContent = new ByteArrayContent(backUpbuffer);

            //backUpbyteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var temp = await _client.PutAsync(url, backUpResponse.Content);
        }
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/1", 2)]
    public async Task PutUser_ReturnBadRequest(string url, short id)
    {
        // Arrange
        User WrongIdUser = new User()
        {
            UserId = id,
            Email = "UpdateOscar@gmail.com",
            Name = "UpdateOscar",
            RoleId = 2,
            ThemeId = 1,
            Password = "Pa$$word1Update"
        };
        string contentAsString = JsonConvert.SerializeObject(WrongIdUser);

        var buffer = System.Text.Encoding.UTF8.GetBytes(contentAsString);
        var byteContent = new ByteArrayContent(buffer);

        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PutAsync(url, byteContent);

        // Assert
        Assert.Equal(400, (int)response.StatusCode);

    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/8")]
    public async Task PutUser_ReturnNotFound(string url)
    {
        // Arrange
        User WrongIdUser = new User()
        {
            UserId = 8,
            Email = "notexisting@gmail.com",
            Name = "void",
            RoleId = 2,
            ThemeId = 1,
            Password = "Pa$$word"
        };
        string contentAsString = JsonConvert.SerializeObject(WrongIdUser);

        var buffer = System.Text.Encoding.UTF8.GetBytes(contentAsString);
        var byteContent = new ByteArrayContent(buffer);

        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        var response = await _client.PutAsync(url, byteContent);
        //var response = await _client.GetAsync(":5230" + url);

        // Assert
        Assert.Equal(404, (int)response.StatusCode); // NotFound Status Code
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/8")]
    public async Task DeleteUser_ReturnNotFound(string url)
    {
        var response = await _client.DeleteAsync(url);

        // Assert
        Assert.Equal(404, (int)response.StatusCode); // NotFound Status Code
    }


    // Should be tested last since the recreated User dot have the same Id and it makes the other tests fail
    [Theory]
    [InlineData("api/UsersControllerWithRepository/1")]
    public async Task DeleteUser_ReturnsNoContent(string url)
    {
        // Arrange 
        // Save the deleted user
        var backUpResponse = await _client.GetAsync(url);

        //backUpResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        //Assert.Equal("application/json; charset=utf-8",
        //    backUpResponse.Content.Headers.ContentType.ToString());
        //User backUpUser = new User();
        //if (backUpResponse != null)
        //{
        //    var jsonString = await backUpResponse.Content.ReadAsStringAsync();
        //    backUpUser = JsonConvert.DeserializeObject<User>(jsonString);
        //}

        // Act
        var response = await _client.DeleteAsync(url);

        // Assert
        Assert.Equal(204, (int)response.StatusCode);

        // Recreate the User for other tests
        var RecreateUser = await _client.PostAsync("api/UsersControllerWithRepository/", backUpResponse.Content);
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/Login")]
    public async Task Login_ReturnNotFound(string url)
    {
        // Arrange
        User WrongUser = new User()
        {
            UserId = 8,
            Email = "notexisting@gmail.com",
            Name = "void",
            RoleId = 2,
            ThemeId = 1,
            Password = "Pa$$word"
        };
        //{
        //    Email = "newuser@gmail.com",
        //    Name = "newuser",
        //    RoleId = 2,
        //    ThemeId = 2,
        //    Password = "newPa$$word"
        //};
        string contentAsString = JsonConvert.SerializeObject(WrongUser);

        var buffer = System.Text.Encoding.UTF8.GetBytes(contentAsString);
        var byteContent = new ByteArrayContent(buffer);

        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PostAsync(url, byteContent);

        // Assert
        Assert.Equal(404, (int)response.StatusCode); // NotFound Status Code
    }

    [Theory]
    [InlineData("api/UsersControllerWithRepository/Login", 1)]
    //[InlineData("api/UsersControllerWithRepository/Login", 2)]
    //[InlineData("api/UsersControllerWithRepository/Login", 3)]
    public async Task Login_ReturnUserWithToken(string url, short id)
    {
        // Arrange
        User oscarUser = new User()
        {
            Email = "oscar@gmail.com",
            Name = "Oscar",
            RoleId = 1,
            ThemeId = 1,
            Password = "Pa$$word1"
        };
        User expectedUser = Utilities.GetSeedingUsers()[id - 1];
        string contentAsString = JsonConvert.SerializeObject(oscarUser);

        var buffer = System.Text.Encoding.UTF8.GetBytes(contentAsString);
        var byteContent = new ByteArrayContent(buffer);

        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PostAsync(url, byteContent);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        if (response != null)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            UserWithToken user = JsonConvert.DeserializeObject<UserWithToken>(jsonString,
                new UserWithTokenConverter());

            //new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
            //User user = JsonConvert.DeserializeObject<User>(jsonString);

            // Assert.Equal(expectedUser, user);  //----- if not using in memory DBContext
            Assert.Equal(expectedUser.Name, user.Name);
            Assert.Equal(expectedUser.Email, user.Email);
            Assert.Equal(expectedUser.Password, user.Password);
            Assert.Equal(expectedUser.RoleId, user.RoleId);
            Assert.Equal(expectedUser.ThemeId, user.ThemeId);
            Assert.NotNull(user.RefreshToken);
            Assert.NotNull(user.AccessToken);

        }

    }


    [Theory]
    [InlineData("api/UsersControllerWithRepository/RegisterUser")]
    public async Task RegisterUser_ReturnUserWithToken(string url)
    {
        // Arrange
        User newUser = new User()
        {
            Email = "new@gmail.com",
            Name = "New",
            RoleId = 2,
            ThemeId = 1,
            Password = "new"
        };
        string contentAsString = JsonConvert.SerializeObject(newUser);

        var buffer = System.Text.Encoding.UTF8.GetBytes(contentAsString);
        var byteContent = new ByteArrayContent(buffer);

        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PostAsync(url, byteContent);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        if (response != null)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            UserWithToken user = JsonConvert.DeserializeObject<UserWithToken>(jsonString,
                new UserWithTokenConverter());
            //User user = JsonConvert.DeserializeObject<User>(jsonString);
            // Assert.Equal(expectedUser, user);  //----- if not using in memory DBContext
            Assert.Equal("New", user.Name);
            Assert.Equal("new@gmail.com", user.Email);
            Assert.Equal("new", user.Password);
            Assert.Equal((short)2, user.RoleId);
            Assert.Equal((short)1, user.ThemeId);
            Assert.NotNull(user.RefreshToken);
            Assert.NotNull(user.AccessToken);

            string deleteUrl = "api/UsersControllerWithRepository/" + user.UserId;
            var deleteResponse = await _client.DeleteAsync(url);
        }

    }


}