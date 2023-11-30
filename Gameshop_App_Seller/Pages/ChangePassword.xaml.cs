using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class ChangePassword : ContentPage
{
    private Users ChngePass = new();
	public ChangePassword()
	{
		InitializeComponent();
	}

    private async void BtnChangePassword_Clicked(object sender, EventArgs e)
    {

        //try
        //{
        //    string password = TxtPassword.Text;
        //    string confirmPass = TxtConfirm.Text;
        //    if (string.IsNullOrEmpty(password))
        //    {
        //        await DisplayAlert("Change Password", "Please type password", "Ok");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(confirmPass))
        //    {
        //        await DisplayAlert("Change Password", "Please type confirm password", "Ok");
        //        return;
        //    }
        //    if (password != confirmPass)
        //    {
        //        await DisplayAlert("Change Password", "Confirm password not match.", "Ok");
        //        return;
        //    }
        //    string token = Preferences.Get("token", "");
        //    string newToken = await ChngePass.ChangePassword(token, password);
        //    if (!string.IsNullOrEmpty(newToken))
        //    {
        //        await DisplayAlert("Change Password", "Password has been changed.", "Ok");
        //        Preferences.Set("token", newToken);
        //        //Preferences.Clear();
        //        //await Navigation.PushAsync(new LoginPage());
        //    }
        //    else
        //    {
        //        await DisplayAlert("Change Password", "Password Change failed.", "Ok");
        //    }
        //}
        //catch (Exception exception)
        //{
        //    return;
        //}
    }

}
