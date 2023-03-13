using System.Windows.Input;

namespace LolApp.ContentViews;

public class SearchByStringView : ContentView
{
    public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(SearchByStringView), string.Empty);

    public string PlaceHolder
    {
        get => (string)GetValue(PlaceHolderProperty);
        set => SetValue(PlaceHolderProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(SearchByStringView), string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SearchByStringView), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(SearchByStringView), null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}
