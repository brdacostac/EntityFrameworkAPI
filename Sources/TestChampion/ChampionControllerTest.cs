using Api.Controllers;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Model;
using Moq;
using StubLib;
using System.Net;
using static StubLib.StubData;

namespace TestControllerApiUt
{
    [TestClass]
    public class ChampionControllerTest
    {
        private readonly IDataManager _stubData = new StubData();
        private readonly ILogger<ChampionsController> _logger = new NullLogger<ChampionsController>();

        [TestMethod]
        public async Task Delete_ReturnBadRequest()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);
            var name = "NonExistingChampion";

            // Act
            var result = await controller.Delete(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.ChampionsMgr.GetItemByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new ChampionsController(mockDataManager.Object, _logger);
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
            mockDataManager.Setup(x => x.ChampionsMgr.GetNbItemsByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new ChampionsController(mockDataManager.Object, _logger);
            var champion =  new DTOChampion { Name = "test", Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>() , Skins = new List<DTOSkin>() };

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
            mockDataManager.Setup(x => x.ChampionsMgr.GetItemByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new ChampionsController(mockDataManager.Object, _logger);

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
            mockDataManager.Setup(x => x.ChampionsMgr.GetNbItems()).Throws(new Exception("Erreur de base de données"));

            var controller = new ChampionsController(mockDataManager.Object, _logger);
            var champion = new DTOChampion { Name = "test", Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };

            // Act
            var result = await controller.Post(champion);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ReturnOk()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);
            var name = "Champion1";
            var rune = new DTOChampion { Name = name, Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>() , Skins = new List<DTOSkin>() };
            await controller.Post(rune);

            // Act
            var result = await controller.Delete(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_ReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);
            var name = "Champion1";

            // Act
            var invalidRune = new DTOChampion {  Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };
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
            var controller = new ChampionsController(_stubData, _logger);
            var name = "Skin1";

            // Act
            var incompleteRune = new DTOChampion { Name = "", Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };
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
            var controller = new ChampionsController(_stubData, _logger);
            var name = "NonExistingRune";
            var rune = new DTOChampion { Name = name, Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };

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
            var controller = new ChampionsController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTOChampion { Name = name, Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };
            var result = await controller.Post(rune);

            // Act
            var updatedRune = new DTOChampion { Name = name, Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };
            var putResult = await controller.Put(name, updatedRune);
            var objectResult = (ObjectResult)putResult;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }



        //[TestMethod]
        //public async Task Post_ReturnBadRequest_WhenModelStateIsInvalid()
        //{
        //    // Arrange
        //    var controller = new ChampionsController(_stubData, _logger);
        //    var rune = new DTOChampion { Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };

        //    // Act
        //    var result = await controller.Post(rune);
        //    var objectResult = (ObjectResult)result;

        //    // Assert
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        //}

        //[TestMethod]
        //public async Task Post_ReturnBadRequest_WhenRuneDataIsIncomplete()
        //{
        //    // Arrange
        //    var controller = new ChampionsController(_stubData, _logger);
        //    var rune = new DTOChampion { Name = "", Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };

        //    // Act
        //    var result = await controller.Post(rune);
        //    var objectResult = (ObjectResult)result;

        //    // Assert
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        //}

        [TestMethod]
        public async Task Post_ReturnBadRequest_WhenRuneAlreadyExists()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);
            var rune = new DTOChampion { Name = "Champ1", Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };
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
            var controller = new ChampionsController(_stubData, _logger);
            var rune = new DTOChampion { Name = "Champ1", Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };

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
            var controller = new ChampionsController(_stubData, _logger);
            var name = "";

            // Act
            var result = await controller.Get(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_ReturnNotFound()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);
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
            var controller = new ChampionsController(_stubData, _logger);
            var name = "Skin1";
            var rune = new DTOChampion { Name = name, Bio = "test", Icon = "test", Image = "test", Characteristics = new Dictionary<string, int>(), Class = "Unknown", Skills = new List<DTOSkill>(), Skins = new List<DTOSkin>() };
            var test = await controller.Post(rune);

            // Act
            var result = await controller.Get(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task GetItems_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.ChampionsMgr.GetNbItems()).Throws(new Exception("Erreur de base de données"));

            var controller = new ChampionsController(mockDataManager.Object, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: 1, count: 1);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task GetChampions_ReturnsBadRequest()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: -1, count: 26);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetChampions_ReturnsOk()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: 1, count: 1, descending: false);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetChampions_ReturnsNotFoundCharacter()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: 2, count: 5, descending: false, characteristic: "tesettest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetChampions_ReturnsNotFoundChampClass()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: 2, count: 5, descending: false, championClass: "testtest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetChampions_ReturnsNotFoundSkillName()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: 2, count: 5, descending: false, skillName: "testtest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetChampions_ReturnsNotFoundName()
        {
            // Arrange
            var controller = new ChampionsController(_stubData, _logger);

            // Act
            var result = await controller.GetChampions(startIndex: 2, count: 5, descending: false, name: "testtest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }
    }
}