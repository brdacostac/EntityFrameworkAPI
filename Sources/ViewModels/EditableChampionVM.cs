using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;

namespace ViewModels
{
	[ObservableObject]
	public partial class EditableChampionVM
	{
		public EditableChampionVM()
		{ }

		public EditableChampionVM(ChampionVM championVM)
		{
			Name = championVM.Name;
            IconBase64 = championVM.Icon;
            LargeImageBase64 = championVM.Image;
            Bio = championVM.Bio;
            ChampionClass = championVM.Class;
            foreach(var ch in championVM.Characteristics)
            {
                AddCharacteristic(ch.Key, ch.Value);
            }
            foreach(var skill in championVM.Skills)
            {
                Skills.Add(skill);
            }
		}

		[ObservableProperty]
		string name;

        [ObservableProperty]
        string iconBase64;

        [ObservableProperty]
        string largeImageBase64;

		[ObservableProperty]
		string bio;

		[ObservableProperty]
		ChampionClass championClass;

		[ObservableProperty]
		ObservableCollection<KeyValuePair<string, int>> characteristics = new ();

		public void AddCharacteristic(string description, int value)
			=> Characteristics.Add(new KeyValuePair<string, int>(description, value));

		public void RemoveCharacteristic(KeyValuePair<string, int> characteristic)
			=> Characteristics.Remove(characteristic);

		[ObservableProperty]
		ObservableCollection<SkillVM> skills = new ObservableCollection<SkillVM>();



		public ChampionVM ToChampionVM()
		{
			var champion = new Champion(name, championClass, iconBase64, largeImageBase64, bio);
			champion.AddCharacteristics(characteristics.Select(kvp => Tuple.Create(kvp.Key, kvp.Value)).ToArray());
			foreach(var skillVM in Skills)
			{
				champion.AddSkill(skillVM.Model);
			}
			return new ChampionVM(champion);
		}
	}
}

