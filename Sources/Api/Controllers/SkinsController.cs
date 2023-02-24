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
        public SkinsController(IDataManager dataManger)
        {
            _dataManager = dataManger;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Skin> skinsList = await _dataManager.SkinsMgr.GetItems(0, await _dataManager.SkinsMgr.GetNbItems());
                if (skinsList.Count() == 0) return StatusCode((int)HttpStatusCode.NoContent);
                return StatusCode((int)HttpStatusCode.OK, skinsList.Select(e => e.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des skins"));
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
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Le nom du skin ne peut pas être vide."));
                }
                Skin skin = await _dataManager.SkinsMgr.GetItemByName(name);
                if (skin == null)
                    return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate("Le skin n'est pas existant"));
                return StatusCode((int)HttpStatusCode.OK, skin.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des skins"));
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOSkinPost skin)
        {
            if (string.IsNullOrWhiteSpace(skin.Name) || string.IsNullOrWhiteSpace(skin.Image) || string.IsNullOrWhiteSpace(skin.Description) || float.IsNegative(skin.Price) || string.IsNullOrWhiteSpace(skin.Icon))
                return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données du skin sont incomplètes"));

            int nbItemTotal = await _dataManager.SkinsMgr.GetNbItems();
            IEnumerable<Skin> skinList = await _dataManager.SkinsMgr.GetItems(0, nbItemTotal);

            if (skinList.Any(skinExist => skinExist.Name == skin.Name))
                return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Le skin existe déjà"));

            Champion champion = await _dataManager.ChampionsMgr.GetItemByName(skin.NameChampion);
            if (champion == null)
                return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate("Le champion n'est pas existant"));


            var skinResult = _dataManager.SkinsMgr.AddItem(skin.ToSkin(champion));
            /*return CreatedAtAction((GetByName),new {id = 1 },championResult) */

            return StatusCode((int)HttpStatusCode.Created, FactoryMessage.MessageCreate("Le champion a été créé"));
        }

        //// PUT api/<ValuesController>/5
        //[HttpPut("{name}")]
        //public async Task<IActionResult> Put(string name, [FromBody] DTOSkin skin)
        //{
        //    //fait automatiquement pour les champs required ...
        //    //if (!ModelState.IsValid)
        //    //    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données du skin sont incomplètes"));

        //    if (string.IsNullOrWhiteSpace(skin.Name) || string.IsNullOrWhiteSpace(skin.Image) || string.IsNullOrWhiteSpace(skin.Description) || float.IsNegative(skin.Price) || string.IsNullOrWhiteSpace(skin.Icon))
        //        return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Les données du skin sont incomplètes"));

        //    int nbItemByName = await _dataManager.SkinsMgr.GetNbItemsByName(skin.Name);
        //    if (nbItemByName == 0)
        //        return StatusCode((int)HttpStatusCode.NotFound, FactoryMessage.MessageCreate("Le skin n'existe pas."));

        //    Skin skinUpdate = await _dataManager.SkinsMgr.GetItemByName(name);
        //    await _dataManager.SkinsMgr.UpdateItem(skinUpdate, skin.ToSkin());
        //    return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate("Le skin a été modifié."));
        //    //no content
        //}

        // DELETE api/<ValuesController>/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                Skin skinDelete = await _dataManager.SkinsMgr.GetItemByName(name);
                if (skinDelete == null)
                    return StatusCode((int)HttpStatusCode.BadRequest, FactoryMessage.MessageCreate("Le skin n'est pas existant"));
                _dataManager.SkinsMgr.DeleteItem(skinDelete);
                return StatusCode((int)HttpStatusCode.OK, FactoryMessage.MessageCreate("Le skin a été supprimé"));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, FactoryMessage.MessageCreate("Une erreur est survenue lors de la récupération des skins"));
            }
        }
    }
}
