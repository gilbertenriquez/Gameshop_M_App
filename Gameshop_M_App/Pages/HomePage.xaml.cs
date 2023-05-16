using Gameshop_M_App.Pages;
namespace Gameshop_M_App.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void GetStart_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new LoginPage());
    }
}