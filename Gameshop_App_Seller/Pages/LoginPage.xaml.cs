namespace Gameshop_App_Seller.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
       Application.Current!.MainPage = new AppShell();
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignUpPage());  
    }

    private async void lOGINbtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AppShell());
    }
}