using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;
using static System.Net.Mime.MediaTypeNames;


namespace Gameshop_App_Seller.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        LoadUserDataAsync();
    }


    private async Task LoadUserDataAsync()
    {
        try
        {
            var userData = App.key;
            var userAccountPath = $"Account/{userData}";

            // Retrieve the user data from the database
            var userSnapshot = await ClientUsers
                .Child(userAccountPath)
                .OnceSingleAsync<Users>();

            // Check if the user data exists
            if (userSnapshot != null)
            {
                // Update the UI with the retrieved user data
                imglogo.Source = !string.IsNullOrEmpty(userSnapshot.ShopProfile)
                    ? new UriImageSource
                    {
                        Uri = new Uri(userSnapshot.ShopProfile),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    }
                    : "account.png";

                lblcompanyname.Text = userSnapshot.ShopName;
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during data retrieval
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async void LogoutBTN_Tapped(object sender, TappedEventArgs e)
    {
        bool answer = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");

        if (answer)
        {
            progressLoading.IsVisible = true;

            try
            {
                App.email = null;
                App.key = null;
                App.UserKey = null;

                // Simulate a delay for demonstration purposes
                await Task.Delay(3000);

                // Navigate to the login page or the initial page of your app 
                await Navigation.PushModalAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the logout process
                Console.WriteLine($"Error during logout: {ex.Message}");
            }
            finally
            {
                progressLoading.IsVisible = false;
            }
        }
    }


    private async void BTNshopDetails_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ShopDetail(App.key));
    }

    private async void TandCBTN_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new TermsAndConditionPage());
    }

    private async void PrivacyPoliBTN_Tapped(object sender, TappedEventArgs e)
    {
      await Navigation.PushModalAsync(new PrivacyPolicyPage());
    }
}