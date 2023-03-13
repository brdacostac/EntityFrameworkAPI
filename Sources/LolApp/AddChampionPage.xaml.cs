using LolApp.ViewModels;
using ViewModels;

namespace LolApp;

public partial class AddChampionPage : ContentPage
{
	public AddChampionPage(ChampionsMgrVM championsMgrVM, ChampionVM champion = null)
	{
		InitializeComponent();
		BindingContext = new AddChampionPageVM(championsMgrVM, champion);
	}
}
