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
        OnAppearingDenied();
        OnAppearingVerified();
        LoadDataAsync();
        OnAppearing();
        

    }
    public BuyerHomePage(string userKey) : this()
    {
        InitializeAsync(userKey);
    }


    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            // Perform the data refreshing logic here
            await LoadDataAsync();

            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }



    protected async void OnAppearingDenied()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var deniedApplicationsList = await vans.GetDeniedApplicationsListAsync();
            var deniedApplication = deniedApplicationsList.FirstOrDefault(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (deniedApplication != null)
            {
                await DisplayAlert("Denied Application", $"Denied Reason: {deniedApplication.DeniedReason}", "OK");

                // User is denied, show DeniedImage
                DeniedImage.IsVisible = true;
                VerifiedImage.IsVisible = false;
            }
            else
            {
                // User is no status, 
                VerifiedImage.IsVisible = false;
                DeniedImage.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAppearingDenied: {ex.Message}");
        }
    }


    protected async void OnAppearingVerified()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var VerifiedStats = await vans.GetVerifiedStatus();
            var VerifiedStatus = VerifiedStats.FirstOrDefault(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (VerifiedStatus != null)
            {               
                // User is verified, show DeniedImage
                DeniedImage.IsVisible = false;
                VerifiedImage.IsVisible = true;
            }
            else
            {
                // User is no status, 
                VerifiedImage.IsVisible = false;
                DeniedImage.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAppearingDenied: {ex.Message}");
        }
    }






    private async Task LoadDataAsync()
    {
        var userProductLists = await vans.GetUserProductListsAsync();

        // Assuming 'datalist' is your CollectionView in XAML
        datalist.ItemsSource = userProductLists;
    }

    protected override async void OnAppearing()
    {
        try
        {
            // Call InitializeAsync and await the result
            string userKey = await InitializeAsync(UserKey);

            // Check if App.key is null or empty
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("App.key is null or empty. Unable to proceed.");
                // Handle this case as needed, e.g., show an error message to the user
                return;
            }

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



    //private async void reportBTN_Clicked(object sender, EventArgs e)
    //{
    //    if (datalist.SelectedItem != null)
    //    {
    //        var selectedUser = datalist.SelectedItem as Users;

    //        if (selectedUser != null)
    //        {
    //            // Pass the necessary data to ReportPage, including reporter's email
    //            await Navigation.PushModalAsync(new ReportPage(selectedUser.ProductName, selectedUser.ProductPrice, selectedUser.MAIL, selectedUser.image1, selectedUser.ReporterEmail));
    //        }
    //    }
    //    else
    //    {
    //        await DisplayAlert("Alert", "Please Select A Data To Edit", "OK!");
    //    }
    //}


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





    //private async void datalist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (e.CurrentSelection != null)
    //    {
    //        var selectedUser = e.CurrentSelection.FirstOrDefault() as Users;

    //        if (selectedUser != null)
    //        {
    //            // Set App.email based on the selected user's email
    //            App.productname = selectedUser.ProductName.ToLower();

    //            // Retrieve the user key based on the email
    //            App.key = await vans.GetUserKey(App.productname);

    //            // Fetch additional user data, including ReporterEmail
    //            selectedUser = await vans.GetUserDataByEmailAsync(App.email);

    //            // Load user data to update UI

    //        }
    //        else
    //        {
    //            // Reset App.key when selection is cleared
    //            App.key = null;
    //        }
    //    }
    //}

    private async void ShopBTN_Clicked(object sender, EventArgs e)
    {

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        // Check if the user is logged in
        if (string.IsNullOrEmpty(App.email) || string.IsNullOrEmpty(App.key))
        {
            // Handle the case where user is not logged in
            await DisplayAlert("Error", "User is not logged in.", "OK");
            return;
        }

        // Get user email
        string userEmail = App.email;

        // Check if the user email is in the "Request" node
        bool isUserInRequest = await App.FirebaseService.IsUserEmailInRequestAsync(userEmail);

        bool isUserInVerified = await App.FirebaseService.IsUserEmailInVerifiedAsync(userEmail);

        // Set the user key in App  
        string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

        // Check if userKey is null or empty
        if (string.IsNullOrEmpty(userKey))
        {
            // Handle the case where userKey is null or empty
            await DisplayAlert("Error", "User key is null or empty.", "OK");
            return;
        }

        if (isUserInVerified)
        {
            // Display an alert indicating that the user is verified
            progressLoading.IsVisible = true;
            await Navigation.PushModalAsync(new HomePage());
            return;
        }

        if (isUserInRequest)
        {
            // Display an alert indicating that the user's email is in the Request list
            await DisplayAlert("Information", "Your email is in the Request list. Your application for becoming a seller is still in process. Please wait for approval.", "OK");
        }
        else
        {
            // Display an alert indicating that the user's email is not in the Request list
            await DisplayAlert("Information", "Please submit your verification ID's to access the Seller Mode", "Proceed");
            // Add debug statement
            await Navigation.PushModalAsync(new Valid_IDpage(userKey));
            return;
        }
    }

    private async void ViewProductBTN_Tapped_1(object sender, TappedEventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        var selectedProduct = e.Parameter as Users; // Replace Users with your actual type

        if (selectedProduct != null)
        {
            //App.productname = selectedProduct.ProductName?.ToLower();
            //App.productprice = await vans.GetUserKey(App.productname);

            // Check for null or empty values before passing them to the constructor
            string reporterMail = selectedProduct.ReporterEmail;
            string productemail = selectedProduct.MAIL ?? string.Empty;
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

            await Navigation.PushModalAsync(new ViewProductPage(App.key,reporterMail, productemail, productName, productPrice, productDescriptions, productQuantity, image1, image2, image3, image4, image5, image6));
        }
    }

    private async void SettingsBTN_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        await Navigation.PushModalAsync(new SellerSettings(App.key));
    }

    private async Task<string> InitializeAsync(string userKey)
    {
        try
        {
            string userEmail = App.email;

            // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
            string obtainedUserKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            // Update UserKey property
            UserKey = obtainedUserKey;

            // Update App.key if needed
            App.key = obtainedUserKey;

            // Return the obtainedUserKey
            return obtainedUserKey;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (log, display, etc.)
            Console.WriteLine($"Error in AppShell initialization: {ex.Message}");
            return null; // Return null or handle the error accordingly
        }
    }

    

    private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }
        string searchText = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            // If the search text is null or empty, reset the datalist to show all items
            datalist.ItemsSource = await vans.GetUserProductListsAsync();
        }
        else
        {
            // Filter items based on the search text
            var filteredItems = await vans.GetUserProductListsAsync(searchText);

            // Update the datalist with the filtered items
            datalist.ItemsSource = filteredItems;
        }
    }

   
}


//protected override void OnAppearing()
//{
//    base.OnAppearing();

//    // Check for internet connectivity
//    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
//    {
//        DisplayNoInternetAlert();
//        return;
//    }

//    try
//    {
//        // Your existing code for OnAppearing...
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Exception in OnAppearing: {ex.Message}");
//    }
//}

//private async void DisplayNoInternetAlert()
//{
//    await DisplayAlert("No Internet Connection", "Please check your internet connection and try again.", "OK");
//}

//// Other existing methods...

//// Ensure to handle connectivity changes
//protected override void OnDisappearing()
//{
//    base.OnDisappearing();

//    // Clear the selection in your datalist
//    datalist.SelectedItem = null;

//    // Unsubscribe from connectivity changes
//    Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
//}

//private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
//{
//    if (e.NetworkAccess == NetworkAccess.Internet)
//    {
//        // Handle the case where internet connection is re-established
//    }
//    else
//    {
//        // Handle the case where there is no internet connection
//        DisplayNoInternetAlert();
//    }
//}
