using LolApp.ViewModels;
using ViewModels;

namespace LolApp;

public partial class ChampionsPage : ContentPage
{
	public ApplicationVM AppVM { get; }
	public ChampionsPageVM VM { get; }
	public ChampionsPage(ApplicationVM appVM)
	{
		InitializeComponent();
		AppVM = appVM;
		VM = new ChampionsPageVM(AppVM.ChampionsMgrVM);
		BindingContext = this;
	}
}
