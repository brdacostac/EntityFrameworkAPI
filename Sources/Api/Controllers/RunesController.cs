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
        private readonly ILogger<RunesController> _logger;
        public RunesController(IDataManager dataManger, ILogger<RunesController> logger)
        {
            _dataManager = dataManger;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRunes([FromQuery(Name = "startIndex")] int? startIndex = 0, [FromQuery(Name = "count")] int? count = 4, [FromQuery(Name = "descending")] bool descending = false,  [FromQuery(Name = "NameSubstring")] string? nameSubstring = null, [FromQuery(Name = "RunneFamily")] RuneFamily? runeFamily = null)
        {
            try
            {
                if (Request.Query.Count > 3)
                {
                    var errorMessage = $"La requête doit contenir uniquement l'un des paramètres suivants : startIndex, count, name, skillName, charName, skill, index, orderingPropertyName.";
                    _logger.LogWarning(errorMessage);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(errorMessage));
                }

                if (count <= 0 || count > 25)
                {
                    var message = "startIndex doit être compris entre 1 et 25.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                if (!string.IsNullOrEmpty(nameSubstring))
                {
                    var totalItemCount = await _dataManager.RunesMgr.GetNbItemsByName(nameSubstring);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItemsByName(nameSubstring, actualStartIndex, actualCount, null, descending);
                    if (!string.IsNullOrEmpty(nameSubstring))
                    {
                        runeList = runeList.Where(r => r.Name.Contains(nameSubstring));
                    }

                    if (!runeList.Any())
                    {
                        var message = $"Aucune rune nommé {nameSubstring} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les runes avec le nom {nameSubstring} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORune>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runeList.Select(e => e.ToDto())));
                }
                //&& runeFamily.IsValid()
                else if (runeFamily != null )
                {
                    var totalItemCount = await _dataManager.RunesMgr.GetNbItemsByFamily(runeFamily.Value);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItemsByFamily(runeFamily.Value, actualStartIndex, actualCount, null, descending);
                    //if (!string.IsNullOrEmpty(skillName))
                    //{
                    //    championListSkillName = championListSkillName.Where(r => r.Name.Contains(skillName));
                    //}

                    if (!runeList.Any())
                    {
                        var message = $"Aucune rune correspond à la famille {runeFamily.Value} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les runes correspondantes à la famille {runeFamily.Value} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORune>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runeList.Select(e => e.ToDto())));
                }
                else
                {
                    int totalItemCount = await _dataManager.ChampionsMgr.GetNbItems();
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Rune> runeList = await _dataManager.RunesMgr.GetItems(actualStartIndex, actualCount, null, descending);

                    if (!runeList.Any())
                    {
                        var message = $"Aucune runes en base de données.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"La récupération des données a été réalisé avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTORune>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, runeList.Select(e => e.ToDto())));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Une erreur est survenue lors de la récupération des runes : {ex.Message}";
                _logger.LogError(errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try { 
                if (string.IsNullOrEmpty(name))
                {
                    var message = $"Le nom de la page de rune ne peut pas être vide.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                Rune rune = await _dataManager.RunesMgr.GetItemByName(name);
                if (rune == null)
                {
                    var message = $"La page de rune {name} n'est pas existante.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                var successMessage = $"La page de rune {name} a été récupé avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, rune.ToDto());
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération de la page de rune {name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTORunePage runepage)
        {
            try {
                if (!ModelState.IsValid)
                {
                    var message = $"Les données de la page de rune ne sont pas correctes";
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
                IEnumerable<RunePage> runePageList = await _dataManager.RunePagesMgr.GetItems(0, nbItemTotal);

                if (runePageList.Any(runeExist => runeExist.Name == runepage.Name))
                {
                    var message = $"La page de rune {runepage.Name} existe déjà.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                var runeResult = _dataManager.RunePagesMgr.AddItem(runepage.ToRunePage());

                var successMessage = $"La page de rune {runepage.Name} a été modifié ajouté avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de l'ajout de la page de rune {runepage.Name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] DTORunePage runepage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = $"Le page de rune passé en paramètre n'est pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(runepage.Name))
                { 
                    var message = $"Le nom de la page de rune n'est pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemByName = await _dataManager.RunePagesMgr.GetNbItemsByName(runepage.Name);
                if (nbItemByName == 0)
                {
                    var message = $"La page de rune {name} n'existe pas.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                RunePage runeUpdate = await _dataManager.RunePagesMgr.GetItemByName(name);
                await _dataManager.RunePagesMgr.UpdateItem(runeUpdate, runepage.ToRunePage());
                var successMessage = $"La page de rune {name} a été modifié avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var message = $"Erreur de base de donnée lors de la modification de la page de rune {name}";
                _logger.LogError(message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(message));
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try { 
                RunePage runeDelete = await _dataManager.RunePagesMgr.GetItemByName(name);
                if (runeDelete == null)
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
                var error_message = $"Erreur de base de donnée lors de la suppression de la page de rune {name}";
                _logger.LogError(error_message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(error_message));
            }
        }

    }
}
