using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;

namespace ViewModels
{
	[ObservableObject]
	public partial class SkinsMgrVM
	{
		internal IDataManager DataMgr { get; set; }

        [ObservableProperty]
        private ChampionVM champion;

		public SkinsMgrVM(IDataManager dataManager)
		{
			DataMgr = dataManager;
		}

		[ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int index = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NbPages))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int count = 5;

        public int NbPages
        {
            get
            { 
                if(Count == 0 || NbSkins == 0)
                {
                    return 0;
                }
                return (NbSkins-1) / Count + 1;
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NbPages))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int nbSkins = 0;

        [ObservableProperty]
        private ObservableCollection<SkinVM> skins = new ObservableCollection<SkinVM>();

        [RelayCommand]
        async Task LoadSkins()
        {
            Skins.Clear();
            IEnumerable<Skin?> skins;
            if(Champion != null)
            {
                skins = await DataMgr.SkinsMgr.GetItemsByChampion(Champion.Model, Index, Count,"Name");
                
            }
            else
            {
                skins = await DataMgr.SkinsMgr.GetItems(Index, Count, "Name");
            }

            foreach(var skin in skins)
            {
                if(skin != null)
                    Skins.Add(new SkinVM(skin));
            }
        }

        [RelayCommand(CanExecute =nameof(CanPreviousPage))]
        async Task PreviousPage()
        {
            if(Index > 0)
            {
                Index--;
                await LoadSkins();
            }
        }
        bool CanPreviousPage() => Index > 0;

        [RelayCommand(CanExecute =nameof(CanNextPage))]
        async Task NextPage()
        {
            if(Index < NbPages-1)
            {
               Index++;
               await LoadSkins();
            }
        }
        bool CanNextPage() => Index < NbPages-1;

        [RelayCommand(CanExecute =nameof(CanDeleteSkin))]
        public async Task<bool> DeleteSkin(object skinVM)
        {
            SkinVM svm = skinVM as SkinVM;
            if(svm == null || !Skins.Contains(svm)) return false;
            bool result = await DataMgr.SkinsMgr.DeleteItem(svm.Model);
            if(result)
            {
                Skins.Remove(svm);
                await LoadSkins();
            }
            return result;
        }
        bool CanDeleteSkin(object svm)
            => svm!= null && svm is SkinVM && Skins.Contains(svm);

        public async Task AddSkin(SkinVM skinVM)
        {
            var added = await DataMgr.SkinsMgr.AddItem(skinVM.Model);
            if(added != null)
            {
                Skins.Add(skinVM);
                await LoadSkins();
            }
        }

        public async Task EditSkin(SkinVM oldSkin, SkinVM newSkin)
        {
            var edited = await DataMgr.SkinsMgr.UpdateItem(oldSkin.Model, newSkin.Model);
            oldSkin.Model = newSkin.Model;
            if(edited != null)
            {
                await LoadSkins();
            }
        }
	}
}

