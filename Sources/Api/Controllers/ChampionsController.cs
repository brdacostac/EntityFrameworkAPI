using Api.Mapper;
using DTOLol;
using DTOLol.Factory;
using Microsoft.AspNetCore.Mvc;
using Model;
using StubLib;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChampionsController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        private readonly ILogger<ChampionsController> _logger;
        public ChampionsController(IDataManager dataManger, ILogger<ChampionsController> logger)
        {
            _dataManager = dataManger;
            _logger = logger;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetAll(int? startIndex = null, int? count = 20, string? name = null)
        {
            try
            {

                if (count > 25) return StatusCode((int)HttpStatusCode.BadRequest);
                if (count <= 0) return StatusCode((int)HttpStatusCode.BadRequest);

                int totalItemCount = await _dataManager.ChampionsMgr.GetNbItems();
                int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                int actualCount = count.HasValue ? count.Value : totalItemCount;
                IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(actualStartIndex, actualCount);

                if (!string.IsNullOrEmpty(name))
                {
                    championList = championList.Where(r => r.Name.Contains(name));
                }

                if (championList.Count() == 0) return StatusCode((int)HttpStatusCode.NoContent);

                int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                int currentPage = actualStartIndex / actualCount + 1;
                int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                var result = new
                {
                    currentPage = currentPage,
                    nextPage = nextPage,
                    totalPages = totalPages,
                    totalCount = totalItemCount,
                    items = championList.Select(e => e.ToDto())
                };

                return StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des champions"));
            }
        }

        //[HttpGet( "{page}/{nbItem}")]//?nbpage=
        //public async Task<IActionResult> GetPage(int page,int nbItem)
        //{

        //    //badRequest?
        //    if (page < 0 || nbItem < 0 )
        //    {
        //        return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Numero de page ou nombre d'item est negatif"));//changer le 416
        //    }
        //    int nbItemTotal = await _dataManager.ChampionsMgr.GetNbItems();

        //    if (page >= nbItemTotal/nbItem)
        //    {
        //        return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Numero de page est trop grand")); // mettre le message d'error en json
        //    }
        //    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(page, nbItem);

        //    List<DTOChampion> championListDto = new List<DTOChampion>();

        //    championList.ToList().ForEach(champion => championListDto.Add(champion.ToDto()));
        //    return StatusCode((int)HttpStatusCode.OK, championListDto);
        //    //return  Ok( championListDto);
        //}
        [HttpGet("page/{page}/items/{nbItem}")]
        public async Task<IActionResult> GetPage(int page, int nbItem)
        {
            if (page < 0 || nbItem < 0)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Numero de page ou nombre d'item est negatif"));//changer le 416
            }

            int nbItemTotal = await _dataManager.ChampionsMgr.GetNbItems();

            if (page >= nbItemTotal / nbItem)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Numero de page est trop grand")); // mettre le message d'error en json
            }

            try
            {
                IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(page, nbItem);
                List<DTOChampion> championListDto = (List<DTOChampion>)championList.Select(champion => champion.ToDto());
                return Ok(championListDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la récupération des données." });
            }
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Le nom du champion ne peut pas être vide."));
                }
                Champion champion = await _dataManager.ChampionsMgr.GetItemByName(name);
                if (champion == null)
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate("Le champion n'est pas existant"));
                return StatusCode((int)HttpStatusCode.OK, champion.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la récupération des données." });
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOChampion champion)
        {
            try
            {
                //vérifier que le model soit valide
                if (!ModelState.IsValid)
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données du champion ne sont pas correctes"));

                if (string.IsNullOrWhiteSpace(champion.Name) || string.IsNullOrWhiteSpace(champion.Image) || string.IsNullOrWhiteSpace(champion.Bio) || string.IsNullOrWhiteSpace(champion.Class) || string.IsNullOrWhiteSpace(champion.Icon))
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données du champion sont incomplètes"));


                int nbItemTotal = await _dataManager.ChampionsMgr.GetNbItems();
                IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(0, nbItemTotal);

                if (championList.Any(championExist => championExist.Name == champion.Name || championExist.Bio == champion.Bio))
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Le champion existe déjà"));


                var championResult = _dataManager.ChampionsMgr.AddItem(champion.ToChampion());
                /*return CreatedAtAction((GetByName),new {id = 1 },championResult) */

                return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate("Le champion a été créé"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la récupération des données." });
            }

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name,[FromBody] DTOChampion champion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données du champion sont incomplètes"));

                if (string.IsNullOrWhiteSpace(champion.Name) || string.IsNullOrWhiteSpace(champion.Image) || string.IsNullOrWhiteSpace(champion.Bio) || string.IsNullOrWhiteSpace(champion.Class) || string.IsNullOrWhiteSpace(champion.Icon))
                {
                    var message = $"Les paramètres du champion ne sont pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemByName = await _dataManager.ChampionsMgr.GetNbItemsByName(champion.Name);
                if (nbItemByName == 0)
                {
                    var message = $"Le champion {name} n'existe pas.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }
                Champion championUpdate = await _dataManager.ChampionsMgr.GetItemByName(name);
                await _dataManager.ChampionsMgr.UpdateItem(championUpdate, champion.ToChampion());
                var successMessage = $"Le champion {name} a été modifié avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Erreur de base de donnée lors de la modification du champion {name}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite en base de données." });
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                Champion championDelete = await _dataManager.ChampionsMgr.GetItemByName(name);
                if (championDelete == null) { 
                    var message = $"Le champion {name} n'est pas existant";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                var result = _dataManager.ChampionsMgr.DeleteItem(championDelete);
                var successMessage = $"Le champion {name} a été supprimé avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur de base de donnée lors de la suppression du champion {name}",ex);
                return StatusCode((int) HttpStatusCode.InternalServerError, new { message = "Une erreur s'est produite lors de la récupération des données." });
            }
        }
    }
}
