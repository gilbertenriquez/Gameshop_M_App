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

        if (!CheckInternetConnection())
        {
            // Optionally display an alert or take appropriate action if there's no internet
            return;
        }

        InitializeComponent();
    }

    public SellerSettings(string userId)
    {
        InitializeComponent();
        this.userId = userId; // Store the user ID
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
        progressLoading.IsVisible = true;
        await Navigation.PushModalAsync(new ProfileDetail(App.key));
        progressLoading.IsVisible = false;
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
}