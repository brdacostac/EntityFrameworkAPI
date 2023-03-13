using CommunityToolkit.Maui.Behaviors;
using LolApp.ViewModels;
using ViewModels;

namespace LolApp;

public partial class ChampionPage : ContentPage
{
	public ApplicationVM AppVM { get; set; }
	public ChampionVM Champion { get; }

	public ChampionPage(ChampionVM cvm, ApplicationVM appVM)
	{
		AppVM = appVM;
		BindingContext = Champion = cvm;

		InitializeComponent();
	}

    void imgClass_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
		Image img = sender as Image;
		if(e.PropertyName == "Source" && img != null && img.Behaviors.Any(b => b is IconTintColorBehavior))
		{
			var beh = (img.Behaviors.First(b => b is IconTintColorBehavior) as IconTintColorBehavior);
			var color = beh.TintColor;
			img.Behaviors.Remove(beh);
			img.Behaviors.Add(new IconTintColorBehavior() { TintColor = color});
		}
    }
}
