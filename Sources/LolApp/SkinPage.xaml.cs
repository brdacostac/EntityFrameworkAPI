using LolApp.ViewModels;
using ViewModels;

namespace LolApp;

public partial class SkinPage : ContentPage
{
	public ApplicationVM AppVM { get; set; }
	public SkinVM SkinVM { get; }

	public SkinPage(SkinVM svm, ApplicationVM appVM)
	{
		BindingContext = SkinVM = svm;
		AppVM = appVM;

		InitializeComponent();
	}
}
