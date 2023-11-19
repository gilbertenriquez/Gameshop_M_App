namespace Gameshop_App_Seller.Pages;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
	private async void BactBTN_Clicked(object sender, EventArgs e)
	{ 
	await Navigation.PopModalAsync();
	}

    private async void nextBTN_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new Valid_IDpage());
    }
}