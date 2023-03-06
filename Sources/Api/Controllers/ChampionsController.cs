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

        //[HttpGet]
        //public async Task<IActionResult> GetAll([FromQuery(Name = "startIndex")] int index = 0, [FromQuery(Name = "pageSize")] int count = 4, [FromQuery(Name = "orderingPropertyName")] string? orderingPropertyName = null, [FromQuery(Name = "descending")] bool? descending = false)
        //{
        //    try
        //    {
        //        // Vérifier que la pageSize est valide
        //        if (pageSize <= 0 || pageSize > 25)
        //        {
        //            return BadRequest("pageSize doit être compris entre 1 et 25.");
        //        }

        //        // Récupérer le nombre total d'éléments
        //        int totalItemCount = await _dataManager.ChampionsMgr.GetNbItems();

        //        // Calculer les paramètres de pagination
        //        int totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);
        //        int currentPage = (startIndex / pageSize) + 1;

        //        // Vérifier que la page demandée est valide
        //        if (currentPage < 1 || currentPage > totalPages)
        //        {
        //            return BadRequest("La page demandée n'est pas valide.");
        //        }

        //        // Récupérer les éléments correspondant à la pagination et au filtrage
        //        IEnumerable<Champion> championList = await _dataManager.ChampionsMgr.GetItems(startIndex, pageSize);
        //        if (!string.IsNullOrEmpty(name))
        //        {
        //            championList = championList.Where(r => r.Name.Contains(name));
        //        }

        //        // Vérifier que des éléments ont été trouvés
        //        if (!championList.Any())
        //        {
        //            return NoContent();
        //        }

        //        // Calculer les paramètres de pagination pour la page suivante
        //        int nextPage = (currentPage < totalPages) ? currentPage + 1 : -1;

        //        // Retourner les résultats paginés et filtrés
        //        var result = new
        //        {
        //            currentPage = currentPage,
        //            nextPage = nextPage,
        //            totalPages = totalPages,
        //            totalCount = totalItemCount,
        //            items = championList.Select(e => e.ToDto())
        //        };

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des runes"));
        //    }
        //}

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

                var successMessage = $"Le champion {name} a été modifié ajouté avec succès.";
                _logger.LogInformation(successMessage);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate<DTOChampion>(successMessage, champion.ToDto()));
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération du champion {name}";
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = errorMessage });
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
                    var message = $"Le champion {champion.Name} a été modifié ajouté avec succès.";
                    _logger.LogInformation(message);
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate(message));
                }

                var championResult = _dataManager.ChampionsMgr.AddItem(champion.ToChampion());

                var successMessage = $"Le champion {champion.Name} a été modifié ajouté avec succès.";
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

        // PUT api/<ValuesController>/5
        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name,[FromBody] DTOChampion champion)
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
                return StatusCode((int) HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur s'est produite lors de la récupération des données."));
            }
        }

        //[HttpGet("{name}/skills")]
        //public async Task<IActionResult> GetSkills(string name)
        //{
        //    Champion champion = await _dataManager.ChampionsMgr.GetItemByName(name);
        //    if (champion == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(champion.Skills);
        //}

        [HttpGet("skills/{skillName}")]
        public async Task<ActionResult<IEnumerable<DTOChampion>>> GetChampionsByNameSkill(string skillName, [FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] string orderingPropertyName = null, [FromQuery] bool descending = false)
        {
            try { 
                IEnumerable<Champion?> champions = await _dataManager.ChampionsMgr.GetItemsBySkill(skillName, index, count, orderingPropertyName, descending);
                if (champions == null || champions.Count() == 0)
                {
                    return NotFound($"Skill '{skillName}' not found.");
                }
                IEnumerable<DTOChampion> championDTOs = champions.Select(champion => champion.ToDto());

                return Ok(championDTOs);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Erreur de base de donnée lors de la récupération du skill {skillName}"; 
                _logger.LogError(errorMessage, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate(errorMessage));
            }
        }

        [HttpGet("skills/{skill}")]
        public async Task<ActionResult<IEnumerable<DTOChampion>>> GetChampionsByNameSkill(DTOSkill skill, [FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] string orderingPropertyName = null, [FromQuery] bool descending = false)
        {
            IEnumerable<Champion?> champions = await _dataManager.ChampionsMgr.GetItemsBySkill(skill.ToSkill(), index, count, orderingPropertyName, descending);
            if (champions == null || champions.Count() == 0)
            {
                return NotFound($"Skill '{skill}' not found.");
            }
            IEnumerable<DTOChampion> championDTOs = champions.Select(champion => champion.ToDto());

            return Ok(championDTOs);
        }

        //[HttpPost("{name}/skills")]
        //public async Task<IActionResult> AddSkill(string name, [FromBody] Skill skill)
        //{
        //    Champion champion = await _dataManager.ChampionsMgr.GetItemByName(name);
        //    if (champion == null)
        //    {
        //        return NotFound();
        //    }

        //    bool added = champion.AddSkill(skill);
        //    if (!added)
        //    {
        //        return BadRequest("The skill already exists for this champion");
        //    }

        //    await _dataManager.ChampionsMgr.UpdateItem(champion);

        //    return CreatedAtAction(nameof(GetSkills), new { name = name }, skill);
        //}

        //[HttpDelete("{name}/skills/{skillName}")]
        //public ActionResult RemoveSkill(string name, string skillName),n
        //{
        //    Champion champion = await _dataManager.ChampionsMgr.GetItemByName(name);
        //    if (champion == null)
        //    {
        //        return NotFound();
        //    }

        //    Skill skillToRemove = champion.Skills.FirstOrDefault(s => s.Name == skillName);
        //    if (skillToRemove == null)
        //    {
        //        return NotFound();
        //    }

        //    bool removed = champion.RemoveSkill(skillToRemove);
        //    if (!removed)
        //    {
        //        return BadRequest("Failed to remove the skill");
        //    }

        //    _repository.Update(champion);

        //    return NoContent();
        //}

        [HttpGet("characteristic/{charName}")]
        public async Task<IActionResult> GetItemsByCharacteristic(string charName, [FromQuery] int index, [FromQuery] int count, [FromQuery] string? orderingPropertyName = null, [FromQuery] bool descending = false)
        {
            try
            {
                var champions = await _dataManager.ChampionsMgr.GetItemsByCharacteristic(charName, index, count, orderingPropertyName, descending);
                var dtos = champions.Select(c => c.ToDto());
                return Ok(dtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the error here.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("class/{championClass}")]
        public async Task<IActionResult> GetItemsByClass(ChampionClass championClass, [FromQuery] int index, [FromQuery] int count, [FromQuery] string? orderingPropertyName = null, [FromQuery] bool descending = false)
        {
            try
            {
                var champions = await _dataManager.ChampionsMgr.GetItemsByClass(championClass, index, count, orderingPropertyName, descending);
                var dtos = champions.Select(c => c.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                // Log the error here.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet("name/{substring}")]
        public async Task<ActionResult<IEnumerable<DTOChampion>>> GetItemsByName(string substring, int index = 0, int count = 20, string orderingPropertyName = null, bool descending = false)
        {
            try
            {
                var champions = await _dataManager.ChampionsMgr.GetItemsByName(substring, index, count, orderingPropertyName, descending);
                var dtos = champions.Select(c => c.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("runePage")]
        public async Task<ActionResult<IEnumerable<DTOChampion>>> GetItemsByRunePage([FromQuery] RunePage? runePage = null, int index = 0, int count = 20, string orderingPropertyName = null, bool descending = false)
        {
            try
            {
                var champions = await _dataManager.ChampionsMgr.GetItemsByRunePage(runePage, index, count, orderingPropertyName, descending);
                var dtos = champions.Select(c => c.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
