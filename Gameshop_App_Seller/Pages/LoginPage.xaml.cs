namespace Gameshop_App_Seller.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new HomePage());
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignUpPage());  
    }

}