using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Net;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunePageController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        public RunePageController(IDataManager dataManger)
        {
            _dataManager = dataManger;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(0, await _dataManager.ChampionsMgr.GetNbItems());
                if (championList.Count() == 0) return StatusCode((int)HttpStatusCode.NoContent);
                return StatusCode((int)HttpStatusCode.OK, championList.Select(e => e.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des champions"));
            }

        }
    }
}
