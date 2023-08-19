namespace Gameshop_App_Seller.Pages;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
	private async void SaveBTN_Clicked(object sender, EventArgs e)
	{ 
	await Navigation.PopModalAsync();
	}
    }