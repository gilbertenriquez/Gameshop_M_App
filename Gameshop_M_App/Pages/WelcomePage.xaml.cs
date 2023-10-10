using Gameshop_M_App.Pages;
namespace Gameshop_M_App.Pages;

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