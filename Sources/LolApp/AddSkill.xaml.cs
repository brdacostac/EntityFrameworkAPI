using LolApp.ViewModels;
using ViewModels;

namespace LolApp;

public partial class AddSkill : ContentPage
{
	public AddSkill(EditableChampionVM champion)
	{
		InitializeComponent();
		BindingContext = new AddSkillVM(champion);
	}
}
