using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class ForgotPassword : ContentPage
{
    private Users userReset = new ();
	public ForgotPassword()
	{
		InitializeComponent();
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