using Api.Controllers;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Model;
using Moq;
using StubLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace TestControllerApiUt
{
    [TestClass]
    public class SkinControllerTest
    {
        private readonly IDataManager _stubData = new StubData();
        private readonly ILogger<SkinsController> _logger = new NullLogger<SkinsController>();

        [TestMethod]
        public async Task Delete_ReturnBadRequest()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "NonExistingSkin";

            // Act
            var result = await controller.Delete(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ReturnOk()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "Skin1";
            var rune = new DTOSkin { Name = name, Description="test", ChampionName="Annie", Icon="test", Image="test", Price = 3};
            var test = await controller.Post(rune);

            // Act
            var result = await controller.Delete(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.RunesMgr.GetItemByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new SkinsController(mockDataManager.Object, _logger);
            var name = "ExistingRune";

            // Act
            var result = await controller.Delete(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.RunesMgr.GetNbItemsByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new SkinsController(mockDataManager.Object, _logger);
            var champion = new DTOSkin { Name = "test", Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };

            // Act
            var result = await controller.Put("ExistingChampion", champion);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Get_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.SkinsMgr.GetItemByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new SkinsController(mockDataManager.Object, _logger);

            // Act
            var result = await controller.Get("ExistingChampion");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.RunesMgr.GetNbItems()).Throws(new Exception("Erreur de base de données"));

            var controller = new SkinsController(mockDataManager.Object, _logger);
            var champion = new DTOSkin { Name="test", Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };

            // Act
            var result = await controller.Post(champion);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_ReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "Skin1";

            // Act
            var invalidRune = new DTOSkin { };
            var putResult = await controller.Put(name, invalidRune);
            var objectResult = (ObjectResult)putResult;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_ReturnBadRequest_WhenRuneDataIsIncomplete()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "Skin1";

            // Act
            var incompleteRune = new DTOSkin { Name = name, Description = "", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };
            var putResult = await controller.Put(name, incompleteRune);
            var objectResult = (ObjectResult)putResult;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_ReturnNotFound()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "NonExistingRune";
            var rune = new DTOSkin { Name = name, Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };

            // Act
            var putResult = await controller.Put(name, rune);
            var objectResult = (ObjectResult)putResult;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_ReturnOk()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTOSkin { Name = name, Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };
            var result = await controller.Post(rune);

            // Act
            var updatedRune = new DTOSkin { Name = name, Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };
            var putResult = await controller.Put(name, updatedRune);
            var objectResult = (ObjectResult)putResult;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_ReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var rune = new DTOSkin {   };

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Post_ReturnBadRequest_WhenRuneDataIsIncomplete()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var rune = new DTOSkin { Name = "", Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetSkins_ReturnsNotFoundNameRune()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);

            // Act
            var result = await controller.GetSkins(startIndex: 2, count: 5, descending: false, nameSubstring: "testtest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetSkins_ReturnsNotFoundChampion()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);

            // Act
            var result = await controller.GetSkins(startIndex: 2, count: 5, descending: false, champion: "testtest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetSkins_ReturnsOkSkins()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);

            // Act
            var result = await controller.GetSkins(startIndex: 2, count: 5, descending: false);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

        }




        [TestMethod]
        public async Task GetAll_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.SkinsMgr.GetNbItems()).Throws(new Exception("Erreur de base de données"));

            var controller = new SkinsController(mockDataManager.Object, _logger);
            var name = "ExistingRune";

            // Act
            var result = await controller.GetSkins(startIndex: 5, count: 5);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_ReturnBadRequest_WhenRuneAlreadyExists()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var rune = new DTOSkin { Name = "Skin1", Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };
            await controller.Post(rune);

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Post_ReturnCreated_WhenRuneAddedSuccessfully()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var rune = new DTOSkin { Name = "Skin1", Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.Created, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_ReturnBadRequest()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "";

            // Act
            var result = await controller.Get(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task GetSkins_ReturnsBadRequest()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);

            // Act
            var result = await controller.GetSkins(startIndex: -1, count: 26);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Get_ReturnNotFound()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "NonExistingRune";

            // Act
            var result = await controller.Get(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_ReturnOk()
        {
            // Arrange
            var controller = new SkinsController(_stubData, _logger);
            var name = "Skin1";
            var rune = new DTOSkin { Name = name, Description = "test", ChampionName = "Annie", Icon = "test", Image = "test", Price = 3 };
            var test = await controller.Post(rune);

            // Act
            var result = await controller.Get(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }
    }
}
