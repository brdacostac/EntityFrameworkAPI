using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using ViewModels;

namespace LolApp.ViewModels
{
	public partial class AddSkillVM : ObservableObject
	{
		public AddSkillVM(EditableChampionVM champion)
		{
			Champion = champion;
		}

		[ObservableProperty]
		SkillType skillType;

		[ObservableProperty]
		string name;

		[ObservableProperty]
		string description;

		[ObservableProperty]
		EditableChampionVM champion;

		[RelayCommand]
		async void AddSkillToChampion()
		{
			champion.Skills.Add(new SkillVM(new Skill(name, skillType, description)));
			await App.Current.MainPage.Navigation.PopModalAsync();
		}

		[RelayCommand]
		async void Cancel()
		{
			await App.Current.MainPage.Navigation.PopModalAsync();
		}

		public IEnumerable<SkillType> AllSkills { get; }
			= Enum.GetValues(typeof(SkillType)).Cast<SkillType>().Except(new SkillType[] {SkillType.Unknown}).ToList();
	}
}

