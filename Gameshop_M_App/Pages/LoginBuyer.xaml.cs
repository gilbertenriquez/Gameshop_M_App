namespace Gameshop_M_App.Pages;

public partial class LoginBuyer : ContentPage
{
	public LoginBuyer()
	{
		InitializeComponent();
	}

    private async void lOGINbtn_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new BuyerHomePage());
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new BuyerSignUpPage());    
    }
}