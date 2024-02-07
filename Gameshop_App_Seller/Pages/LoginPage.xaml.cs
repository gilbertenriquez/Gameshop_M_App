using Gameshop_App_Seller.Models;
using Firebase.Auth;
using static Gameshop_App_Seller.App;
using Gameshop_App_Seller.Pages;
using Plugin.Connectivity;
using System.Windows.Input;
using Newtonsoft.Json;
using System.ComponentModel;
using Firebase.Database;
using System.Text.RegularExpressions;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using LiteDB;


namespace Gameshop_App_Seller.Pages;

public partial class LoginPage : INotifyPropertyChanged
{

    //public string webAPIKey = "AIzaSyDkunRqHTm1yzzAy59rU_1m9GSxOZkzpoA";
    private Users ulogin = new();
    CancellationTokenSource cancellationTokenSource = new();
    public LoginPage()
    {
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        InitializeComponent();
        bool hasKey = Preferences.ContainsKey("token");
        //if (hasKey)
        //{
        //    string token = Preferences.Get("token", "");
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        Navigation.PushAsync(new HomePage());
        //    }
        //}
    }


    private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess != NetworkAccess.Internet)
        {
            nointernet.IsVisible = true;
            var toast = Toast.Make("You may have no internet connection!", ToastDuration.Long, 12);
            await toast.Show(cancellationTokenSource.Token);
        }
        else
        {
            nointernet.IsVisible = false;
            var toast = Toast.Make("Internet connection Restored!", ToastDuration.Long, 12);
            await toast.Show(cancellationTokenSource.Token);
        }
    }

    private void IC_check()
    {
        if (CrossConnectivity.Current.IsConnected)
        {
            nointernet.IsVisible = false;
            return;
        }
        else
        {
            nointernet.IsVisible = true;
            return;
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        await Navigation.PushModalAsync(new ForgotPassword());
  
    }

    private async void CreateAccountBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PushModalAsync(new SignUpPage());
        progressLoading.IsVisible = false;
    }

 
    private async void btnLOGIN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        if (emailEntry == null || string.IsNullOrEmpty(emailEntry.Text?.Trim()) || string.IsNullOrEmpty(passwordEntry.Text))
        {
            await DisplayAlert("Alert!", "Please fill up your Email and Password!", "Got it!");
            progressLoading.IsVisible = false;
            return;
        }

        string cleanedEmail = emailEntry.Text.Trim().ToLower(); // Convert to lowercase     
        string userEmail = emailEntry.Text;
        string banType = await App.FirebaseService.GetBanType(userEmail);

        if (banType.Equals("Temporary ban", StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Temporary Ban", "You have been banned for 3 days.", "OK");
            progressLoading.IsVisible = false;
            return;
        }
        else if (banType.Equals("Permanent ban", StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Permanent Ban", "Your account has been permanently banned.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        var result = await ulogin.UserLogin(cleanedEmail, passwordEntry.Text);
        string userUid = await GetUserKeyByEmail(cleanedEmail);

        if (!string.IsNullOrEmpty(userUid))
        {
            App.key = userUid;
            App.email = cleanedEmail;

            if (result)
            {
                await DisplayAlert("Information!", "Log In Successfully!", "OK");
                await Navigation.PushModalAsync(new BuyerHomePage(userUid));
                progressLoading.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Information!", "Log In Failed!", "OK");
                progressLoading.IsVisible = false;
            }
        }
        else
        {
            await DisplayAlert("Information!", "Invalid Email or Password", "OK");
            progressLoading.IsVisible = false;
        }

        progressLoading.IsVisible = false;
    }




    private async Task<string> GetUserKeyByEmail(string email)
    {
        var users = await ClientUsers
            .Child("Account")
            .OnceAsync<Users>();

        var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

        return userWithKey?.Key;
    }
}

    //if (string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
    //{
    //    await DisplayAlert("Alert!", "Please Fill up your Email and Password!", "Got it!");
    //    return;
    //}

//progressLoading.IsVisible = true;

//string userUid = await GetUserKeyByEmail(emailEntry.Text);

//if (!string.IsNullOrEmpty(userUid))
//{
//    App.key = userUid;
//    App.email = emailEntry.Text;

//    var result = await ulogin.Login(emailEntry.Text, passwordEntry.Text);

//    if (result)
//    {
//        await DisplayAlert("Alert!", "Access Granted!", "OK!");
//        emailEntry.Text = "";
//        passwordEntry.Text = "";
//await Navigation.PushModalAsync(new MainPage());
//    }
//    else
//    {
//        await DisplayAlert("Alert!", "Access Denied!", "OK!");
//    }
//}
//else
//{
//    await DisplayAlert("Alert!", "Invalid Email or Password", "OK!");
//}

//progressLoading.IsVisible = false;



//    // Authenticate the user
//    bool loginResult = await ulogin.Login(emailEntry.Text, passwordEntry.Text);

//    if (loginResult)
//    {

//        string userEmail = emailEntry.Text;

//        App.email = userEmail;

//        App.key = await ulogin.GetUserKey(App.email);
//        await Navigation.PushModalAsync(new BuyerHomePage());
//    }
//}
//private async Task<bool> YourMethodThatUsesUserKey()
//{
//    await Navigation.PushModalAsync(new BuyerHomePage());
//    return true; // Replace with your logic

//if (String.IsNullOrEmpty(emailEntry.Text))
//{
//    await DisplayAlert("Warning", "Please Enter your Email", "OK!");
//    progressLoading.IsVisible = false;
//    return;
//}
//if (String.IsNullOrEmpty(passwordEntry.Text))
//{
//    await DisplayAlert("Warning", "Please Enter your Password", "OK!");
//    progressLoading.IsVisible = false;
//    return;
//}
//var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
//try
//   {
//    progressLoading.IsVisible = true;
//    var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text);
//    var content = await auth.GetFreshAuthAsync();
//    var serializedContent = JsonConvert.SerializeObject(content);
//    Preferences.Set("FreshFirebaseToken", serializedContent);        
//    await Navigation.PushModalAsync(new BuyerHomePage());          

//    }       
//catch (Exception ex)
//{
//    progressLoading.IsVisible = false;
//    await DisplayAlert("Warning", "Error", "OK!");
//    return;
//}


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


//string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
//var email = emailEntry.Text;
//        if (!Regex.IsMatch(emailEntry.Text, emailPattern))
//        {

//            await DisplayAlert("Warning", "Please Enter A Valid Email Address", "OK");
//progressLoading.IsVisible = false;
//            return;
//        }

//            if (string.IsNullOrEmpty(emailEntry.Text))
//{
//    await DisplayAlert("Warning", "Please Enter Your Email Address", "OK");
//    progressLoading.IsVisible = false;
//    return;
//}

//var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));

//try
//{
//    progressLoading.IsVisible = true;
//    var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text);

//    var password = passwordEntry.Text;

//    // Call the SignIn method to handle the authentication
//    var firebaseToken = await ulogin.SignIn(email, password);
//    // Retrieve the user UID (user key) from Realtime Database
//    string userUid = await GetUserKeyByEmail(emailEntry.Text);

//    if (!string.IsNullOrEmpty(auth.FirebaseToken) && !string.IsNullOrEmpty(userUid))
//    {
//        // Save the UID (user key) in App.key
//        App.key = userUid;
//        App.email = emailEntry.Text;

//        if (!string.IsNullOrEmpty(firebaseToken))
//        {
//            // Continue with your logic if the sign-in is successful
//            // Now you can use 'firebaseToken' as needed.
//            var content = await auth.GetFreshAuthAsync();
//            var serializedContent = JsonConvert.SerializeObject(content);
//            Preferences.Set("FreshFirebaseToken", serializedContent);
//            await Navigation.PushModalAsync(new MainPage());
//        }
//        else
//        {
//            progressLoading.IsVisible = false;
//            await DisplayAlert("Warning", "Invalid Email or Password", "OK!");
//        }
//    }
//}
//catch (Exception ex)
//{
//    progressLoading.IsVisible = false;
//    Console.WriteLine($"Error during login: {ex.Message}");

//    // Display a more informative error message based on the exception
//    string errorMessage = "An error occurred during login.";

//    if (ex is Firebase.Auth.FirebaseAuthException firebaseException)
//    {
//        // Log the entire exception for detailed analysis
//        Console.WriteLine($"Firebase Authentication Exception: {firebaseException}");

//        // Log the error reason
//        Console.WriteLine($"Firebase Authentication Error Reason: {firebaseException.Reason}");

//        // Display a more informative error message based on the exception
//        if (firebaseException.Reason == Firebase.Auth.AuthErrorReason.Undefined)
//        {
//            errorMessage = "Undefined authentication error.";
//        }
//        else if (firebaseException.Reason == Firebase.Auth.AuthErrorReason.InvalidEmailAddress)
//        {
//            errorMessage = "Invalid email address.";
//        }
//        else if (firebaseException.Reason == Firebase.Auth.AuthErrorReason.WrongPassword)
//        {
//            errorMessage = "Invalid password.";
//        }
//    }

//    await DisplayAlert("Warning", errorMessage, "OK");
//}