using Gameshop_M_App.Models;
using Gameshop_M_App.Pages;
using Plugin.Connectivity;

namespace Gameshop_M_App.Pages;

public partial class LoginPage : ContentPage
{
     Users gamers = new Users();
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PushModalAsync(new HomePage());	
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignUpPage());  
    }


    private async void lOGINbtn_Clicked(object sender, EventArgs e)
    {
        //var result = await gamers.UsersLogin(emailEntry.Text, passwordEntry.Text);

        //if (string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
        //{
        //    await DisplayAlert("Alert!", "Please Fill up your Email or Password!", "Got it!");
        //    emailEntry.Text = "";
        //    passwordEntry.Text = "";
        //    return;
        //}
        //progressLoading.IsVisible = true;
        //if (result)
        //{
        //    await DisplayAlert("Alert!", "Access Granted!", "OK!");
        //    emailEntry.Text = "";
        //    passwordEntry.Text = "";
            await Navigation.PushModalAsync( new AppShell());
        //    progressLoading.IsVisible = false;
        //    return;

        //}
        //IC_check();
        //await DisplayAlert("Alert!", "Access Denied!", "OK!");
        //progressLoading.IsVisible = false;
        //emailEntry.Text = "";
        //passwordEntry.Text = "";
    }
    //private async void IC_check()
    //{
    //    if (CrossConnectivity.Current.IsConnected)
    //    {
    //        return;
    //    }
    //    await DisplayAlert("Alert", "No Internet Connection", "OK!");
    //    return;
    //}
}