namespace Gameshop_M_App.Pages;

public partial class SellerBuyerPage : ContentPage
{
	public SellerBuyerPage()
	{
		InitializeComponent();
	}

    private async void sellerBTN_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new LoginPage());
    }

    private async void BuyerBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginBuyer());
    }
}