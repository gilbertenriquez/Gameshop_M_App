using Gameshop_M_App.Pages;
namespace Gameshop_M_App.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PushAsync(new HomePage());	
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());  
    }
}