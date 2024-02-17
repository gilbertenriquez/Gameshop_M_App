using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;



namespace Gameshop_App_Seller.Pages;

public partial class HomePage : ContentPage
{
    private Users dusers = new Users();
    private object lastSelectedItem;
    private string userkey;

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
                    await DisplayAlert("Information", "Shop details are incomplete. Please update your shop details.", "OK");
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
            userkey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            if (!string.IsNullOrEmpty(userkey))
            {
                // Retrieve and display the user's products
                var userProducts = await dusers.GetUserProducts(userkey);

                if (userProducts.Any())
                {
                    listViewProducts.ItemsSource = userProducts;
                    return;
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
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }
        await Navigation.PushModalAsync(new addproductPage());
    }

    private async void EditProdsBTN_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

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
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        if (e.CurrentSelection != null)
        {
            var selectedUser = e.CurrentSelection.FirstOrDefault() as Users;

            if (selectedUser != null)
            {
                // Set App.email based on the selected user's email
                App.productname = selectedUser.ProductName.ToLower();

                // Retrieve the user key based on the email
                App.UserKey = await dusers.GetUserKey(App.productname);

                // Fetch additional user data, including ReporterEmail
                selectedUser = await dusers.GetUserDataByEmailAsync(App.email);

                // Load user data to update UI

                // Update lastSelectedItem
                lastSelectedItem = selectedUser;
            }
            else
            {
                // Reset App.key when selection is cleared
                App.UserKey = null;
            }
        }
    }

    private async void sellerReview_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        await Navigation.PushModalAsync(new ReviewsOnSellerPage());
    }

    private async void sellerHistory_Clicked(object sender, EventArgs e)
    {

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }
        await Navigation.PushModalAsync(new PurchaseHistory());
    }

    private async void sellerSettings_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }
        await Navigation.PushModalAsync(new SettingsPage(App.key));
    }

    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await LoadUserDataAsync();
            OnAppearing();

            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }

    private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        string userEmail = App.email;
        string userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);
        string searchText = e.NewTextValue;

        // Use the GetUserProducts method to retrieve the full list of user products
        var allUserProducts = await dusers.SearchUserProducts(userKey); // Pass the appropriate user key here

        if (string.IsNullOrWhiteSpace(searchText))
        {
            // If the search text is null or empty, reset the datalist to show all items
            listViewProducts.ItemsSource = allUserProducts;
        }
        else
        {
            // Filter items based on the search text or numeric value
            var filteredItems = allUserProducts
                .Where(product =>
                    product.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    product.ProductPrice.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    IsNumericMatch(product.ProductPrice, searchText))
                .ToList();

            // Update the datalist with the filtered items
            listViewProducts.ItemsSource = new ObservableCollection<Users>(filteredItems);
        }
    }

    // Helper method to check if a string is numeric and matches the search text
    private bool IsNumericMatch(string input, string searchText)
    {
        // Remove currency symbol ("₱") and commas, then check if it contains the search text
        string cleanedInput = input.Replace("₱", "").Replace(",", "");

        if (double.TryParse(cleanedInput, out double numericValue))
        {
            return numericValue.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    private async void PreviewTheItem_Tapped(object sender, TappedEventArgs e)
    {

        var selectedProduct = e.Parameter as Users; // Replace Users with your actual type

        if (selectedProduct != null)
        {
            //App.productname = selectedProduct.ProductName?.ToLower();
            //App.productprice = await vans.GetUserKey(App.productname);

            // Check for null or empty values before passing them to the constructor
            string productemail = selectedProduct.MAIL ?? string.Empty;
            string productName = selectedProduct.ProductName ?? "Unknown Product";
            string productPrice = selectedProduct.ProductPrice ?? "Unknown Price";
            string productDescriptions = selectedProduct.ProductDesc ?? "Unknown Description";
            string productQuantity = selectedProduct.ProductQuantity ?? "Unknown Quantity";
            string mainImage = selectedProduct.Imagae_1_link ?? string.Empty;
            string image1 = selectedProduct.image1 ?? string.Empty;
            string image2 = selectedProduct.image2 ?? string.Empty;
            string image3 = selectedProduct.image3 ?? string.Empty;
            string image4 = selectedProduct.image4 ?? string.Empty;
            string image5 = selectedProduct.image5 ?? string.Empty;
            string image6 = selectedProduct.image6 ?? string.Empty;

            await Navigation.PushModalAsync(new PreviewItem(productemail, productName, productPrice, productDescriptions, productQuantity, mainImage, image1, image2, image3, image4, image5, image6));
        }
    }
}