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
        InitializeComponent();


    }


    public HomePage(string userkey) : this()
    {
        InitializeComponent();
        InitializeAsync(userkey);
        OnAppearing();
        LoadUserDataAsync();


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
            Console.WriteLine($"Error in App initialization: {ex.Message}");
        }
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
                    selectedUser.image6));
            }
        }
        else
        {
            await DisplayAlert("Alert", "Please Select A Data To Edit", "OK!");
        }
    }
    private async void ChatBTN_Clicked(object sender, EventArgs e)
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
                //await Navigation.PushModalAsync(new ChatHomepage(userKey));
            }
        }
        else
        {
            await DisplayAlert("Warning", "No user key found", "OK");
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

    private async void reportBTN_Clicked(object sender, EventArgs e)
    {
        bool userConfirmed = await DisplayAlert("ALERT", "Report This Product", "Yes", "No");

        if (userConfirmed)
        {
            await Navigation.PushModalAsync(new ReportPage());
        }
        else
        {
            return;
        }
    }

}