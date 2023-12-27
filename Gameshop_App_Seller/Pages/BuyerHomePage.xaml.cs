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
        //OnAppearingDenied();

    }



    //protected async void OnAppearingDenied()
    //{
    //    base.OnAppearing();
    //    string userEmail = App.email;
    //    var deniedApplicationsList = vans.GetDeniedApplicationsList();
    //    // Assuming you have the user's email stored in App.email

    //    // Assuming Users is your class representing denied applications
    //    var deniedApplication = deniedApplicationsList.FirstOrDefault(app => app.MAIL == userEmail);

    //    if (deniedApplication != null)
    //    {
    //        // Display a message with the DeniedReason
    //        await DisplayAlert("Denied Application", $"Denied Reason: {deniedApplication.DeniedReason}", "OK");
    //    }
    //    else
    //    {
    //        // Handle the case where the denied application is not found
    //        await DisplayAlert("Error", "Denied application not found.", "OK");
    //    }
    //}



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

        bool isUserInVerified = await App.FirebaseService.IsUserEmailInVerifiedAsync(userEmail);

        // Set the user key in App  
        string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);
        if (isUserInRequest)
        {
            // Display an alert indicating that the user's email is in the Request list
            await DisplayAlert("Information", "You Are Verified", "OK");
            await Navigation.PushModalAsync(new AppShell(userKey));
            return;
        }


        if (isUserInRequest)
        {
            // Display an alert indicating that the user's email is in the Request list
            await DisplayAlert("Information", "Your email is in the Request list. Your application for becoming a seller is still in process. Please wait for approval.", "OK");
            return;
        }
        else
        {
            // Display an alert indicating that the user's email is not in the Request list
            await DisplayAlert("Information", "Please submit your verification ID's to access the Seller Mode", "Proceed");
            await Navigation.PushModalAsync(new Valid_IDpage(userKey));
            return;
        }

        // Display a confirmation dialog
        bool userConfirmed = await DisplayAlert("Confirmation", "Do you want to continue?", "Yes", "No");

        if (userConfirmed)
        {        
            if (!string.IsNullOrEmpty(userKey))
            {
                App.key = userKey;
                // You might want to use userKey here as needed
                await Navigation.PushModalAsync(new AppShell(userKey));
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
            var selectedUser = datalist.SelectedItem as Users;

            if (selectedUser != null)
            {
                // Pass the necessary data to ReportPage, including reporter's email
                await Navigation.PushModalAsync(new ReportPage(selectedUser.ProductName, selectedUser.ProductPrice, selectedUser.MAIL, selectedUser.image1, selectedUser.ReporterEmail));
            }
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
                // Set App.email based on the selected user's email
                App.productname = selectedUser.ProductName.ToLower();

                // Retrieve the user key based on the email
                App.key = await vans.GetUserKey(App.productname);

                // Fetch additional user data, including ReporterEmail
                selectedUser = await vans.GetUserDataByEmailAsync(App.email);

                // Load user data to update UI

            }
            else
            {
                // Reset App.key when selection is cleared
                App.key = null;
            }
        }
    }
}
