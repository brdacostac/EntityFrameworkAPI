using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;


namespace ViewModels
{
	public partial class ChampionVM : ObservableObject
	{
		public Champion Model
		{
			get => model;
			set
			{
				model = value;
				OnPropertyChanged(nameof(Name));
				OnPropertyChanged(nameof(Bio));
				OnPropertyChanged(nameof(Class));
				OnPropertyChanged(nameof(Icon));
				OnPropertyChanged(nameof(Image));
				OnPropertyChanged(nameof(Skills));
				OnPropertyChanged(nameof(Characteristics));
			}
		}
		private Champion model;

		public ChampionVM(Champion model)
		{
			Model = model;
			foreach(var skill in Model.Skills)
			{
				Skills.Add(new SkillVM(skill));
			}
            Skills.CollectionChanged += Skills_CollectionChanged;
		}

        private void Skills_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
			SkillVM vm = e.NewItems?[0] as SkillVM;
            switch(e.Action)
			{
				case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					Model.Skills.Add(new Skill(vm.Name, vm.Type, vm.Description));
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
					Model.Skills.Remove(vm.Model);
					break;
			}
        }

        public string Name => Model.Name;

		public ChampionClass Class
		{
			get => Model.Class;
			set => SetProperty(Model.Class, value, newValue => Model.Class = newValue);
		}

		public string Bio
		{
			get => Model.Bio;
			set => SetProperty(Model.Bio, value, newBio => Model.Bio = newBio);
		}

		public string Icon
		{
			get => Model.Icon;
			set
			{
				SetProperty(Model.Icon, value, newIcon =>
				{
					Model.Icon = newIcon;
				});
			}
		}

		public string Image
		{
			get => Model.Image.Base64;
			set
			{
				SetProperty(Model.Image.Base64, value, newImage =>
				{
					Model.Image.Base64 = newImage;
				});
			}
		}

		[ObservableProperty]
		private ObservableCollection<SkillVM> skills = new ObservableCollection<SkillVM>();

		public ReadOnlyDictionary<string, int> Characteristics
			=> Model.Characteristics;

		[ObservableProperty]
		private ObservableCollection<SkinVM> skins = new ObservableCollection<SkinVM>();
	}
}

