namespace Gameshop_App_Seller.Pages;

public partial class TermsAndConditionPage : ContentPage
{
	public TermsAndConditionPage()
	{
        if (CheckInternetConnection())
        {
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