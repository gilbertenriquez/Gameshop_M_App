namespace Gameshop_App_Seller.Pages;

public partial class PrivacyPolicyPage : ContentPage
{
	public PrivacyPolicyPage()
	{

        if (!CheckInternetConnection())
        {
            // Optionally display an alert or take appropriate action if there's no internet
            return;
        }
        InitializeComponent();
	}

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }

    private bool CheckInternetConnection()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            DisplayAlert("Error", "No internet connection. Please check your network settings.", "OK");
            return false;
        }
        return true;
    }
}