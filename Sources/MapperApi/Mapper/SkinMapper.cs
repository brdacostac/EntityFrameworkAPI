﻿using DTOLol;
using Model;

namespace MapperApi.Mapper
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

        public static Skin ToSkin(this DTOSkin dTOSkin,Champion champion)
        {
            
            return new Skin(dTOSkin.Name, champion, dTOSkin.Price, dTOSkin.Icon, dTOSkin.Image, dTOSkin.Description);
        }

        public static Skin ToSkin(this DTOSkin dTOSkin)
        {

            return new Skin(dTOSkin.Name, null, dTOSkin.Price, dTOSkin.Icon, dTOSkin.Image, dTOSkin.Description);
        }

    }
}
