using Api.Mapper;
using DTOLol;
using DTOLol.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Net;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkinsController : ControllerBase
    {
        private readonly IDataManager _dataManager;
        private readonly ILogger<SkinsController> _logger;
        public SkinsController(IDataManager dataManger, ILogger<SkinsController> logger)
        {
            _dataManager = dataManger;
            _logger = logger;
        }

        // GET: api/<ValuesController>
        [HttpGet]

        [HttpGet]
        public async Task<IActionResult> GetRunes([FromQuery(Name = "startIndex")] int? startIndex = 0, [FromQuery(Name = "count")] int? count = 4, [FromQuery(Name = "descending")] bool descending = false, [FromQuery(Name = "NameSubstring")] string? nameSubstring = null, [FromQuery(Name = "Champion")] Champion champion = null)
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
                    var totalItemCount = await _dataManager.SkinsMgr.GetNbItemsByName(nameSubstring);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Skin> skinList = await _dataManager.SkinsMgr.GetItemsByName(nameSubstring, actualStartIndex, actualCount, null, descending);
                    if (!string.IsNullOrEmpty(nameSubstring))
                    {
                        skinList = skinList.Where(r => r.Name.Contains(nameSubstring));
                    }

                    if (!skinList.Any())
                    {
                        var message = $"Aucun skin nommé {nameSubstring} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les skins avec le nom {nameSubstring} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOSkin>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, skinList.Select(e => e.ToDto())));
                }
                //&& runeFamily.IsValid()
                else if (champion != null)
                {
                    var totalItemCount = await _dataManager.SkinsMgr.GetNbItemsByChampion(champion);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Skin> skinList = await _dataManager.SkinsMgr.GetItemsByChampion(champion, actualStartIndex, actualCount, null, descending);
                    //if (!string.IsNullOrEmpty(skillName))
                    //{
                    //    championListSkillName = championListSkillName.Where(r => r.Name.Contains(skillName));
                    //}

                    if (!skinList.Any())
                    {
                        var message = $"Aucune champion {champion.Name} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les skins {champion.Name} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOSkin>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, skinList.Select(e => e.ToDto())));
                }
                else
                {
                    int totalItemCount = await _dataManager.SkinsMgr.GetNbItems();
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Skin> skinList = await _dataManager.SkinsMgr.GetItems(actualStartIndex, actualCount, null, descending);

                    if (!skinList.Any())
                    {
                        var message = $"Aucun skins en base de données.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"La récupération des données a été réalisé avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOSkin>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, skinList.Select(e => e.ToDto())));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Une erreur est survenue lors de la récupération des champions : {ex.Message}";
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
                    var message = $"Le nom du skin ne peut pas être vide.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                Skin skin = await _dataManager.SkinsMgr.GetItemByName(name);
                if (skin == null)
                {
                    var message = $"Le skin {name} n'est pas existant.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }
                var successMessage = $"Le skin {name} a été récupé avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, skin.ToDto());
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération du skin {name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOSkin skin)
        {
            try { 
                if (!ModelState.IsValid)
                {
                    var message = $"Les données du skin ne sont pas correctes";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(skin.Name) || string.IsNullOrWhiteSpace(skin.Image) || string.IsNullOrWhiteSpace(skin.Description) || float.IsNegative(skin.Price) || string.IsNullOrWhiteSpace(skin.Icon))
                {
                    var message = $"Le skin {skin.Name} a des données incomplétés.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemTotal = await _dataManager.SkinsMgr.GetNbItems();
                IEnumerable<Skin> skinList = await _dataManager.SkinsMgr.GetItems(0, nbItemTotal);

                if (skinList.Any(skinExist => skinExist.Name == skin.Name))
                {
                    var message = $"Le skin {skin.Name} a été existe déjà.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                Champion champion = await _dataManager.ChampionsMgr.GetItemByName(skin.ChampionName);
                var skinResult = _dataManager.SkinsMgr.AddItem(skin.ToSkin(champion));
                /*return CreatedAtAction((GetByName),new {id = 1 },championResult) */

                var successMessage = $"Le skin {skin.Name} a été ajouté avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de l'ajout du skin {skin.Name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] DTOSkin skin)
        {
            try {
                //fait automatiquement pour les champs required ...
                if (!ModelState.IsValid)
                {
                    var message = $"Le skin passé en paramètre n'est pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(skin.Name) || string.IsNullOrWhiteSpace(skin.Image) || string.IsNullOrWhiteSpace(skin.Description) || float.IsNegative(skin.Price) || string.IsNullOrWhiteSpace(skin.Icon))
                {
                    var message = $"Les paramètres du skin ne sont pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemByName = await _dataManager.SkinsMgr.GetNbItemsByName(skin.Name);
                if (nbItemByName == 0)
                {
                    var message = $"Le skin {name} n'existe pas.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                Skin skinUpdate = await _dataManager.SkinsMgr.GetItemByName(name);
                Champion champion = await _dataManager.ChampionsMgr.GetItemByName(skin.ChampionName);

                await _dataManager.SkinsMgr.UpdateItem(skinUpdate, skin.ToSkin(champion));
                var successMessage = $"Le skin {name} a été modifié avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de modification du skin {skin.Name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                Skin skinDelete = await _dataManager.SkinsMgr.GetItemByName(name);
                if (skinDelete == null)
                {
                    var message = $"Le skin {name} n'est pas existant";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                _dataManager.SkinsMgr.DeleteItem(skinDelete);
                var successMessage = $"Le skin {name} a été supprimé avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var error_message = $"Erreur de base de donnée lors de la suppression du skin {name}";
                _logger.LogError(error_message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(error_message));
            }
        }
    }
}
