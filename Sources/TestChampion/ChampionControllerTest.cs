using Api.Controllers;
using ClientApi;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Model;
using StubLib;

namespace TestChampion
{
    [TestClass]
    public class ChampionControllerTest
    {
        [TestMethod]
        public async Task TestGetChampion()
        {
            IDataManager stubData = new StubData();
            ChampionsController controller = new ChampionsController(stubData);
            var championResult = await controller.GetAll();

            var objectValue =  championResult as ObjectResult;
            Assert.IsNotNull(objectValue);

            var valeur = objectValue.Value as IEnumerable<DTOChampion>;
            Assert.IsNotNull(valeur);

            var expected = valeur.Count();
            var actual = stubData.ChampionsMgr.GetNbItems().Result;

            Assert.AreEqual(expected,actual);
        }
    }
}