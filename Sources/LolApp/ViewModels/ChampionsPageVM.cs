using System;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using ViewModels;

namespace LolApp.ViewModels
{
    [ObservableObject]
	public partial class ChampionsPageVM
	{
		public ChampionsMgrVM ChampionsMgrVM { get; set; } 

		public ChampionsPageVM(ChampionsMgrVM championsMgrVM)
		{
			ChampionsMgrVM = championsMgrVM;
            PropertyChanged += ChampionsMgrVM_PropertyChanged;

		}

        [ObservableProperty]
        private ChampionClassVM selectedClass;


		[RelayCommand]
        public async Task SelectedChampionClassChanged(ChampionClassVM champClass)
        {
            if(SelectedClass != null) SelectedClass.IsSelected = false;
            if(champClass.Model == ChampionClass.Unknown
                || champClass.Model == SelectedClass?.Model)
            {
                SelectedClass = null;
                return;
            }
            SelectedClass = champClass;
            SelectedClass.IsSelected = true;
            await ChampionsMgrVM.LoadChampionsByClass(SelectedClass.Model);//ChampionsMgrVM.SelectedClass);

        }


        [ObservableProperty]
        private ChampionVM selectedChampion;

        [ObservableProperty]
        private string searchedName;

    

        [ObservableProperty]
        private string searchedSkill;

        [ObservableProperty]
        private string searchedCharacteristic;

        private static string[] searchedStrings = { nameof(SearchedName), nameof(SearchedSkill), nameof(SearchedCharacteristic), nameof(SelectedClass) };

        private async void ChampionsMgrVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(searchedStrings.Any(s => e.PropertyName == s))
            {
                if(GetProperty(e.PropertyName).GetValue(this) != GetProperty(e.PropertyName).GetDefaultValue())
                {
                    foreach(string s in searchedStrings.Except(new string[]{e.PropertyName }))
                    {
                        var prop = GetProperty(s);
                        prop.ResetPropertyValue(this);
                    }
                    return;
                }
                ChampionsMgrVM.Index=0;
                if(searchedStrings.All(s => GetProperty(s).GetValue(this) == GetProperty(s).GetDefaultValue()))
                {
                    await ChampionsMgrVM.LoadChampions();
                }
            }
        }

        private PropertyInfo? GetProperty(string propName)
            => typeof(ChampionsPageVM).GetProperty(propName);

	}

    public static class Extensions
    {
        public static void ResetPropertyValue(this PropertyInfo pi, ChampionsPageVM instance)
        {
            if(pi.PropertyType == typeof(ChampionClassVM))
            {
                var temp = pi.GetValue(instance);
                if(temp != null)
                    (temp as ChampionClassVM).IsSelected = false;
                return;
            }
            pi.SetValue(instance, pi.GetDefaultValue());
        }

        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        public static object GetDefaultValue(this PropertyInfo pi)
            => pi.PropertyType.GetDefaultValue();
    }
}

