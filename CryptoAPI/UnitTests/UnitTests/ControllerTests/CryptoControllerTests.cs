using CryptoAPI.Controllers;
using CryptoAPI.Models;
using CryptoAPI.Repository_pattern;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class CryptoControllerTests
    {
        /// <summary>
        /// The system under testing (UserController) includes following unit tests
        /// GetAllCryptos_WhenDBIsEmpty_ReturnsEmptyList
        /// GetAllCryptos_WhenDBIsNOTEmpty_ReturnsAllItems
        /// GetCryptoByID_WithexistingId_ReturnsCryptoValue
        /// GetCryptoByID_WithUnexistingId_ReturnsNotFound
        /// GetCryptoByName_WithExistingName_ReturnCryptoValue
        /// GetCryptoByName_WithUnexistingName_ReturnNotFound
        /// </summary>

        private int numCrypto;
        private List<Crypto> LoadTestData()
        {
            var cryptoList = new List<Crypto>
                {
                new Crypto { CryptoId = 1, CoupleName = "BTCUSD", Value = 0.0},
                new Crypto { CryptoId = 2, CoupleName = "USD", Value = 0.0}
                };
            numCrypto = cryptoList.Count;
            return cryptoList;
        }

        [Fact]
        public async Task GetAllCryptos_WhenDBIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetAllCryptosAsync())
                .ReturnsAsync(new List<Crypto>());

            var controller = new CryptoControllerWithRepository(repositoryMock.Object);

            // Act
            var cryptos = await controller.GetAllCryptos();

            // Assert
            repositoryMock.Verify(r => r.GetAllCryptosAsync());
            var cryptosEnumerable = (cryptos.Result as OkObjectResult).Value as IEnumerable<Crypto>;
            Assert.Equal(0, cryptosEnumerable.Count());
        }

        [Fact]
        public async Task GetAllCryptos_WhenDBIsNOTEmpty_ReturnsAllItems()
        {
            // Arrange
            List<Crypto> listOfCryptos = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetAllCryptosAsync())
                .ReturnsAsync(listOfCryptos);

            var controller = new CryptoControllerWithRepository(repositoryMock.Object);

            // Act
            var cryptos = await controller.GetAllCryptos();

            // Assert
            repositoryMock.Verify(r => r.GetAllCryptosAsync());
            var cryptosEnumerable = (cryptos.Result as OkObjectResult).Value as IEnumerable<Crypto>;
            //foreach (var crypto in cryptosEnumerable)
            //{
            //    Assert.Equal(0.0, crypto.Value);
            //}
            Assert.Equal(listOfCryptos.Count, cryptosEnumerable.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCryptoByID_WithexistingId_ReturnsCryptoValue(int cryptoId)
        {
            // Arrange
            List<Crypto> listOfCryptos = LoadTestData();
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetCryptoByIdAsync(cryptoId))
                .ReturnsAsync(listOfCryptos.FirstOrDefault(x => x.CryptoId == cryptoId));

            var controller = new CryptoControllerWithRepository(repositoryMock.Object);

            // Act
            var crypto = await controller.GetCryptoByID(cryptoId);

            // Assert
            Assert.IsType<OkObjectResult>(crypto.Result);
            var cryptoResult = crypto.Result as OkObjectResult;
            Assert.Equal(200, cryptoResult.StatusCode);
            var crypoFromActionResult = cryptoResult.Value as Crypto;
            repositoryMock.Verify(r => r.GetCryptoByIdAsync(cryptoId));
            Assert.Equal(cryptoId, crypoFromActionResult.CryptoId);
            Assert.Equal(0.0, crypoFromActionResult.Value);
        }

        [Fact]
        public async Task GetCryptoByID_WithUnexistingId_ReturnsNotFound()
        {
            // Arrange
            int unexistingCryptoId = 1;
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetCryptoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Crypto)null);

            var controller = new CryptoControllerWithRepository(repositoryMock.Object);

            // Act
            var crypto = await controller.GetCryptoByID(unexistingCryptoId);

            // Assert
            repositoryMock.Verify(r => r.GetCryptoByIdAsync(It.IsAny<int>()));
            Assert.IsType<NotFoundResult>(crypto.Result);
        }

        [Fact]
        public async Task GetCryptoByName_WithExistingName_ReturnCryptoValue()
        {
            // Arrange
            List<Crypto> listOfCryptos = LoadTestData();
            string existingCoupleName = "BTCUSD";
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetCryptoByCoupleNameAsync(existingCoupleName))
                .ReturnsAsync(listOfCryptos.FirstOrDefault(x => x.CoupleName == existingCoupleName));

            var controller = new CryptoControllerWithRepository(repositoryMock.Object);

            // Act
            var crypto = await controller.GetCryptoFromName(existingCoupleName);

            // Assert
            repositoryMock.Verify(r => r.GetCryptoByCoupleNameAsync(It.IsAny<string>()));
            var cryptoResult = (crypto.Result as OkObjectResult).Value as Crypto;
            Assert.Equal(0.0, cryptoResult.Value);
        }

        [Fact]
        public async Task GetCryptoByName_WithUnexistingName_ReturnNotFound()
        {
            // Arrange
            string unexistingCoupleName = "";
            var repositoryMock = new Mock<ICryptoRepository>();
            repositoryMock
                .Setup(r => r.GetCryptoByCoupleNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Crypto)null);

            var controller = new CryptoControllerWithRepository(repositoryMock.Object);

            // Act
            var crypto = await controller.GetCryptoFromName(unexistingCoupleName);

            // Assert
            repositoryMock.Verify(r => r.GetCryptoByCoupleNameAsync(It.IsAny<string>()));
            Assert.IsType<NotFoundResult>(crypto.Result);
        }

    }
}