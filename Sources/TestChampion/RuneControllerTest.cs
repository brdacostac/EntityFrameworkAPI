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
    public class RuneControllerTest
    {
        private readonly IDataManager _stubData = new StubData();
        private readonly ILogger<RunesController> _logger = new NullLogger<RunesController>();

        //[TestMethod]
        //public async Task GetRunes_WithInvalidStartIndex_ReturnsBadRequest()
        //{
        //    // Arrange
        //    RunesController controller = new RunesController(_stubData, _logger);

        //    // Act
        //    var result = await controller.GetRunes(-1, 3);
        //    var objectResult = (ObjectResult)result;

        //    // Assert
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        //}

        //[TestMethod]
        //public async Task GetRunes_WithInvalidCount_ReturnsBadRequest()
        //{
        //    // Arrange
        //    RunesController controller = new RunesController(_stubData, _logger);

        //    // Act
        //    var result = await controller.GetRunes(0, 26);
        //    var objectResult = (ObjectResult)result;

        //    // Assert
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        //}

        [TestMethod]
        public async Task Delete_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.RunesMgr.GetItemByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new RunesController(mockDataManager.Object, _logger);
            var name = "ExistingRune";

            // Act
            var result = await controller.Delete(name);
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
            mockDataManager.Setup(x => x.RunesMgr.GetNbItems()).Throws(new Exception("Erreur de base de données"));

            var controller = new RunesController(mockDataManager.Object, _logger);

            // Act
            var result = await controller.GetRunes(startIndex:1, count: 1);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task GetRunes_ReturnsNotFoundNameRune()
        {
            // Arrange
            var controller = new RunesController(_stubData, _logger);

            // Act
            var result = await controller.GetRunes(startIndex: 2, count: 5, descending: false, runeFamily: "testtest");
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetRunes_ReturnsOk()
        {
            // Arrange
            var controller = new RunesController(_stubData, _logger);

            // Act
            var result = await controller.GetRunes(startIndex: 1, count: 1, descending: false);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task GetRunePages_ReturnsBadRequest()
        {
            // Arrange
            var controller = new RunesController(_stubData, _logger);

            // Act
            var result = await controller.GetRunes(startIndex: -1, count: 26);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Put_ReturnInternalServerError()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.RunesMgr.GetNbItemsByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new RunesController(mockDataManager.Object, _logger);
            var champion = new DTORune { Name = "test", Description = "test", Family = "Unknown", Icon = "test", Image = "test" };

            // Act
            var result = await controller.Put("ExistingChampion", champion);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Get_ReturnInternalServerErro()
        {
            // Arrange
            var mockDataManager = new Mock<IDataManager>();
            mockDataManager.Setup(x => x.RunesMgr.GetItemByName(It.IsAny<string>())).Throws(new Exception("Erreur de base de données"));

            var controller = new RunesController(mockDataManager.Object, _logger);

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

            var controller = new RunesController(mockDataManager.Object, _logger);
            var champion = new DTORune { Name = "test", Description = "test", Family = "Unknown", Icon = "test", Image = "test" };

            // Act
            var result = await controller.Post(champion);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        }


        [TestMethod]
        public async Task Delete_ReturnBadRequest()
        {
            // Arrange
            RunesController controller = new RunesController(_stubData, _logger);
            var name = "NonExistingRune";

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
            RunesController controller = new RunesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORune { Name = name, Description = "test", Family = "Unknown", Icon = "test", Image = "test" };
            var test = await controller.Post(rune);

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
            var controller = new RunesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORune { Name = name, Description = "test", Family = "Unknown", Icon = "test", Image = "test" };
            var result = await controller.Post(rune);

            // Act
            var invalidRune = new DTORune { Name = name, Description = "", Family = "Unknown", Icon = "test", Image = "test" };
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
            var controller = new RunesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORune { Name = name, Description = "test", Family = "Unknown", Icon = "test", Image = "test" };
            var result = await controller.Post(rune);

            // Act
            var incompleteRune = new DTORune { Name = name, Description = "test", Family = "", Icon = "test", Image = "test" };
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
            var controller = new RunesController(_stubData, _logger);
            var name = "NonExistingRune";
            var rune = new DTORune { Name = name, Description = "test", Family = "Unknown", Icon = "test", Image = "test" };

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
            var controller = new RunesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORune { Name = name, Description = "test", Family = "Unknown", Icon = "test", Image = "test" };
            var result = await controller.Post(rune);

            // Act
            var updatedRune = new DTORune { Name = name, Description = "test2", Family = "Unknown", Icon = "test2", Image = "test2" };
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
            var controller = new RunesController(_stubData, _logger);
            controller.ModelState.AddModelError("test", "error");
            var rune = new DTORune { Name = "Rune1", Description = "test", Family = "Unknown", Icon = "test", Image = "test" };

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
            var controller = new RunesController(_stubData, _logger);
            var rune = new DTORune { Name = "", Description = "test", Family = "Unknown", Icon = "test", Image = "test" };

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        }

        [TestMethod]
        public async Task Post_ReturnBadRequest_WhenRuneAlreadyExists()
        {
            // Arrange
            var controller = new RunesController(_stubData, _logger);
            var rune = new DTORune { Name = "Rune1", Description = "test", Family = "Unknown", Icon = "test", Image = "test" };
            await controller.Post(rune);

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            await controller.Delete("Rune1");

        }

        [TestMethod]
        public async Task Post_ReturnCreated_WhenRuneAddedSuccessfully()
        {
            // Arrange
            var controller = new RunesController(_stubData, _logger);
            var rune = new DTORune { Name = "Rune1", Description = "test", Family = "Unknown", Icon = "test", Image = "test" };

            // Act
            var result = await controller.Post(rune);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.Created, objectResult.StatusCode);
            await controller.Delete("Rune1");
        }

        [TestMethod]
        public async Task Get_ReturnBadRequest()
        {
            // Arrange
            RunesController controller = new RunesController(_stubData, _logger);
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
            RunesController controller = new RunesController(_stubData, _logger);
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
            RunesController controller = new RunesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORune { Name = name, Description = "test", Family = "Unknown", Icon = "test", Image = "test" };
            var test = await controller.Post(rune);

            // Act
            var result = await controller.Get(name);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
            await controller.Delete(name);
        }
    }
}
