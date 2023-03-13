using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
//using Microsoft.Maui.Graphics.Platform;
using ViewModels;

namespace LolApp.ViewModels
{
	[ObservableObject]
	public partial class AddChampionPageVM
	{
        ChampionsMgrVM ChampionsMgrVM { get; }

		public AddChampionPageVM(ChampionsMgrVM championsMgrVM, ChampionVM champion = null)
        {
            ChampionsMgrVM = championsMgrVM;
            if(champion == null) return;

            oldChampion = champion;
            IsNew = false;
            this.champion = new EditableChampionVM(oldChampion);
        }

        private ChampionVM oldChampion;

        [ObservableProperty]
        bool isNew = true;

        [ObservableProperty]
        EditableChampionVM champion = new ();

        [RelayCommand]
        public async void PickIcon() => Champion.IconBase64 = await PickIconsAndImagesUtils.PickPhoto(42);

        [RelayCommand]
        public async void PickLargeImage() => Champion.LargeImageBase64 = await PickIconsAndImagesUtils.PickPhoto(1000);

        [RelayCommand]
		async Task Cancel()
			=> await App.Current.MainPage.Navigation.PopAsync();

        [RelayCommand]
        async Task AddChampion()
        {
            ChampionVM champVM = Champion.ToChampionVM();
            await ChampionsMgrVM.AddChampion(champVM);
            await App.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        async Task EditChampion()
        {
            ChampionVM newChampion = Champion.ToChampionVM();
            await ChampionsMgrVM.EditChampion(oldChampion, newChampion);
            await App.Current.MainPage.Navigation.PopAsync();
        }

        [ObservableProperty]
        string newCharacteristicDescription;

        [ObservableProperty]
        int newCharacteristicValue;

        [RelayCommand]
        void AddCharacteristic()
        {
            Champion?.AddCharacteristic(newCharacteristicDescription, newCharacteristicValue);
        }

        [RelayCommand]
        void RemoveCharacteristic(KeyValuePair<string, int> characteristic)
            => Champion?.RemoveCharacteristic(characteristic);

        [RelayCommand]
        async Task AddSkill()
            => await App.Current.MainPage.Navigation.PushModalAsync(new AddSkill(Champion));

	}
}

