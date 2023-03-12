﻿using DTOLol;
using Model;

namespace Api.Mapper
{
    public static class SkinMapper
    {
        public static DTOSkin ToDto(this Skin skin)
        {
            DTOSkin dTOSkin = new DTOSkin()
            {
                Name = skin.Name,
                ChampionName = skin.Champion.Name,
                Description = skin.Description,
                Price = skin.Price, 
                Icon = skin.Icon,
                Image = skin.Image.Base64
            };
            return dTOSkin;
        }

        public static Skin ToSkin(this DTOSkin dTOSkin)
        {
            return null;
            //return new Skin(dTOSkin.Name, dTOSkin.ChampionName.ToDto(), dTOSkin.Price, dTOSkin.Icon, dTOSkin.Image, dTOSkin.Description);
        }

        public static Skin ToSkin(this DTOSkinPost dTOSkin, Champion champion)
        {
            return new Skin(dTOSkin.Name, champion, dTOSkin.Price, dTOSkin.Icon, dTOSkin.Image, dTOSkin.Description);
        }
    }
}
