namespace Gameshop_App_Seller.Pages;

public partial class PrivacyPolicyPage : ContentPage
{
	public PrivacyPolicyPage()
	{
		InitializeComponent();
	}

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}