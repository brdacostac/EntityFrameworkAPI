using Api.Controllers;
using ClientApi;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Model;
using StubLib;
using System.Net;

namespace TestChampion
{
    [TestClass]
    public class ChampionControllerTest
    {
        [TestMethod]
        public async Task TestGetChampion()
        {
            IDataManager stubData = new StubData();
            ChampionController controller = new ChampionController(stubData);
            // Act
            var championResult = await controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(championResult, typeof(ObjectResult));
            var objectResult = (ObjectResult)championResult;
            Assert.IsNotNull(objectResult);
            //pas utile on vérifie le code de retour
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

            var champions = objectResult.Value as IEnumerable<DTOChampion>;
            Assert.IsNotNull(champions);

            var expectedCount = stubData.ChampionsMgr.GetNbItems().Result;
            var actualCount = champions.Count();
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public async Task TestGetPage()
        {
            IDataManager stubData = new StubData();
            ChampionController controller = new ChampionController(stubData);
            int page = 0;
            int nbItem = 2;
            int nbItemTotal = stubData.ChampionsMgr.GetNbItems().Result;

            // Test avec des entrées valides
            var championResult = await controller.GetPage(page, nbItem);
            var objectValue = (ObjectResult)championResult;
            Assert.IsNotNull(objectValue);
            Assert.AreEqual((int)HttpStatusCode.OK, objectValue.StatusCode);

            var valeur = objectValue.Value as IEnumerable<DTOChampion>;
            Assert.IsNotNull(valeur);
            Assert.AreEqual(nbItem, valeur.Count());

            // Test avec des entrées non valides (page ou nbItem < 0)
            championResult = await controller.GetPage(-1, nbItem);
            var statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            championResult = await controller.GetPage(page, -1);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            // Test avec une page trop grande
            championResult = await controller.GetPage(nbItemTotal / nbItem + 1, nbItem);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.RequestedRangeNotSatisfiable, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestGetChampionByName()
        {
            IDataManager stubData = new StubData();
            ChampionController controller = new ChampionController(stubData);

            // Test avec un nom valide
            var championResult = await controller.Get("Ashe");
            var objectResult = (ObjectResult)championResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);

            var champion = (Champion)objectResult.Value;
            Assert.IsNotNull(champion);
            Assert.AreEqual("Ashe", champion.Name);

            // Test avec un nom non valide
            championResult = await controller.Get("InvalidName");
            var statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestPost()
        {
            IDataManager stubData = new StubData();
            ChampionController controller = new ChampionController(stubData);
            // Test avec des entrées valides
            DTOChampion champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };
            var championResult = await controller.Post(champion);
            //StatusCodeResult
            var statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.Created, statusCodeResult.StatusCode);

            // Test avec des entrées non valides (données incomplètes)
            champion = new DTOChampion
            {
                Name = "",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };
            championResult = await controller.Post(champion);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            champion = new DTOChampion
            {
                Name = "Test",
                Image = "",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };
            championResult = await controller.Post(champion);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "",
                Class = "Test",
                Icon = "Test"
            };
            championResult = await controller.Post(champion);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "",
                Icon = "Test"
            };
            championResult = await controller.Post(champion);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = ""
            };
            championResult = await controller.Post(champion);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);

            // Test avec des entrées non valides (champion déjà existant)
            champion = new DTOChampion
            {
                Name = "Test",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };
            championResult = await controller.Post(champion);
            statusCodeResult = (ObjectResult)championResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestPut()
        {
            IDataManager stubData = new StubData();
            ChampionController controller = new ChampionController(stubData);
            DTOChampion champion = new DTOChampion
            {
                Name = "Akali",
                Image = "Test",
                Bio = "Test",
                Class = "Test",
                Icon = "Test"
            };
            // Test avec des entrées valides
            var result = await controller.Put(champion);
            var objectValue = (ObjectResult)result;
            Assert.IsNotNull(objectValue);
            Assert.AreEqual((int)HttpStatusCode.OK, objectValue.StatusCode);
            Assert.AreEqual("Le champion a été modifié.", objectValue.Value);

            // Test avec des entrées non valides (champion introuvable)
            champion.Name = "Inconnu";
            result = await controller.Put(champion);
            objectValue = (ObjectResult)result;
            Assert.IsNotNull(objectValue);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectValue.StatusCode);
            Assert.AreEqual("Le champion n'existe pas.", objectValue.Value);
        }

        [TestMethod]
        public async Task TestDeleteChampion()
        {
            IDataManager stubData = new StubData();
            ChampionController controller = new ChampionController(stubData);
            // Test avec un nom de champion existant
            var result = await controller.Delete("Aatrox");
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.AreEqual("Le champion a été supprimé.", objectResult.Value);

            // Test avec un nom de champion non existant
            result = await controller.Delete("Unknown Champion");
            objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
            Assert.AreEqual("Le champion n'est pas existant", objectResult.Value);
        }
    }
}