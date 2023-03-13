using System;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace ViewModels
{
	[ObservableObject]
	public partial class SkinVM
	{
		public Skin Model
		{
			get => model;
			set
			{
				model = value;
                OnPropertyChanged(nameof(Name));
				OnPropertyChanged(nameof(Description));
				OnPropertyChanged(nameof(Price));
				OnPropertyChanged(nameof(Icon));
				OnPropertyChanged(nameof(Image));
			}
		}
		private Skin model;

		public SkinVM(Skin model)
			=> Model = model;

        public string Name => Model.Name;

		public string Description
		{
			get => Model.Description;
			set => SetProperty(Model.Description, value, newValue => Model.Description = newValue);
		}

		public float Price
		{
			get => Model.Price;
			set => SetProperty(Model.Price, value, newValue => Model.Price = newValue);
		}

		public string Icon
		{
			get => Model.Icon;
			set
			{
				SetProperty(Model.Icon, value, newIcon => Model.Icon = newIcon);
			}
		}

		public string Image
		{
			get => Model.Image.Base64;
			set
			{
				SetProperty(Model.Image.Base64, value, newImage => Model.Image.Base64 = newImage);
			}
		}

		public Champion Champion => Model.Champion;
	}
}

