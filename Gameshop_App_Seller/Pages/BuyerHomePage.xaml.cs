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
    public BuyerHomePage(string userKey) : this()
    {
        InitializeAsync(userKey);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Clear the selection in your datalist
        datalist.SelectedItem = null;

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
                string profilePictureUrl = usersInfo[0].ProfilePicture;
                // Set the image source
                SetImageSource(profilePictureUrl);
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


    private void SetImageSource(string imageUrl)
    {
        if (!string.IsNullOrEmpty(imageUrl))
        {
            imglogo.Source = new UriImageSource
            {
                Uri = new Uri(imageUrl),
                CachingEnabled = true,
                CacheValidity = TimeSpan.FromDays(1) // Set an appropriate cache validity period
            };
        }
        else
        {
            // Set a default image URL or file path if needed
            imglogo.Source = "account.png";
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

    private async void ShopBTN_Clicked(object sender, EventArgs e)
    {
        string userEmail = App.email;

        //Check if the user email is in the "Request" node
        bool isUserInRequest = await App.FirebaseService.IsUserEmailInRequestAsync(userEmail);

        bool isUserInVerified = await App.FirebaseService.IsUserEmailInVerifiedAsync(userEmail);

        // Set the user key in App  
        string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);
        if (isUserInVerified)
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

    private async void ViewProductBTN_Tapped_1(object sender, TappedEventArgs e)
    {
        var selectedProduct = e.Parameter as Users; // Replace Users with your actual type

        if (selectedProduct != null)
        {
            App.productname = selectedProduct.ProductName?.ToLower();
            App.key = await vans.GetUserKey(App.productname);

            // Check for null or empty values before passing them to the constructor
            string email = selectedProduct.MAIL ?? string.Empty;
            string productName = selectedProduct.ProductName ?? "Unknown Product";
            string productPrice = selectedProduct.ProductPrice ?? "Unknown Price";
            string productDescriptions = selectedProduct.ProductDesc ?? "Unknown Description";
            string productQuantity = selectedProduct.ProductQuantity ?? "Unknown Quantity";
            string image1 = selectedProduct.image1 ?? string.Empty;
            string image2 = selectedProduct.image2 ?? string.Empty;
            string image3 = selectedProduct.image3 ?? string.Empty;
            string image4 = selectedProduct.image4 ?? string.Empty;
            string image5 = selectedProduct.image5 ?? string.Empty;
            string image6 = selectedProduct.image6 ?? string.Empty;

            await Navigation.PushModalAsync(new ViewProductPage(email, productName, productPrice, productDescriptions, productQuantity, image1, image2, image3, image4, image5, image6));
        }
    }

    private async void SettingsBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SellerSettings(App.key));
    }

    private async void InitializeAsync(string userKey)
    {
        try
        {
            string userEmail = App.email;

            // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
            string obtainedUserKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            UserKey = obtainedUserKey;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (log, display, etc.)
            Console.WriteLine($"Error in AppShell initialization: {ex.Message}");
        }
    }
}


