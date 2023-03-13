using System;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModels
{
	[ObservableObject]
	public partial class EditableSkinVM
	{
		public EditableSkinVM(ChampionVM championVM)
		{
            champion = championVM.Model;
        }

		public EditableSkinVM(SkinVM skinVM)
		{
			Name = skinVM.Name;
            IconBase64 = skinVM.Icon;
            LargeImageBase64 = skinVM.Image;
            Description = skinVM.Description;
            Price = skinVM.Price;
            champion = skinVM.Champion;
		}

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string iconBase64;

        [ObservableProperty]
        private string largeImageBase64;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private float price;

        private Champion champion;

        public SkinVM ToSkinVM()
		{
			var skin = new Skin(name, champion, price, iconBase64, largeImageBase64, description);
			return new SkinVM(skin);
		}
	}
}

