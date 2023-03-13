using Model;

namespace LolApp.ContentViews;

public partial class ChampionClassSelector : ContentView
{
	public ChampionClassSelector()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(nameof(SelectedValue), typeof(ChampionClass), typeof(ChampionClassSelector), ChampionClass.Unknown, BindingMode.TwoWay);
    public ChampionClass SelectedValue
    {
        get => (ChampionClass)GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public static readonly BindableProperty CheckedColorProperty = BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(ChampionClassSelector), Colors.DarkSalmon);

    public Color CheckedColor
    {
        get => (Color)GetValue(CheckedColorProperty);
        set => SetValue(CheckedColorProperty, value);
    }

    public static readonly BindableProperty UncheckedColorProperty = BindableProperty.Create(nameof(UncheckedColor), typeof(Color), typeof(ChampionClassSelector), Colors.DarkSalmon);

    public Color UncheckedColor
    {
        get => (Color)GetValue(UncheckedColorProperty);
        set => SetValue(UncheckedColorProperty, value);
    }
}
