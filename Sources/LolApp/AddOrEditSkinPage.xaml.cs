using LolApp.ViewModels;
using ViewModels;

namespace LolApp;

public partial class AddOrEditSkinPage : ContentPage
{
	AddOrEditSkinPage()
	{
		InitializeComponent();
	}

	public AddOrEditSkinPage(SkinsMgrVM skinsMgrVM, SkinVM skin)
		:this()
	{
		BindingContext = new AddOrEditSkinPageVM(skinsMgrVM, skin);
	}

	public AddOrEditSkinPage(SkinsMgrVM skinsMgrVM, ChampionVM champion)
		:this()
	{
		BindingContext = new AddOrEditSkinPageVM(skinsMgrVM, champion);
	}
}
