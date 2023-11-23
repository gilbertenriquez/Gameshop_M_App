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

    public string webAPIKey = "AIzaSyD87ruvsmZWekQCPyaChBumV9ma9iaWAkY";
    private Users ulogin = new();
    public ICommand TapCommand => new Command(async () => await Navigation.PushModalAsync(new SignUpPage()));
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
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        try
        {

            if (emailEntry.Text == String.Empty)
            {
                await DisplayAlert("Warning", " Please Enter your Email", "OK");
                progressLoading.IsVisible = false;
                return;
            }
            if (passwordEntry.Text == String.Empty)
            {
                await DisplayAlert("Warning", " Please Enter your Password", "OK");
                progressLoading.IsVisible = false;
                return;
            }
            if (String.IsNullOrEmpty(emailEntry.Text) && String.IsNullOrEmpty(passwordEntry.Text))
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                await Navigation.PushModalAsync(new BuyerHomePage());
                progressLoading.IsVisible = true;
            }
            progressLoading.IsVisible = false;
            return;
            }
               
                
        
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            progressLoading.IsVisible = false;
            throw;
        }
        

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

