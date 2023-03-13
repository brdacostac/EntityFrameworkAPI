using System;
using System.Globalization;
using Model;

namespace LolApp.Resources.Converters
{
    public class ChampionClassToIconConverter : IValueConverter
    {
        private static Dictionary<ChampionClass, string> icons = new()
        {
            [ChampionClass.Assassin] = "assassin.png",
            [ChampionClass.Fighter] = "fighter.png",
            [ChampionClass.Mage] = "mage.png",
            [ChampionClass.Marksman] = "marksman.png",
            [ChampionClass.Support] = "support.png",
            [ChampionClass.Tank] = "tank.png"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ChampionClass champClass = (ChampionClass)value;
                if(!icons.TryGetValue(champClass, out string icon))
                {
                    return "";
                }
                return ImageSource.FromFile($"{icon}");
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

