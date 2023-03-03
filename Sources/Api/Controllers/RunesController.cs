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
        public async Task<IActionResult> GetAll(int? startIndex = null, int? count = 4, string? name = null)
        {
            try
            {
                int totalItemCount = await _dataManager.RunesMgr.GetNbItems();
                int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                int actualCount = count.HasValue ? count.Value : totalItemCount;
                IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItems(actualStartIndex, actualCount);

                if (!string.IsNullOrEmpty(name))
                {
                    runeList = runeList.Where(r => r.Name.Contains(name));
                }

                if (runeList.Count() == 0) return StatusCode((int)HttpStatusCode.NoContent);

                int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                int currentPage = actualStartIndex / actualCount + 1;
                int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                var result = new
                {
                    currentPage = currentPage,
                    nextPage = nextPage,
                    totalPages = totalPages,
                    totalCount = totalItemCount,
                    items = runeList.Select(e => e.ToDto())
                };

                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des runes"));
            }
        }


            /*[HttpGet]
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
            }*/

            /* [HttpGet("page/{page}/items/{nbItem}")]
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
             }*/

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

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTORune rune)
        {
            try { 
                if (!ModelState.IsValid)
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données de la rune ne sont pas correctes"));

                if (string.IsNullOrWhiteSpace(rune.Name) || string.IsNullOrWhiteSpace(rune.Image) || string.IsNullOrWhiteSpace(rune.Description) || string.IsNullOrWhiteSpace(rune.Family) || string.IsNullOrWhiteSpace(rune.Icon))
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données de la rune sont incomplètes"));


                int nbItemTotal = await _dataManager.RunesMgr.GetNbItems();
                IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItems(0, nbItemTotal);

                if (runeList.Any(runeExist => runeExist.Name == rune.Name))
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Cette rune existe déjà"));


                var runeResult = _dataManager.RunesMgr.AddItem(rune.ToRune());

                return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate("La rune a été créé"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la insertion des données." });
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] DTORune rune)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données de la rune sont incomplètes"));

                if (string.IsNullOrWhiteSpace(rune.Name) || string.IsNullOrWhiteSpace(rune.Image) || string.IsNullOrWhiteSpace(rune.Description) || string.IsNullOrWhiteSpace(rune.Family) || string.IsNullOrWhiteSpace(rune.Icon))
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données de la rune sont incomplètes"));

                int nbItemByName = await _dataManager.RunesMgr.GetNbItemsByName(rune.Name);
                if (nbItemByName == 0)
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate("La rune n'existe pas."));

                Rune runeUpdate = await _dataManager.RunesMgr.GetItemByName(name);
                await _dataManager.RunesMgr.UpdateItem(runeUpdate, rune.ToRune());
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate("La rune a été modifié."));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la modification des données." });
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try { 
                Rune runeDelete = await _dataManager.RunesMgr.GetItemByName(name);
                if (runeDelete == null)
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("La rune n'est pas existant"));
                _dataManager.RunesMgr.DeleteItem(runeDelete);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate("La rune a été supprimé"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la suppression des données." });
            }
        }

    }
}
