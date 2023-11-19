namespace Gameshop_App_Seller.Pages;

public partial class BuyerHomePage : ContentPage
{
	public BuyerHomePage()
	{
        InitializeComponent();
	}

    private async void BecomeSellerBTN_Clicked(object sender, EventArgs e)
    {
		bool user = await DisplayAlert("Confirmation", "Do you want to continue?", "Yes", "No");
		if (user)
		{
			await Navigation.PushModalAsync(new AppShell());
		}
    }
}