using Firebase.Auth;
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
        OnAppearing();

    }

    private async void LoadDataAsync()
    {
        var userProductLists = await vans.GetUserProductListsAsync();

        // Assuming 'datalist' is your ListView in XAML
        datalist.ItemsSource = userProductLists;
    }
    protected override async void OnAppearing()
    {
        try
        {
            string userKey = App.key;
            Console.WriteLine($"User key: {userKey}");

            var usersInfo = await vans.GetUsersinfoAsync(userKey);

            if (usersInfo != null)
            {
                username.ItemsSource = usersInfo;
                Console.WriteLine($"Users info count: {usersInfo.Count}");
            }
            else
            {
                Console.WriteLine("User information is null. Handle this case if needed.");
                // Handle the case where user information is not available
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"Exception in OnAppearing: {ex.Message}");
        }
    }




    private async void BecomeSellerBTN_Clicked(object sender, EventArgs e)
    {
        string userEmail = App.email;

        //Check if the user email is in the "Request" node
        bool isUserInRequest = await App.FirebaseService.IsUserEmailInRequestAsync(userEmail);

        if (isUserInRequest)
        {
            // Display an alert indicating that the user's email is in the Request list
            await DisplayAlert("Information", "Your email is in the Request list. Your application for becoming a seller is still in process. Please wait for approval.", "OK");
            return;
        }
        else
        {
            // Display an alert indicating that the user's email is not in the Request list
            await DisplayAlert("Information", "Your email is not in the Request list. Your application for becoming a seller is not in the pending list.", "OK");
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

    private async void reportBTN_Clicked(object sender, EventArgs e)
    {
        if (datalist.SelectedItem != null)
        {
            // Pass the App.key to ReportPage
            await Navigation.PushModalAsync(new ReportPage(App.key));
        }
        else
        {
            await DisplayAlert("Alert", "Please Select A Data To Edit", "OK!");
        }
    }



    private async void datalist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            var selectedUser = e.CurrentSelection.FirstOrDefault() as Users;

            if (selectedUser != null)
            {
                App.email = selectedUser.MAIL;
                App.key = await vans.GetUserKey(App.email);

                // Fetch all user data based on the selected email
                var allUserData = await vans.GetUserDataByEmailAsync(App.email);

                if (allUserData != null)
                {
                    // Process or display the fetched user data as needed
                    // For example, you might want to update UI elements or navigate to another page
                    // Pass the data to the next page if needed
                }
                else
                {
                    // Handle the case where user data is not available
                }
            }
            else
            {
                // Reset App.key when selection is cleared
                App.key = null;
            }
        }
    }

}
