using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;
using Microsoft.Maui.Controls;

namespace Gameshop_App_Seller.Pages;

public partial class SellerSettings : ContentPage
{
    private Users Info = new Users();
    private string userId;
	public SellerSettings()
	{

        InitializeComponent();
    }

    public SellerSettings(string userId)
    {
        InitializeComponent();
        this.userId = userId; // Store the user ID
    }


    protected async override void OnAppearing()
    {
        base.OnAppearing();

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
                imglogo.Source = !string.IsNullOrEmpty(userSnapshot.ProfilePicture)
                    ? new UriImageSource
                    {
                        Uri = new Uri(userSnapshot.ProfilePicture),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    }
                    : "account.png";

                FirstName.Text = userSnapshot.FNAME;
                LastName.Text = userSnapshot.LNAME;
            }
           
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during data retrieval
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async void ProfileDetailBTN_Tapped(object sender, TappedEventArgs e)
    {
        
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }
        await Navigation.PushModalAsync(new ProfileDetail(App.key));
 
    }

    private async void PrivacyPoliBTN_Tapped(object sender, TappedEventArgs e)
    {
        
        await Navigation.PushModalAsync(new PrivacyPolicyPage());
       
    }

    private async void TandCBTN_Tapped(object sender, TappedEventArgs e)
    {
       
        await Navigation.PushModalAsync(new TermsAndConditionPage());
       
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {

        await Navigation.PopModalAsync();

    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }
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
                await Task.Delay(1000);

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
}