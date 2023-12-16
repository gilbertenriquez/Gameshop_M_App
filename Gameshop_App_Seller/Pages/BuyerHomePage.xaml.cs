using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class BuyerHomePage : ContentPage
{
    private Users vans = new Users();
   

    public BuyerHomePage()
    {
        InitializeComponent();

        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        var userProductLists = await vans.GetUserProductListsAsync();

        // Assuming 'datalist' is your ListView in XAML
        datalist.ItemsSource = userProductLists;
    }



    private async void BecomeSellerBTN_Clicked(object sender, EventArgs e)
    {
        string userEmail = App.email;

        // Check if the user email is in the "Request" node
        bool isUserInRequest = await App.FirebaseService.IsUserEmailInRequestAsync(userEmail);

        if (isUserInRequest)
        {
            // Display an alert indicating that the user's email is in the Request list
            await DisplayAlert("Information", "Your email is in the Request list. Your application for becoming a seller is still in process. Please wait for approval.", "OK");
        }
        else
        {
            // Display an alert indicating that the user's email is not in the Request list
            await DisplayAlert("Information", "Your email is not in the Request list. Your application for becoming a seller is not in the pending list.", "OK");
            return;
        }

        // Display a confirmation dialog
        bool userConfirmed = await DisplayAlert("Confirmation", "Do you want to continue?", "Yes", "No");

            if (userConfirmed)
            {
                // Set the user key in App
                string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

                if (!string.IsNullOrEmpty(userKey))
                {
                    App.key = userKey;
                    // You might want to use userKey here as needed
                    await Navigation.PushModalAsync(new Valid_IDpage(userKey));
                }
                else
                {
                    await DisplayAlert("Warning", "No user key found", "OK");
                }
            }
        }
}
