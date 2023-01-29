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
            IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(0, await _dataManager.ChampionsMgr.GetNbItems());

            List<DTOChampion> championListDto = new List<DTOChampion>();
            foreach (Champion champion in championList)
            {
                championListDto.Add(champion.ToDto());
            }
            return StatusCode((int)HttpStatusCode.OK, championListDto);
            //return  Ok( championListDto);
        }

        [HttpGet( "Get/{page}-{nbItem}")]
        public async Task<IActionResult> GetPage(int page,int nbItem)
        {

            if(page < 0 || nbItem < 0 )
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
            foreach (Champion champion in championList)
            {
                championListDto.Add(champion.ToDto());
            }
            return StatusCode((int)HttpStatusCode.OK, championListDto);
            //return  Ok( championListDto);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post( string Name,  string Class, string Bio ="", string Icon ="",  string Image="")
        {
            DTOChampion dTOChampion = new DTOChampion()
            {
                Name = Name,
                Bio = Bio,
                Class =Class,
                Icon = Icon,
                Image =Image
            };
            _dataManager.ChampionsMgr.AddItem(dTOChampion.ToChampion());

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
