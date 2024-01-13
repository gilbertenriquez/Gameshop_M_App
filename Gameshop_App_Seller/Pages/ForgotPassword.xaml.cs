using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class ForgotPassword : ContentPage
{
    private Users userReset = new ();
	public ForgotPassword()
	{
        if (!CheckInternetConnection())
        {
            // Optionally display an alert or take appropriate action if there's no internet
            return;
        }

        InitializeComponent();
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

    private async void BtnSendLink_Clicked(object sender, EventArgs e)
    {
        string email = TxtEmail.Text;
        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("Warning", "Please enter your email.", "Ok");
            return;
        }

        bool isSend = await userReset.ResetPassword(email);
        if (isSend)
        {
            await DisplayAlert("Reset Password", "We Send A link in your email.", "Ok");
        }
        else
        {
            await DisplayAlert("Reset Password", "Link send failed.", "Ok");
        }


    }


    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}