using CryptoAPI.Controllers;
using CryptoAPI.Models;
using CryptoAPI.Repository_pattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class UserControllerTests
    {
        /// <summary>
        /// The system under testing (UserController) includes following unit tests
        /// 
        /// GetUsers_WhenDBIsEmpty_ReturnEmptyUserList
        /// GetUsers_WhenDBIsNOTEmpty_ReturnUsers
        /// GetUserById_WithexistingId_ReturnsUser
        /// GetUserById_WithUnexistingId_ReturnsNotFound
        /// GetUserDetails_WithUnexistingId_ReturnsNotFound
        /// PutUser_WithDifferentIds_ReturnsBadRequest
        /// PutUser_WithUnexsitingId_ReturnsNotFound
        /// PutUser_ModifiedSuccess_ReturnsNoContent -> To be fixed
        /// PostUser_NewUser_ReturnsCreatedAtAction
        /// DeleteUser_UnexistingUser_ReturnsNotFound
        /// DeleteUser_ExistingUser_ReturnsNoContent
        /// Login_UnexistingUser_ReturnsNotFound
        /// </summary>
        
        private int numUsers;
        private List<User> LoadTestData()
        {
            var userList = new List<User>
                {
                new User { UserId = 1, Name = "Oscar", RoleId = 1, Email = "Oscar@gmail.com", Password = "pass123", ThemeId = 1, ReceiveNotifications = true},
                new User { UserId = 2, Name = "Anass", RoleId = 1, Email = "Anass@gmail.com", Password = "pass321", ThemeId = 1, ReceiveNotifications = false}
                };
            numUsers = userList.Count;
            return userList;
        }

        [Fact]
        public async Task GetUsers_WhenDBIsEmpty_ReturnEmptyUserList()
        {
            // Arrange
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUsersAsync())
                .ReturnsAsync(new List<User>());
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var users = await controller.GetUsers();

            // Assert
            repositoryMock.Verify(r => r.GetUsersAsync());
            var usersEnumerable = (users.Result as OkObjectResult).Value as IEnumerable<User>;
            Assert.Equal(0, usersEnumerable.Count());
        }

        [Fact]
        public async Task GetUsers_WhenDBIsNOTEmpty_ReturnUsers()
        {
            // Arrange
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUsersAsync())
                .ReturnsAsync(listOfUsers);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var users = await controller.GetUsers();

            // Assert
            repositoryMock.Verify(r => r.GetUsersAsync());
            var usersEnumerable = (users.Result as OkObjectResult).Value as IEnumerable<User>;
            Assert.Equal(numUsers, usersEnumerable.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetUserById_WithexistingId_ReturnsUser(int userId)
        {
            // Arrange
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUserByIDAsync(userId))
                .ReturnsAsync(listOfUsers.FirstOrDefault(x => x.UserId == userId));
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var user = await controller.GetUserByID(userId);

            // Assert
            Assert.IsType<OkObjectResult>(user.Result);
            var userResult = user.Result as OkObjectResult;
            Assert.Equal(200, userResult.StatusCode);
            var userFromActionResult = userResult.Value as User;
            Assert.NotNull(userFromActionResult);
            repositoryMock.Verify(r => r.GetUserByIDAsync(userId));
            Assert.Equal(userId, userFromActionResult.UserId);

        }

        [Fact]
        public async Task GetUserById_WithUnexistingId_ReturnsNotFound()
        {
            // Arrange
            int unexistingUserId = 1;
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUserByIDAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var user = await controller.GetUserByID(unexistingUserId);

            // Assert
            repositoryMock.Verify(r => r.GetUserByIDAsync(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(user.Result);
        }

        [Fact]
        public async Task GetUserDetails_WithUnexistingId_ReturnsNotFound()
        {
            // Arrange
            int unexistingUserId = 1;
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUserDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var user = await controller.GetUserDetails(unexistingUserId);

            // Assert
            repositoryMock.Verify(r => r.GetUserDetailsAsync(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(user.Result);
        }

        [Fact]
        public async Task PutUser_WithDifferentIds_ReturnsBadRequest()
        {
            // Arrange
            int unexistingIdinPath = 1;
            int unexistingIdinBody = 2;
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUsersAsync())
                .ReturnsAsync(listOfUsers);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            User updatedUser = new User { UserId = unexistingIdinBody, Name = "Alice", RoleId = 1, Email = "Alice@gmail.com", Password = "pass789", ThemeId = 2, ReceiveNotifications = false };
            var actionResult = await controller.PutUser(unexistingIdinPath, updatedUser);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public async Task PutUser_WithUnexsitingId_ReturnsNotFound()
        {
            // Arrange
            int unexistingId = 15;
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUsersAsync())
                .ReturnsAsync(listOfUsers);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            User updatedUser = new User { UserId = unexistingId, Name = "Alice", RoleId = 1, Email = "Alice@gmail.com", Password = "pass789", ThemeId = 2, ReceiveNotifications = false };
            var actionResult = await controller.PutUser(unexistingId, updatedUser);

            // Assert
            repositoryMock.Verify(r => r.UserExists(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(actionResult);
        }

        //[Fact]
        //public async Task PutUser_ModifiedSuccess_ReturnsNoContent()
        //{
        //    // Arrange
        //    int existingId = 1;
        //    List<User> listOfUsers = LoadTestData();
        //    var repositoryMock = new Mock<ICryptoRepository>();
        //    repositoryMock
        //        .Setup(r => r.GetUsersAsync())
        //        .ReturnsAsync(listOfUsers);
        //    var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

        //    var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

        //    // Act
        //    User updatedUser = new User { UserId = existingId, Name = "Alice", RoleId = 1, Email = "Alice@gmail.com", Password = "pass789", ThemeId = 2, ReceiveNotifications = false };
        //    var actionResult = await controller.PutUser(existingId, updatedUser);

        //    // Assert
        //    Assert.IsType<NoContentResult>(actionResult);
        //}

        [Fact]
        public async Task PostUser_NewUser_ReturnsCreatedAtAction()
        {
            // Arrange
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUsersAsync())
                .ReturnsAsync(listOfUsers);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            User newUser = new User { UserId = ++numUsers, Name = "Alice", RoleId = 1, Email = "Alice@gmail.com", Password = "pass789", ThemeId = 2, ReceiveNotifications = false };
            
            // Act
            var user = await controller.PostUser(newUser);

            // Assert
            repositoryMock.Verify(r => r.AddUser(It.IsAny<User>()));
            Assert.IsType<CreatedAtActionResult>(user.Result);

        }

        [Fact]
        public async Task DeleteUser_UnexistingUser_ReturnsNotFound()
        {
            // Arrange
            int unexistingId = 15;
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUserByIDAsync(unexistingId))
                .ReturnsAsync(listOfUsers.FirstOrDefault(x => x.UserId == unexistingId));
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var actionResult = await controller.DeleteUser(unexistingId);

            // Assert
            repositoryMock.Verify(r => r.GetUserByIDAsync(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            int existingId = 1;
            List<User> listOfUsers = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUserByIDAsync(existingId))
                .ReturnsAsync(listOfUsers.FirstOrDefault(x => x.UserId == existingId));
            repositoryMock
                .Setup(r => r.DeleteUser(listOfUsers.FirstOrDefault(x => x.UserId == existingId)));
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            // Act
            var actionResult = await controller.DeleteUser(existingId);

            // Assert
            repositoryMock.Verify(r => r.DeleteUser(It.IsAny<User>()));
            repositoryMock.Verify(r => r.SaveChangesAsync());
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task Login_UnexistingUser_ReturnsNotFound()
        {
            // Arrange
            User unexistingUser = new User { UserId = ++numUsers, Name = "Alice", RoleId = 1, Email = "Alice@gmail.com", Password = "pass789", ThemeId = 2, ReceiveNotifications = false };
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetUserWithTokenAsync(unexistingUser))
                .ReturnsAsync((User)null);
            var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

            var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

            //User unexistingUser = new User { UserId = ++numUsers, Name = "Alice", RoleId = 1, Email = "Alice@gmail.com", Password = "pass789", ThemeId = 2, ReceiveNotifications = false };

            // Act
            var userWithToken = await controller.Login(unexistingUser);

            // Assert
            repositoryMock.Verify(r => r.GetUserWithTokenAsync(unexistingUser));
            Assert.IsType<NotFoundResult>(userWithToken.Result);
        }

        //[Fact]
        //public async Task Login_ExistingUser_ReturnsUserWithToken()
        //{
        //    // Arrange
        //    int existingId = 1;
        //    List<User> listOfUsers = LoadTestData();
        //    var repositoryMock = new Mock<ICryptoRepository>();
        //    repositoryMock
        //        .Setup(r => r.GetUserWithTokenAsync(listOfUsers.FirstOrDefault(x => x.UserId == existingId)));
        //    var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();

        //    var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

        //    // Act
        //    var userWithToken = await controller.Login(listOfUsers.FirstOrDefault(x => x.UserId == existingId)); // this still return NotFound as Login in the controller return a null in user after doing include query

        //    // Assert
        //    repositoryMock.Verify(r => r.GetUserWithTokenAsync(It.IsAny<User>()));
        //    var userResult = (userWithToken.Result as OkObjectResult).Value as UserWithToken;
        //    Assert.IsType<UserWithToken>(userResult);
        //}

        //[Fact]
        //public async Task RefreshToken_UnexistingUser_ReturnsNull()
        //{
        //    // Arrange
        //    List<User> listOfUsers = LoadTestData();
        //    var repositoryMock = new Mock<ICryptoRepository>();
        //    repositoryMock
        //        .Setup(r => r.GetUsersAsync())
        //        .ReturnsAsync(listOfUsers);
        //    var _JWTSettingsMock = new Mock<IOptions<JWTSettings>>();
        //    //_JWTSettingsMock
        //    //    .Setup(r => r.Value)
        //    //    .Returns(new JWTSettings { });

        //    var controller = new UsersControllerWithRepository(repositoryMock.Object, _JWTSettingsMock.Object);

        //    RefreshRequest refreshRequest = new RefreshRequest { AccessToken = "xBqttLjfWKcOVeZvg3KwkfTvm9QpVSLzeNyh50fFTA4=", RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTEiLCJuYmYiOjE2NTUzNjUzMjUsImV4cCI6MTY1NTM4NjkyNSwiaWF0IjoxNjU1MzY1MzI1fQ.3l-ceFY8bm2MC21YuFybEmSvdrUGpscdZ-VEwaYkZoA" };

        //    // Act
        //    var userWithToken = await controller.RefreshToken(refreshRequest);  // this is not returning the user probably because of JWTSettings in Mock object

        //    // Assert
        //    var userWithTokenResult = (userWithToken.Result as OkObjectResult).Value as UserWithToken;
        //    Assert.NotNull(userWithTokenResult);
        //}


    }
}
