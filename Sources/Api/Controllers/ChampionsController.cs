using MapperApi.Mapper;
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
        //, [FromQuery(Name = "skill")] string skill = null
        // [FromQuery(Name = "runePage")] string runePage = null,

        [HttpGet]
        public async Task<IActionResult> GetChampions([FromQuery(Name = "startIndex")] int? startIndex = 0, [FromQuery(Name = "count")] int? count = 4, [FromQuery(Name = "name")] string? name = null, [FromQuery(Name = "characteristic")] string? characteristic = null, [FromQuery(Name = "championClass")] string? championClass = null, [FromQuery(Name = "descending")] bool descending = false, [FromQuery(Name = "skillName")] string? skillName = null)
        {
            try
            {
                if (Request.Query.Count > 5)
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
                if (!string.IsNullOrEmpty(characteristic))
                {
                    var totalItemCount = await _dataManager.ChampionsMgr.GetNbItemsByCharacteristic(characteristic);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItemsByCharacteristic(characteristic, actualStartIndex, actualCount, null, descending);
                    if (!string.IsNullOrEmpty(characteristic))
                    {
                        championList = championList.Where(r => r.Name.Contains(characteristic));
                    }

                    if (!championList.Any())
                    {
                        var message = $"Aucune charactéristique nommé {characteristic} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les charactéristiques avec le nom {characteristic} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                }
                if (!string.IsNullOrEmpty(skillName))
                {
                    var totalItemCount = await _dataManager.ChampionsMgr.GetNbItemsBySkill(skillName);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItemsBySkill(skillName, actualStartIndex, actualCount, null, descending);
                    if (!string.IsNullOrEmpty(skillName))
                    {
                        championList = championList.Where(r => r.Name.Contains(skillName));
                    }

                    if (!championList.Any())
                    {
                        var message = $"Aucun skill nommé {skillName} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les skills avec le nom {skillName} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                }
                //else if (!string.IsNullOrEmpty(skill))
                //{
                //    var skillObject = await _dataManager.ChampionsMgr.GetItemsBySkill(skill, 1, 1);
                //    var totalItemCount = await _dataManager.ChampionsMgr.GetNbItemsBySkill(skill);
                //    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                //    int actualCount = count.HasValue ? count.Value : totalItemCount;

                //    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItemsBySkill(skillObject, actualStartIndex, actualCount, null, descending);
                //    //if (!string.IsNullOrEmpty(skillName))
                //    //{
                //    //    championListSkillName = championListSkillName.Where(r => r.Name.Contains(skillName));
                //    //}

                //    if (!championList.Any())
                //    {
                //        var message = $"Aucun skill nommé {skillObject.Name} n'a été trouvé.";
                //        _logger.LogInformation(message);
                //        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                //    }

                //    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                //    int currentPage = actualStartIndex / actualCount + 1;
                //    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                //    var successMessage = $"Les skills avec le nom {skillObject.Name} ont été récupéré avec succès.";
                //    _logger.LogInformation(successMessage);
                //    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                //}
                else if (!string.IsNullOrEmpty(name))
                {
                    var totalItemCount = await _dataManager.ChampionsMgr.GetNbItemsByName(name);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItemsByName(name, actualStartIndex, actualCount, null, descending);
                    if (!string.IsNullOrEmpty(name))
                    {
                        championList = championList.Where(r => r.Name.Contains(name));
                    }

                    if (!championList.Any())
                    {
                        var message = $"Aucun champion nommé {name} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les champions avec le nom {name} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                }
                else if (!string.IsNullOrEmpty(championClass))
                {
                    ChampionClass champClass = Enum.TryParse<ChampionClass>(championClass, true, out champClass) ? champClass : ChampionClass.Unknown;
                    var totalItemCount = await _dataManager.ChampionsMgr.GetNbItemsByClass(champClass);
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItemsByClass(champClass, actualStartIndex, actualCount, null, descending);


                    if (!championList.Any())
                    {
                        var message = $"Aucun champion avec la classe {championClass} n'a été trouvé.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"Les champions avec la classe {championClass} ont été récupéré avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                }
                //else if (runePage != null)
                //{
                //    var totalItemCount = await _dataManager.ChampionsMgr.GetNbItemsByRunePage(runePage);
                //    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                //    int actualCount = count.HasValue ? count.Value : totalItemCount;

                //    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItemsByRunePage(runePage, actualStartIndex, actualCount, null, descending);
                //    //if (!string.IsNullOrEmpty(runePage))
                //    //{
                //    //    championList = championList.Where(r => r.Name.Contains(runePage));
                //    //}

                //    if (!championList.Any())
                //    {
                //        var message = $"Aucun champion avec la page rune {runePage} n'a été trouvé.";
                //        _logger.LogInformation(message);
                //        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                //    }

                //    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                //    int currentPage = actualStartIndex / actualCount + 1;
                //    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                //    var successMessage = $"Les champions avec la page rune {runePage} ont été récupéré avec succès.";
                //    _logger.LogInformation(successMessage);
                //    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                //}
                else
                {
                    int totalItemCount = await _dataManager.ChampionsMgr.GetNbItems();
                    int actualStartIndex = startIndex.HasValue ? startIndex.Value : 0;
                    int actualCount = count.HasValue ? count.Value : totalItemCount;

                    IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(actualStartIndex, actualCount, name, descending);
                    if (!string.IsNullOrEmpty(name))
                    {
                        championList = championList.Where(r => r.Name.Contains(name));
                    }

                    if (!championList.Any())
                    {
                        var message = $"Aucun champion avec le nom {name} n'est pas existant.";
                        _logger.LogInformation(message);
                        return StatusCode((int)HttpStatusCode.NoContent, FactoryMessage.MessageCreate(message));
                    }

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / actualCount);
                    int currentPage = actualStartIndex / actualCount + 1;
                    int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

                    var successMessage = $"La récupération des données a été réalisé avec succès.";
                    _logger.LogInformation(successMessage);
                    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<IEnumerable<DTOChampion>>(successMessage, currentPage, nextPage, totalPages, totalItemCount, championList.Select(e => e.ToDto())));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Une erreur est survenue lors de la récupération des champions : {ex.Message}";
                _logger.LogError(errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    var message = $"Le nom du champion ne peut pas être vide.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }
                Champion champion = await _dataManager.ChampionsMgr.GetItemByName(name);
                if (champion == null)
                {
                    var message = $"Le champion {name} n'est pas existant.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate(message));
                }

                var successMessage = $"Le champion {name} a été récupéré avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<DTOChampion>(successMessage, champion.ToDto()));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération du champion {name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOChampion champion)
        {
            try
            {
                //vérifier que le model soit valide
                if (!ModelState.IsValid)
                {
                    var message = $"Les données du champion ne sont pas correctes";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(champion.Name) || string.IsNullOrWhiteSpace(champion.Image) || string.IsNullOrWhiteSpace(champion.Bio) || string.IsNullOrWhiteSpace(champion.Class) || string.IsNullOrWhiteSpace(champion.Icon))
                {
                    var message = $"Le champion {champion.Name} a des données incomplétés.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }


                int nbItemTotal = await _dataManager.ChampionsMgr.GetNbItems();
                IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(0, nbItemTotal);

                if (championList.Any(championExist => championExist.Name == champion.Name || championExist.Bio == champion.Bio))
                {
                    var message = $"Le champion {champion.Name} a été ajouté avec succès.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                var championResult = _dataManager.ChampionsMgr.AddItem(champion.ToChampion());

                var successMessage = $"Le champion {champion.Name} a été ajouté avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate(successMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de l'ajout du champion {champion.Name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }

        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, [FromBody] DTOChampion champion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = $"Le champion passé en paramètre n'est pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                if (string.IsNullOrWhiteSpace(champion.Name) || string.IsNullOrWhiteSpace(champion.Image) || string.IsNullOrWhiteSpace(champion.Bio) || string.IsNullOrWhiteSpace(champion.Class) || string.IsNullOrWhiteSpace(champion.Icon))
                {
                    var message = $"Les paramètres du champion ne sont pas correct";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                int nbItemByName = await _dataManager.ChampionsMgr.GetNbItemsByName(name);
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
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur s'est produite en base de données."));
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                Champion championDelete = await _dataManager.ChampionsMgr.GetItemByName(name);
                if (championDelete == null)
                {
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
                _logger.LogError($"Erreur de base de donnée lors de la suppression du champion {name}", ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur s'est produite lors de la récupération des données."));
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetItemCount(
        [FromQuery(Name = "characteristic")] string? charName = null,
        [FromQuery(Name = "class")] string? championClass = null,
        [FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "runePage")] string? runePage = null,
        [FromQuery(Name = "skill")] string? skill = null,
        [FromQuery(Name = "skillName")] string? skillName = null)
        {
            int count = 0;

            if (charName != null)
            {
                count = await _dataManager.ChampionsMgr.GetNbItemsByCharacteristic(charName);
            }
            else if (championClass != null)
            {
                ChampionClass championClassEnum = Enum.TryParse<ChampionClass>(championClass, true, out championClassEnum) ? championClassEnum : ChampionClass.Unknown;
                count = await _dataManager.ChampionsMgr.GetNbItemsByClass(championClassEnum);
            }
            else if (name != null)
            {
                count = await _dataManager.ChampionsMgr.GetNbItemsByName(name);
            }
            /*else if (runePage != null)
            {
                count = await _dataManager.ChampionsMgr.GetNbItemsByRunePage(await _dataManager.ChampionsMgr.GetItemsByRunePage(runePage));
            }*/
            /*else if (skill != null)
            {
                count = await _dataManager.ChampionsMgr.GetNbItemsBySkill();
            }*/
            else if (skillName != null)
            {
                count = await _dataManager.ChampionsMgr.GetNbItemsBySkill(skillName);
            }
            else
            {
                count = await _dataManager.ChampionsMgr.GetNbItems();
            }

            return Ok(count);
        }
    }
}
