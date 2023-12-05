using Gameshop_App_Seller.Models;
using System.Collections.ObjectModel;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class BuyerHomePage : ContentPage
{
    private Users vans = new Users();
    public ObservableCollection<Users.Product> ProductList { get; set; }

    public BuyerHomePage()
    {
        InitializeComponent();
        ProductList = new ObservableCollection<Users.Product>();
        datalist.ItemsSource = ProductList; // Set the ItemsSource here
        LoadProductList();
    }

    private async void LoadProductList()
    {
        try
        {
            var productList = await vans.GetAllProductList();

            if (productList != null && productList.Count > 0)
            {
                // Clear existing items before adding new ones
                ProductList.Clear();

                foreach (var product in productList)
                {
                    // Add the product to your ObservableCollection
                    ProductList.Add(product);

                    // Debug information
                    Console.WriteLine($"Added product: {product.ProductName}");
                }
            }
            else
            {
                Console.WriteLine("No products found.");
            }
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"Exception during LoadProductList: {ex}");
        }
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