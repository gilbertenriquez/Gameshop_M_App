using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace Gameshop_App_Seller.Pages
{
    public partial class ShopProduct : ContentPage
    {
        private Users Shopinfo = new Users();
        private string userkey;
        private string emailShop;

        public ObservableCollection<Users> UserProducts { get; set; }

        public ShopProduct(string email, string userKey)
        {
            InitializeComponent();
            this.userkey = userKey;
            this.emailShop = email;
            LoadUserDataAsync();
            OnBackButtonPressed();
        }

        protected override bool OnBackButtonPressed()
        {
            // Implement the logic to allow the back button press only on this page
            // Return true to indicate that the back button press has been handled
            return true;
        }

        private async Task LoadUserDataAsync()
        {
            try
            {
                var userData = userkey;
                var userAccountPath = $"Account/{userData}";

                // Retrieve the user data from the database
                var userSnapshot = await ClientUsers
                    .Child(userAccountPath)
                    .OnceSingleAsync<Users>();

                // Check if the user data exists
                if (userSnapshot != null)
                {
                    imglogo.Source = userSnapshot.ShopProfile;
                    lblcompanyname.Text = userSnapshot.ShopName;
                    imgcover.Source = userSnapshot.ShopCoverImg;

                    // Retrieve and display the user's products
                    UserProducts = new ObservableCollection<Users>(await Shopinfo.GetUserProducts(userkey));

                    listViewProducts.ItemsSource = UserProducts;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during data retrieval
                Console.WriteLine($"Error: {ex.Message}");
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
                listViewProducts.ItemsSource = UserProducts;
            }
            else
            {
                // Filter items based on the search text or numeric value
                var filteredItems = UserProducts
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

        private async void BuyItem_Tapped(object sender, TappedEventArgs e)
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
                string mainImage = selectedProduct.Imagae_1_link ?? string.Empty;
                string image1 = selectedProduct.image1 ?? string.Empty;
                string image2 = selectedProduct.image2 ?? string.Empty;
                string image3 = selectedProduct.image3 ?? string.Empty;
                string image4 = selectedProduct.image4 ?? string.Empty;
                string image5 = selectedProduct.image5 ?? string.Empty;
                string image6 = selectedProduct.image6 ?? string.Empty;

                await Navigation.PushModalAsync(new ViewProductPage(App.key, reporterMail, productemail, productName, productPrice, productDescriptions, productQuantity, mainImage, image1, image2, image3, image4, image5, image6));

            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}