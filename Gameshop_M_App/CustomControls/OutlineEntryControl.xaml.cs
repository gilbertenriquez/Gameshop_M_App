using static System.Net.Mime.MediaTypeNames;

namespace Gameshop_M_App.CustomControls;

public partial class OutlineEntryControl : Grid
{
    public OutlineEntryControl()
    {
        InitializeComponent();
    }
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(OutlineEntryControl),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(OutlineEntryControl),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    private void txtEntry_Focused(object sender, FocusEventArgs e)
    {

        LblPlaceholder.FontSize = 11;
        LblPlaceholder.TranslateTo(0, -26, 80, easing: Easing.Linear);
    }



    private void txtEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {

            LblPlaceholder.FontSize = 11;
            LblPlaceholder.TranslateTo(0, -26, 80, easing: Easing.Linear);
        }
        else
        {
            LblPlaceholder.FontSize = 15;
            LblPlaceholder.TranslateTo(0, 0, 80, easing: Easing.Linear);
        }
    }
}
