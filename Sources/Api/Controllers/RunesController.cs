using Api.Mapper;
using DTOLol;
using DTOLol.Factory;
using Microsoft.AspNetCore.Mvc;
using Model;
using StubLib;
using System.Net;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunesController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        public RunesController(IDataManager dataManger)
        {
            _dataManager = dataManger;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItems(0, await _dataManager.RunesMgr.GetNbItems());
                if (runeList.Count() == 0) return StatusCode((int)HttpStatusCode.NoContent);
                return StatusCode((int)HttpStatusCode.OK, runeList.Select(e => e.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des runes"));
            }
        }

        [HttpGet("page/{page}/items/{nbItem}")]
        public async Task<IActionResult> GetPage(int page, int nbItem)
        {
            if (page < 0 || nbItem < 0)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Numero de page ou nombre d'item est negatif"));//changer le 416
            }

            int nbItemTotal = await _dataManager.RunesMgr.GetNbItems();

            if (page >= nbItemTotal / nbItem)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Numero de page est trop grand")); // mettre le message d'error en json
            }

            try
            {
                IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItems(page, nbItem);
                List<DTORune> runeListDto = (List<DTORune>)runeList.Select(rune => rune.ToDto());
                return Ok(runeListDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la récupération des données." });
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try { 
                if (string.IsNullOrEmpty(name))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Le nom de la rune ne peut pas être vide."));
                }
                Rune rune = await _dataManager.RunesMgr.GetItemByName(name);
                if (rune == null)
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate("La rune n'est pas existant"));
                return StatusCode((int)HttpStatusCode.OK, rune.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la récupération des données." });
            }
        }

    }
}
