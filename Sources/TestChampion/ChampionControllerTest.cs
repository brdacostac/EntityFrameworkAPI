using Api.Controllers;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Model;
using StubLib;
using System.Net;
using static StubLib.StubData;

namespace TestChampion
{
    [TestClass]
    public class ChampionControllerTest
    {
        [TestMethod]
        public async Task TestGetChampion()
        {
            //Test OkObjectResult
            //Arrange 
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);
            var expectedCount = await stubData.ChampionsMgr.GetNbItems();

            // Act
            var championResult = await controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(championResult, typeof(ObjectResult));
            var objectResult = (ObjectResult)championResult;
            //var objectResult = (ObjectResult)championResult;
            Assert.IsNotNull(objectResult);
            //pas utile on vérifie le code de retour
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

            var champions = objectResult.Value as IEnumerable<DTOChampion>;
            Assert.IsNotNull(champions);
            Assert.AreEqual(expectedCount, champions.Count());

            //Test NoContentObjectResult
            // Arrange
            //IDataManager stubData2 = new StubData();
            //stubData2.ChampionsMgr = new ChampionsManager(new List<Champion>());
            //ChampionsController controller2 = new ChampionsController(stubData2);

            //// Act
            //var result = await controller2.GetAll();

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(ObjectResult));
            //var noContentResult = (ObjectResult)result;
            //Assert.AreEqual((int)HttpStatusCode.NoContent, noContentResult.StatusCode);

            // Arrange
            IDataManager stubData3 = new StubData();
            stubData3.ChampionsMgr = new ChampionsManager((StubData)null);
            ChampionsController controller3 = new ChampionsController(stubData3);

            // Act
            var result2 = await controller3.GetAll();

