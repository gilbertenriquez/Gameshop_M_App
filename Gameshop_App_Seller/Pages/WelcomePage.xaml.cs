namespace Gameshop_App_Seller.Pages;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private async void GetStart_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new LoginPage());	
    }
}