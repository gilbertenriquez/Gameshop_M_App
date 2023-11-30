using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;


public partial class BuyerHomePage : ContentPage
{


    private Users vans = new Users();

    public BuyerHomePage()
    {
        InitializeComponent();

      
    }

    private async void BecomeSellerBTN_Clicked(object sender, EventArgs e)
    {
        // Obtain the user email from your source; replace this with your actual logic
        string userEmail = App.email;

        // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
        string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

        if (!string.IsNullOrEmpty(userKey))
        {
            bool user = await DisplayAlert("Confirmation", "Do you want to continue?", "Yes", "No");
            if (user)
            {
                App.key = userKey;
                // You might want to use userKey here as needed
                await Navigation.PushModalAsync(new AppShell(userKey));
            }
        }
        else
        {
            await DisplayAlert("Warning", "No user key found", "OK");
        }
    }


  
}