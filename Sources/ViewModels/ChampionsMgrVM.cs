using System.Threading.Tasks;
using Model;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Data.SqlTypes;
using System.Reflection;

namespace ViewModels;

public partial class ChampionsMgrVM : ObservableObject
{
    internal IDataManager DataMgr { get; set; }

    public ChampionsMgrVM(IDataManager dataManager)
    {
        DataMgr = dataManager;

        loadingMethods = new Dictionary<LoadingCriterium, Func<object, Task>>()
        {
            [LoadingCriterium.None] = async (o) => await LoadChampions(), 
            [LoadingCriterium.ByName] = async (o) =>
            {
                string substring = o as string;
                if(substring == null) return;
                await LoadChampionsByName(substring);
            },
            [LoadingCriterium.BySkill] = async (o) =>
            {
                string skillString = o as string;
                if(skillString == null) return;
                await LoadChampionsBySkill(skillString);
            },
            [LoadingCriterium.ByCharacteristic] = async (o) =>
            {
                string characString = o as string;
                if(characString == null) return;
                await LoadChampionsByCharacteristic(characString);
            },
            [LoadingCriterium.ByClass] = async (o) =>
            {
                if(!Enum.IsDefined(typeof(ChampionClass), o)) return;
                ChampionClass champClass = (ChampionClass)o;
                await LoadChampionsByClass(champClass);
            },
        };
    }

    private async Task LoadChampionsFunc(Func<Task<IEnumerable<Champion?>>> loader,
                                         Func<Task<int>> nbReader,
                                         LoadingCriterium criterium,
                                         object parameter = null)
    {
        Champions.Clear();
        var someChampions = (await loader()).Select(c => new ChampionVM(c)).ToList();
        foreach (var cvm in someChampions)
        {
            Champions.Add(cvm);
        }
        NbChampions = await nbReader();
        currentLoadingCriterium = criterium;
        currentLoadingParameter = parameter;
    }

    [RelayCommand]
    public async Task LoadChampions()
    {
        await LoadChampionsFunc(async () => await DataMgr.ChampionsMgr.GetItems(index, count, "Name"),
                          async () => await DataMgr.ChampionsMgr.GetNbItems(),
                          LoadingCriterium.None);
    }

    [RelayCommand(CanExecute =nameof(CanLoadChampionsByName))]
    public async Task LoadChampionsByName(string substring)
    {
        await LoadChampionsFunc(async () => await DataMgr.ChampionsMgr.GetItemsByName(substring, index, count, "Name"),
                                async () => await DataMgr.ChampionsMgr.GetNbItemsByName(substring),
                                LoadingCriterium.ByName,
                                substring);
    }
    private bool CanLoadChampionsByName(string substring)
        => !string.IsNullOrWhiteSpace(substring);

    [RelayCommand(CanExecute =nameof(CanLoadChampionsBySkill))]
    public async Task LoadChampionsBySkill(string skill)
    {
        await LoadChampionsFunc(
            async () => await DataMgr.ChampionsMgr.GetItemsBySkill(skill, index, count, "Name"),
            async () => await DataMgr.ChampionsMgr.GetNbItemsBySkill(skill),
            LoadingCriterium.BySkill,
            skill);
    }
    private bool CanLoadChampionsBySkill(string substring) => !string.IsNullOrWhiteSpace(substring);

    [RelayCommand(CanExecute = nameof(CanLoadChampionsByCharacteristic))]
    public async Task LoadChampionsByCharacteristic(string characteristic)
    {
        await LoadChampionsFunc(
            async () => await DataMgr.ChampionsMgr.GetItemsByCharacteristic(characteristic, index, count, "Name"),
            async () => await DataMgr.ChampionsMgr.GetNbItemsByCharacteristic(characteristic),
            LoadingCriterium.ByCharacteristic,
            characteristic);
    }

    private bool CanLoadChampionsByCharacteristic(string characteristic)
        => !string.IsNullOrWhiteSpace(characteristic);

    [RelayCommand]
    public async Task LoadChampionsByClass(ChampionClass champClass)
    {
        if(champClass == ChampionClass.Unknown)
        {
            return;
        }
        await LoadChampionsFunc(
            async () => await DataMgr.ChampionsMgr.GetItemsByClass(champClass, index, count, "Name"),
            async () => await DataMgr.ChampionsMgr.GetNbItemsByClass(champClass),
            LoadingCriterium.ByClass,
            champClass);
    }

    [RelayCommand(CanExecute =nameof(CanDeleteChampion))]
    public async Task<bool> DeleteChampion(object champVM)
    {
        ChampionVM cvm = champVM as ChampionVM;
        if(cvm == null || !Champions.Contains(cvm)) return false;
        bool result = await DataMgr.ChampionsMgr.DeleteItem(cvm.Model);
        if(result)
        {
            Champions.Remove(cvm);
            await LoadChampions();
        }
        return result;
    }
    bool CanDeleteChampion(object cvm)
        => cvm!= null && cvm is ChampionVM && Champions.Contains(cvm);

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
            if(Count == 0 || NbChampions == 0)
            {
                return 0;
            }
            return (NbChampions-1) / Count + 1;
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NbPages))]
    [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
    [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
    private int nbChampions = 0;

    [ObservableProperty]
    private ObservableCollection<ChampionVM> champions = new ObservableCollection<ChampionVM>();

    [RelayCommand(CanExecute =nameof(CanPreviousPage))]
    async Task PreviousPage()
    {
        if(Index > 0)
        {
            Index--;
            await loadingMethods[currentLoadingCriterium](currentLoadingParameter);
        }
    }
    bool CanPreviousPage() => Index > 0;

    [RelayCommand(CanExecute =nameof(CanNextPage))]
    async Task NextPage()
    {
        if(Index < NbPages-1)
        {
           Index++;
           await loadingMethods[currentLoadingCriterium](currentLoadingParameter);
        }
    }
    bool CanNextPage() => Index < NbPages-1;


    enum LoadingCriterium
    {
        None,
        ByName,
        BySkill,
        ByCharacteristic,
        ByClass
    }

    private LoadingCriterium currentLoadingCriterium = LoadingCriterium.None;
    private object currentLoadingParameter = null;

    private Dictionary<LoadingCriterium, Func<object, Task>> loadingMethods;

    public async Task AddChampion(ChampionVM champVM)
    {
        var added = await DataMgr.ChampionsMgr.AddItem(champVM.Model);
        if(added != null)
        {
            Champions.Add(champVM);
            await LoadChampions();
        }
    }

    public async Task EditChampion(ChampionVM oldChampion, ChampionVM newChampion)
    {
        var edited = await DataMgr.ChampionsMgr.UpdateItem(oldChampion.Model, newChampion.Model);
        oldChampion.Model = newChampion.Model;
        if(edited != null)
        {
            await LoadChampions();
        }
    }
}
