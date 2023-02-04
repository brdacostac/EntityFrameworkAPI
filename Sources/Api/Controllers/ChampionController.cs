using Api.Mapper;
using DTOLol;
using Microsoft.AspNetCore.Mvc;
using Model;
using StubLib;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChampionController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        public ChampionController(IDataManager dataManger)
        {
            _dataManager = dataManger;
        }

        // GET: api/<ValuesController>
        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
           
            return StatusCode((int)HttpStatusCode.OK, (await _dataManager.ChampionsMgr.GetItems(0, await _dataManager.ChampionsMgr.GetNbItems())).Select(e => e.ToDto()));
            //return  Ok( championListDto);
        }

        [HttpGet( "Get/{page}-{nbItem}")]
        public async Task<IActionResult> GetPage(int page,int nbItem)
        {

            if(page < 0 || nbItem < 0 )
                //badRequest?
            {
                return StatusCode((int)HttpStatusCode.RequestedRangeNotSatisfiable, "Numero de page ou nombre d'item est negatif");
            }
            int nbItemTotal = await _dataManager.ChampionsMgr.GetNbItems();

            if (page >= nbItemTotal/nbItem)
            {
                return StatusCode((int)HttpStatusCode.RequestedRangeNotSatisfiable, "Numero de page est trop grand");
            }
            IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(page, nbItem);

            List<DTOChampion> championListDto = new List<DTOChampion>();

            championList.ToList().ForEach(champion => championListDto.Add(champion.ToDto()));
            return StatusCode((int)HttpStatusCode.OK, championListDto);
            //return  Ok( championListDto);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            Champion champion = await _dataManager.ChampionsMgr.GetItemByName(name, null);
            if (champion == null)
                return StatusCode((int)HttpStatusCode.NotFound, "Le champion n'est pas existant");
            return StatusCode((int)HttpStatusCode.OK, champion);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOChampion champion)
        {
            if (string.IsNullOrWhiteSpace(champion.Name) || string.IsNullOrWhiteSpace(champion.Image) || string.IsNullOrWhiteSpace(champion.Bio) || string.IsNullOrWhiteSpace(champion.Class) || string.IsNullOrWhiteSpace(champion.Icon))
                return StatusCode((int)HttpStatusCode.BadRequest, "Les données du champion sont incomplètes");

            int nbItemTotal = await _dataManager.ChampionsMgr.GetNbItems();
            IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(0, nbItemTotal);

            if (championList.Any(championExist => championExist.Name == champion.Name  || championExist.Bio == champion.Bio))
                return StatusCode((int)HttpStatusCode.BadRequest, "Le champion existe déjà");

            _dataManager.ChampionsMgr.AddItem(champion.ToChampion());

            return StatusCode((int)HttpStatusCode.Created, "Le champion a été créé");
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DTOChampion champion)
        {
            int nbItemByName = await _dataManager.ChampionsMgr.GetNbItemsByName(champion.Name);
            if (nbItemByName == 0)
                return StatusCode((int)HttpStatusCode.NotFound, "Le champion n'existe pas.");

            Champion championDelete = await _dataManager.ChampionsMgr.GetItemByName(champion.Name,null);
            await _dataManager.ChampionsMgr.UpdateItem(championDelete, champion.ToChampion());
            return StatusCode((int)HttpStatusCode.OK, "Le champion a été modifié.");
            //no content
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            Champion championDelete = await _dataManager.ChampionsMgr.GetItemByName(name,null);
            if(championDelete == null)
                return StatusCode((int)HttpStatusCode.NotFound, "Le champion n'est pas existant");

            return StatusCode((int)HttpStatusCode.OK, "Le champion a été supprimé.");
        }

    }
}
