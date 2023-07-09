namespace Gameshop_M_App.Pages;

public partial class BuyerSignUpPage : ContentPage
{
	public BuyerSignUpPage()
	{
		InitializeComponent();
	}

    private async void SaveBTN_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}