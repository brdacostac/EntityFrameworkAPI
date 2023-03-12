using Api.Controllers;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Model;
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
    public class RunePagesControllerTest
    {
        private readonly IDataManager _stubData = new StubData();
        private readonly ILogger<RunePagesController> _logger = new NullLogger<RunePagesController>();

        [TestMethod]
        public async Task Delete_ReturnBadRequest()
        {
            // Arrange
            var controller = new RunePagesController(_stubData, _logger);
            var name = "NonExistingPageRune";

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
            var controller = new RunePagesController(_stubData, _logger);
            var name = "RunePage1";
            var rune = new DTORunePage { Name = name, DTORuneDic = new Dictionary<string, DTORune>() };
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
            var controller = new RunePagesController(_stubData, _logger);
            var name = "Rune1";

            // Act
            var invalidRune = new DTORunePage { DTORuneDic = new Dictionary<string, DTORune>() };
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
            var controller = new RunePagesController(_stubData, _logger);
            var name = "Rune1";

            // Act
            var incompleteRune = new DTORunePage { Name = "", DTORuneDic = new Dictionary<string, DTORune>() };
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
            var controller = new RunePagesController(_stubData, _logger);
            var name = "NonExistingRune";
            var rune = new DTORunePage { Name = name, DTORuneDic = new Dictionary<string, DTORune>() };

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
            var controller = new RunePagesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORunePage { Name = name, DTORuneDic = new Dictionary<string, DTORune>() };
            var result = await controller.Post(rune);

            // Act
            var updatedRune = new DTORunePage { Name = name, DTORuneDic = new Dictionary<string, DTORune>() };
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
            var controller = new RunePagesController(_stubData, _logger);
            var rune = new DTORunePage { DTORuneDic = new Dictionary<string, DTORune>() };

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
            var controller = new RunePagesController(_stubData, _logger);
            var rune = new DTORunePage { Name = "", DTORuneDic = new Dictionary<string, DTORune>() };

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
            var controller = new RunePagesController(_stubData, _logger);
            var rune = new DTORunePage { Name = "test", DTORuneDic = new Dictionary<string, DTORune>() };
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
            var controller = new RunePagesController(_stubData, _logger);
            var rune = new DTORunePage { Name = "test", DTORuneDic = new Dictionary<string, DTORune>() };

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
            var controller = new RunePagesController(_stubData, _logger);
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
            var controller = new RunePagesController(_stubData, _logger);
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
            var controller = new RunePagesController(_stubData, _logger);
            var name = "Rune1";
            var rune = new DTORunePage { Name = name, DTORuneDic = new Dictionary<string, DTORune>() };
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
