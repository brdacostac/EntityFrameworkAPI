using MapperApi.Mapper;
using DTOLol;
using DTOLol.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Net;

namespace Api.Controllers
{
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RunePagesController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        private readonly ILogger<RunePagesController> _logger;
        public RunePagesController(IDataManager dataManger, ILogger<RunePagesController> logger)
        {
            _dataManager = dataManger;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetRunePages([FromQuery(Name = "startIndex")] int? startIndex = 0, [FromQuery(Name = "count")] int? count = 4, [FromQuery(Name = "descending")] bool descending = false, [FromQuery(Name = "NameSubstring")] string? nameSubstring = null, [FromQuery(Name = "Champion")] string? nameChampion = null, [FromQuery(Name = "Rune")] string? nameRune = null)
        {
            try
            {
                if (count <= 0 || count > 25)
                {
                    var message = "startIndex doit être compris entre 1 et 25.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (Request.Query.Count > 4)
                {
                    var errorMessage = $"La requête doit contenir uniquement l'un des paramètres suivants : startIndex, count, name, skillName, charName, skill, index, orderingPropertyName.";
                    _logger.LogWarning(errorMessage);
                    return BadRequest(FactoryMessage.MessageCreate(errorMessage));
                }



                if (!string.IsNullOrEmpty(nameSubstring))
                {
                    var totalItemCount = await _dataManager.RunePagesMgr.GetNbItemsByName(nameSubstring);
                    int actualCount = count != null ? count.Value : totalItemCount;

                    IEnumerable<RunePage> runePageList = await _dataManager.RunePagesMgr.GetItemsByName(nameSubstring, (int)startIndex, actualCount, null, descending);

                    if (!runePageList.Any())
                    {
                        var message = $"Aucune page de runes nommé {nameSubstring} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = (int)(startIndex / actualCount + 1);
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les pages de runes avec le nom {nameSubstring} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORunePage>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runePageList.Select(e => e.ToDto())));
                }
                else if (!string.IsNullOrEmpty(nameChampion))
                {
                    Champion champObj = await _dataManager.ChampionsMgr.GetItemByName(nameChampion);
                    var totalItemCount = await _dataManager.RunePagesMgr.GetNbItemsByChampion(champObj);
                    int actualCount = count != null ? count.Value : totalItemCount;

                    IEnumerable<RunePage> runePageList = await _dataManager.RunePagesMgr.GetItemsByChampion(champObj, (int)startIndex, actualCount, null, descending);
                    if (!runePageList.Any())
                    {
                        var message = $"Aucune page de rune correspond au champion {nameChampion} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = (int)(startIndex / actualCount + 1);
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les pages runes correspondantes au champion {nameChampion} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORunePage>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runePageList.Select(e => e.ToDto())));
                }
                else if (!string.IsNullOrEmpty(nameRune))
                {
                    var rune = await _dataManager.RunesMgr.GetItemByName(nameRune);
                    var totalItemCount = await _dataManager.RunePagesMgr.GetNbItemsByRune(rune);
                    int actualCount = count != null ? count.Value : totalItemCount;

                    IEnumerable<RunePage> runePageList = await _dataManager.RunePagesMgr.GetItemsByRune(rune, (int)startIndex, actualCount, null, descending);
                    if (!runePageList.Any())
                    {
                        var message = $"Aucune page de rune correspond a la rune {nameRune} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = (int)(startIndex / actualCount + 1);
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les pages runes correspondantes à la rune {rune.Name} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORunePage>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runePageList.Select(e => e.ToDto())));
                }
                else
                {
                    int totalItemCount = await _dataManager.RunePagesMgr.GetNbItems();
                    int actualCount = count != null ? count.Value : totalItemCount;

                    IEnumerable<RunePage> runeList = await _dataManager.RunePagesMgr.GetItems((int)startIndex, actualCount, null, descending);

                    if (!runeList.Any())
                    {
                        var message = $"Aucune runes en base de données.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = (int)(startIndex / actualCount + 1);
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"La récupération des données a été réalisé avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORunePage>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runeList.Select(e => e.ToDto())));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Une erreur est survenue lors de la récupération des pages de runes : {ex.Message}.";
                _logger.LogError(errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }


        // GET api/<ValuesController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    var message = $"Le nom de la page de rune ne peut pas être vide.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                RunePage runepage = await _dataManager.RunePagesMgr.GetItemByName(name);
                if (runepage == null)
                {
                    var message = $"Le nom de la page de rune {name} n'est pas existant.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                var successMessage = $"La page de rune {name} a été modifié ajoutée avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, runepage.ToDto());
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération de la page de rune {name}.";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        [HttpGet, MapToApiVersion("3.0")]
        public async Task<IActionResult> GetV3(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    var message = $"Le nom de la page de rune ne peut pas être vide. V3 ";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                RunePage runepage = await _dataManager.RunePagesMgr.GetItemByName(name);
                if (runepage == null)
                {
                    var message = $"Le nom de la page de rune {name} n'est pas existant. V3";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                var successMessage = $"La page de rune {name} a été modifié ajoutée avec succès. V3";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, runepage.ToDto());
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération de la page de rune {name}. V3";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }



        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTORunePage runepage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = $"Les données de la page de rune ne sont pas correctes.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(runepage.Name))
                {
                    var message = $"La page de rune {runepage.Name} a des données incomplétés.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemTotal = await _dataManager.RunePagesMgr.GetNbItems();
                IEnumerable<RunePage> runepageList = await _dataManager.RunePagesMgr.GetItems(0, nbItemTotal);

                if (runepageList.Any(runeExist => runeExist.Name == runepage.Name))
                {
                    var message = $"La page de rune {runepage.Name} existe déjà.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                var runeResult = _dataManager.RunePagesMgr.AddItem(runepage.ToRunePage());

                var successMessage = $"La page de rune {runepage.Name} a été modifiée ajouté avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de l'ajout de la page de rune {runepage.Name}.";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] DTORunePage runePage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = $"Les paramètres de la page de runes ne sont pas corrects.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(runePage.Name))
                {
                    var message = $"Le nom de la page de runes ne sont pas corrects";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemByName = await _dataManager.RunePagesMgr.GetNbItemsByName(runePage.Name);
                if (nbItemByName == 0)
                {
                    var message = $"La page de runes {name} n'existe pas.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                RunePage runeUpdate = await _dataManager.RunePagesMgr.GetItemByName(name);
                await _dataManager.RunePagesMgr.UpdateItem(runeUpdate, runePage.ToRunePage());
                var successMessage = $"La page de rune {name} a été modifié avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var message = $"Erreur de base de donnée lors de la modification de la page de rune {name}.";
                _logger.LogError(message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(message));
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                RunePage runepageDelete = await _dataManager.RunePagesMgr.GetItemByName(name);
                if (runepageDelete == null)
                {
                    var message = $"La page de rune {name} n'est pas existante.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                var successMessage = $"La page de rune {name} a été supprimée avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var error_message = $"Erreur de base de donnée lors de la suppression de la page de rune {name}.";
                _logger.LogError(error_message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(error_message));
            }
        }

    }

}

