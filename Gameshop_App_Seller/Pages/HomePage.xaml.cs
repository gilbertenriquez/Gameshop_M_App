using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;
using static System.Net.Mime.MediaTypeNames;


namespace Gameshop_App_Seller.Pages;

public partial class HomePage : ContentPage
{
    private Users dusers = new Users();
    public HomePage()
    {
        LoadUserDataAsync();
        InitializeComponent();
        OnAppearing();
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
                if (string.IsNullOrEmpty(userSnapshot.ShopProfile)
                    || string.IsNullOrEmpty(userSnapshot.ShopCoverImg)
                    || string.IsNullOrEmpty(userSnapshot.ShopName)
                    || string.IsNullOrEmpty(userSnapshot.ShopContactNumber)
                    || string.IsNullOrEmpty(userSnapshot.ShopMessengerLink))
                {
                    // If any of the properties is empty or null, show an alert or take appropriate action
                    await DisplayAlert("Error", "Shop details are incomplete. Please update your shop details.", "OK");
                    await Navigation.PushModalAsync(new ShopDetail(App.key));
                    return;
                }
                else
                {
                    // Update the UI with the retrieved user data
                    imglogo.Source = new UriImageSource
                    {
                        Uri = new Uri(userSnapshot.ShopProfile),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    };

                    imglogo.Source = userSnapshot.ShopProfile;
                    lblcompanyname.Text = userSnapshot.ShopName;
                    imgcover.Source = userSnapshot.ShopCoverImg;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during data retrieval
            Console.WriteLine($"Error: {ex.Message}");
        }
    }




    protected override async void OnAppearing()
    {
        try
        {
            // Obtain the user email from your source; replace this with your actual logic
            string userEmail = App.email;

            // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
            string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            if (!string.IsNullOrEmpty(userKey))
            {
                // Retrieve and display the user's products
                var userProducts = await dusers.GetUserProducts(userKey);

                if (userProducts.Any())
                {
                    listViewProducts.ItemsSource = userProducts;
                }
                else
                {
                    await DisplayAlert("Warning", "User has no products.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Warning", "User key not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Log or display the exception for debugging
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void AddProdsBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new addproductPage());
    }

    private async void EditProdsBTN_Clicked(object sender, EventArgs e)
    {
        if (listViewProducts.SelectedItem != null)
        {
            var selectedUser = listViewProducts.SelectedItem as Users;

            if (selectedUser != null)
            {

                await Navigation.PushModalAsync(new EditProductPage(
                    selectedUser.ProductName,
                    selectedUser.ProductDesc,
                    selectedUser.ProductPrice,
                    selectedUser.ProductQuantity,
                    selectedUser.Imagae_1_link,
                    selectedUser.image1,
                    selectedUser.image2,
                    selectedUser.image3,
                    selectedUser.image4,
                    selectedUser.image5,
                    selectedUser.image6,
                    selectedUser.ProductPath,
                    selectedUser.MAIL));
            }
        }
        else
        {
            await DisplayAlert("Alert", "Please Select A Data To Edit", "OK!");
        }
    }


    private async void listproducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            var selectedUser = e.CurrentSelection.FirstOrDefault() as Users;

            if (selectedUser != null)
            {
                // Set App.email based on the selected user's email
                App.productname = selectedUser.ProductName.ToLower();

                // Retrieve the user key based on the email
                App.key = await dusers.GetUserKey(App.productname);

                // Fetch additional user data, including ReporterEmail
                selectedUser = await dusers.GetUserDataByEmailAsync(App.email);

                // Load user data to update UI

            }
            else
            {
                // Reset App.key when selection is cleared
                App.key = null;
            }
        }
    }

   

    private async void sellerReview_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ReviewsOnSellerPage());
    }

    private async void sellerHistory_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new PurchaseHistory());
    }

    private async void sellerSettings_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SettingsPage());
    }
}