            // Assert
            Assert.IsInstanceOfType(result2, typeof(ObjectResult));
            var objectResult2 = (ObjectResult)result2;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, objectResult2.StatusCode);
        }

        //[TestMethod]
        //public async Task TestGetPage()
        //{
        //    IDataManager stubData = new StubData();
        //    ChampionsController controller = new ChampionsController(stubData);
        //    int page = 1;
        //    int nbItem = 1;
        //    int nbItemTotal = stubData.ChampionsMgr.GetNbItems().Result;

        //    // Test avec des entrées valides
        //    var championResult = await controller.GetPage(page, nbItem);
        //    var objectValue = (ObjectResult)championResult;
        //    Assert.IsNotNull(objectValue);
        //    Assert.AreEqual((int)HttpStatusCode.OK, objectValue.StatusCode);

        //    var valeur = objectValue.Value as IEnumerable<DTOChampion>;
        //    Assert.IsNotNull(valeur);
        //    Assert.AreEqual(nbItem, valeur.Count());

        //    // Test avec des entrées non valides (page ou nbItem < 0)
        //    championResult = await controller.GetPage(-1, nbItem);
        //    var statusCodeResult = (ObjectResult)championResult;
        //    Assert.IsNotNull(statusCodeResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

        //    championResult = await controller.GetPage(page, -1);
        //    statusCodeResult = (ObjectResult)championResult;
        //    Assert.IsNotNull(statusCodeResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

        //    // Test avec une page trop grande
        //    championResult = await controller.GetPage(nbItemTotal / nbItem + 1, nbItem);
        //    statusCodeResult = (ObjectResult)championResult;
        //    Assert.IsNotNull(statusCodeResult);
        //    Assert.AreEqual((int)HttpStatusCode.RequestedRangeNotSatisfiable, statusCodeResult.StatusCode);
        //}

        [TestMethod]
        public async Task TestGetChampionByName()
        {
            // Arrange
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);

            // Act
            // Test avec un nom valide
            var championResult = await controller.Get("Akali");
            var objectResult = (ObjectResult)championResult;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

            var champion = (DTOChampion)objectResult.Value;
            Assert.IsNotNull(champion);
            Assert.AreEqual("Akali", champion.Name);

            // Act
            // Test avec un nom vide
            championResult = await controller.Get("");
            var statusCodeResult = (ObjectResult)championResult;

            // Assert
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            // Act
            // Test avec un nom invalide
            championResult = await controller.Get("InvalidName");
            statusCodeResult = (ObjectResult)championResult;

            // Assert
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        [DataRow("Test", "Test", "Test", "Test", "Test", HttpStatusCode.Created)]
        [DataRow("", "Test", "Test", "Test", "Test", HttpStatusCode.BadRequest)]
        [DataRow("Test", "", "Test", "Test", "Test", HttpStatusCode.BadRequest)]
        [DataRow("Test", "Test", "", "Test", "Test", HttpStatusCode.BadRequest)]
        [DataRow("Test", "Test", "Test", "", "Test", HttpStatusCode.BadRequest)]
        [DataRow("Test", "Test", "Test", "Test", "", HttpStatusCode.BadRequest)]
        public async Task TestPost(string name, string image, string bio, string championClass, string icon, HttpStatusCode expectedStatusCode)
        {
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);

            // Arrange
            DTOChampion champion = new DTOChampion
            {
                Name = name,
                Image = image,
                Bio = bio,
                Class = championClass,
                Icon = icon
            };

            // Act
            var championResult = await controller.Post(champion);
            var statusCodeResult = (ObjectResult)championResult;

            // Assert
            Assert.AreEqual((int)expectedStatusCode, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestPost_ModelStateInvalid()
        {
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);
            controller.ModelState.AddModelError("Test", "Test");

            // Arrange
            DTOChampion champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };

            // Act
            var championResult = await controller.Post(champion);
            var statusCodeResult = (ObjectResult)championResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestPost_ChampionAlreadyExists()
        {
            // Arrange
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);

            DTOChampion champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };

            await controller.Post(champion);

            // Act
            var championResult = await controller.Post(champion);
            var statusCodeResult = (ObjectResult)championResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

        }

        [TestMethod]
        public async Task TestPost_CreateChampion()
        {
            // Arrange
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);

            DTOChampion champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };

            // Act
            var championResult = await controller.Post(champion);
            var statusCodeResult = (ObjectResult)championResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Created, statusCodeResult.StatusCode);

        }

        //[TestMethod]
        //public async Task TestUpdateChampion()
        //{
        //    // Arrange
        //    var stubData = new StubData();
        //    var controller = new ChampionsController(stubData);
        //    var championName = "Akali";
        //    var champion = new DTOChampion
        //    {
        //        Name = "Akali",
        //        Image = "ashe.png",
        //        Bio = "Ashe's Bio",
        //        Class = "Marksman",
        //        Icon = "ashe-icon.png"
        //    };
        //    var updatedChampion = new DTOChampion
        //    {
        //        Name = "Akali",
        //        Image = "new-ashe.png",
        //        Bio = "New Ashe's Bio",
        //        Class = "Assassin",
        //        Icon = "new-ashe-icon.png"
        //    };

        //    // Act
        //    var result = await controller.Put(championName, updatedChampion);
        //    var objectResult = (ObjectResult)result;

        //    // Assert
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

        //    var updatedChampionEntity = await stubData.ChampionsMgr.GetItemByName(championName);
        //    Assert.IsNotNull(updatedChampionEntity);
        //    Assert.AreEqual(updatedChampion.Image, updatedChampionEntity.Image.Base64);
        //    Assert.AreEqual(updatedChampion.Bio, updatedChampionEntity.Bio);
        //    Assert.AreEqual(updatedChampion.Class, updatedChampionEntity.Class.ToString());
        //    Assert.AreEqual(updatedChampion.Icon, updatedChampionEntity.Icon);

        //    // Test with incomplete data
        //    champion = new DTOChampion
        //    {
        //        Name = "Ashe",
        //        Image = "ashe.png",
        //        Bio = "Ashe's Bio",
        //        Class = "Marksman",
        //    };
        //    result = await controller.Put(championName, champion);
        //    objectResult = (ObjectResult)result;
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

        //    // Test with non-existent champion
        //    var nonExistentChampionName = "NonExistentChampion";
        //    result = await controller.Put(nonExistentChampionName, updatedChampion);
        //    objectResult = (ObjectResult)result;
        //    Assert.IsNotNull(objectResult);
        //    Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        //}

        [TestMethod]
        public async Task TestDeleteChampion()
        {
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);
            // Test avec un nom de champion existant
            var result = await controller.Delete("Aatrox");
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

            // Test avec un nom de champion non existant
            result = await controller.Delete("Unknown Champion");
            objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task TestDelete_ExistingChampion()
        {
            // Arrange
            var existingChampionName = "Aatrox";
            var stubData = new StubData();
            await stubData.ChampionsMgr.AddItem(new Champion(existingChampionName));
            var controller = new ChampionsController(stubData);

            // Act
            var result = await controller.Delete(existingChampionName);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

            // Vérifiez que le champion a été supprimé en appelant GetItemByName, qui devrait renvoyer null
            var deletedChampion = await stubData.ChampionsMgr.GetItemByName(existingChampionName);
            Assert.IsNull(deletedChampion);
        }

        [TestMethod]
        public async Task TestDelete_NotExistingChampion()
        {
            // Arrange
            var nonExistentChampionName = "Unknown Champion";
            var stubData = new StubData();
            var controller = new ChampionsController(stubData);

            // Act
            var result = await controller.Delete(nonExistentChampionName);
            var objectResult = (ObjectResult)result;

            // Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }
    }
}