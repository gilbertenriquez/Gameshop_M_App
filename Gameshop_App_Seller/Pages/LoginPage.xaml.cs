using Gameshop_App_Seller.Models;
using Firebase.Auth;
using Gameshop_App_Seller.Pages;
using Plugin.Connectivity;
using System.Windows.Input;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Gameshop_App_Seller.Pages;

public partial class LoginPage : INotifyPropertyChanged
{

    public string webAPIKey = "AIzaSyDkunRqHTm1yzzAy59rU_1m9GSxOZkzpoA";
    private Users ulogin = new();
    public LoginPage()
    {
        InitializeComponent();
        bool hasKey = Preferences.ContainsKey("token");
        if (hasKey)
        {
            string token = Preferences.Get("token", "");
            if (!string.IsNullOrEmpty(token))
            {
                Navigation.PushAsync(new HomePage());
            }
        }

    }
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPassword());
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignUpPage());
    }

    private async void lOGINbtn_Clicked(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(emailEntry.Text))
        {
            await DisplayAlert("Warning", "Please Enter your Email", "OK!");
            progressLoading.IsVisible = false;
            return;
        }
        if (String.IsNullOrEmpty(passwordEntry.Text))
        {
            await DisplayAlert("Warning", "Please Enter your Password", "OK!");
            progressLoading.IsVisible = false;
            return;
        }
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        try
           {
            progressLoading.IsVisible = true;
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text);
            var content = await auth.GetFreshAuthAsync();
            var serializedContent = JsonConvert.SerializeObject(content);
            Preferences.Set("FreshFirebaseToken", serializedContent);
            await Navigation.PushModalAsync(new BuyerHomePage());          
           
            }       
        catch (Exception ex)
        {
            progressLoading.IsVisible = false;
            await DisplayAlert("Warning", "Error", "OK!");
            return;
        }
        

    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }




    //var result = await ulogin.userLogin(emailEntry.Text, passwordEntry.Text);

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
    //    await Navigation.PushModalAsync(new BuyerHomePage());
    //    progressLoading.IsVisible = false;
    //    return;

    //}
    //await DisplayAlert("Alert!", "Access Denied!", "OK!");
    //progressLoading.IsVisible = false;
    //emailEntry.Text = "";
    //passwordEntry.Text = "";


}

