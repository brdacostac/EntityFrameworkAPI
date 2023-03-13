using System;
using CommunityToolkit.Mvvm.Input;
using Model;
using ViewModels;

namespace LolApp.ViewModels
{
	public partial class ApplicationVM
	{
		public ChampionsMgrVM ChampionsMgrVM { get; set; }

		public SkinsMgrVM SkinsMgrVM { get; set; }

		public ApplicationVM(ChampionsMgrVM championsMgrVM, SkinsMgrVM skinsMgrVM)
		{
			ChampionsMgrVM = championsMgrVM;
			SkinsMgrVM = skinsMgrVM;
		}

		[RelayCommand]
		async Task NavigateToChampionDetailsPage(ChampionVM cvm)
		{
			SkinsMgrVM.Champion = cvm;
			SkinsMgrVM.Index = 0;
			SkinsMgrVM.Count = 5;
			await SkinsMgrVM.LoadSkinsCommand.ExecuteAsync(cvm);
			await App.Current.MainPage.Navigation.PushAsync(new ChampionPage(cvm, this));
		}

		[RelayCommand]
		async Task NavigateToAddNewChampionPage()
			=> await App.Current.MainPage.Navigation.PushAsync(new AddChampionPage(ChampionsMgrVM));

		[RelayCommand(CanExecute = nameof(CanNavigateToEditChampionPage))]
		async Task NavigateToEditChampionPage(object champ)
			=> await App.Current.MainPage.Navigation.PushAsync(new AddChampionPage(ChampionsMgrVM, champ as ChampionVM));

		bool CanNavigateToEditChampionPage(object champ) => champ != null && champ is ChampionVM;

		[RelayCommand]
		async Task NavigateToSkinDetailsPage(object svm)
		{
			if (svm == null || svm is not SkinVM) return;
			await App.Current.MainPage.Navigation.PushAsync(new SkinPage(svm as SkinVM, this));
		}

		[RelayCommand]
		async Task NavigateToAddNewSkinPage(ChampionVM champion)
			=> await App.Current.MainPage.Navigation.PushAsync(new AddOrEditSkinPage(SkinsMgrVM, champion));

		[RelayCommand(CanExecute = nameof(CanNavigateToEditSkinPage))]
		async Task NavigateToEditSkinPage(object skin)
			=> await App.Current.MainPage.Navigation.PushAsync(new AddOrEditSkinPage(SkinsMgrVM, skin as SkinVM));

		bool CanNavigateToEditSkinPage(object skin) => skin != null && skin is SkinVM;
	}
}

