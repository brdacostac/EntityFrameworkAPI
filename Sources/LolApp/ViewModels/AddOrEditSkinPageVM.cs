using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using ViewModels;

namespace LolApp.ViewModels
{
	[ObservableObject]
	public partial class AddOrEditSkinPageVM
	{
		SkinsMgrVM SkinsMgrVM { get; }

		private SkinVM oldSkin;

		[ObservableProperty]
        bool isNew = true;

		[ObservableProperty]
		EditableSkinVM skin;

		public AddOrEditSkinPageVM(SkinsMgrVM skinsMgrVM, SkinVM oldSkin)
		{
			SkinsMgrVM = skinsMgrVM;

            this.oldSkin = oldSkin;
            IsNew = false;
            this.skin = new EditableSkinVM(oldSkin);
		}

		public AddOrEditSkinPageVM(SkinsMgrVM skinsMgrVM, ChampionVM champion)
		{
			SkinsMgrVM = skinsMgrVM;
			skin = new EditableSkinVM(champion);
		}

		[RelayCommand]
        public async void PickIcon() => Skin.IconBase64 = await PickIconsAndImagesUtils.PickPhoto(42);

        [RelayCommand]
        public async void PickLargeImage() => Skin.LargeImageBase64 = await PickIconsAndImagesUtils.PickPhoto(1000);

        [RelayCommand]
		async Task Cancel()
			=> await App.Current.MainPage.Navigation.PopAsync();

        [RelayCommand]
        async Task AddSkin()
        {
            SkinVM skinVM = Skin.ToSkinVM();
            await SkinsMgrVM.AddSkin(skinVM);
            await App.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        async Task EditSkin()
        {
            SkinVM newSkin = Skin.ToSkinVM();
            await SkinsMgrVM.EditSkin(oldSkin, newSkin);
            await App.Current.MainPage.Navigation.PopAsync();
        }
	}
}

