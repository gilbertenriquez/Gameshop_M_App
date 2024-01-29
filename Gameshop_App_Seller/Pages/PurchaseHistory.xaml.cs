using Firebase.Auth;
using Firebase.Database.Query;
using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class PurchaseHistory : ContentPage
{
    private Users history = new Users();
    public PurchaseHistory()
    {
        InitializeComponent();
        OnAppearingPurchase();

    }

    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            // Perform the data refreshing logic here
            await OnAppearingPurchase();

            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }


    protected async Task OnAppearingPurchase()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var deniedApplicationsList = await history.GetPurchaseListAsync();

            if (deniedApplicationsList.Any())
            {
                foreach (var application in deniedApplicationsList)
                {
                    application.IsConfirmationButtonVisible = !application.IsConfirmed;
                }

                purchaselist.ItemsSource = deniedApplicationsList;
                // Count the number of confirmed items
                int confirmedCount = deniedApplicationsList.Count(item => item.IsConfirmed);

                // Display the count in your Label
                SoldItem.Text = $"Items Sold: {confirmedCount}";
            }
            else
            {
                await DisplayAlert("Information!", "No Item has been Sold", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAppearingDenied: {ex.Message}");
        }
    }








    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {

        await Navigation.PopModalAsync();

    }

    private async void SellerConfirmation_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool answer = await DisplayAlert("Confirmation", "Are you sure you want to proceed?", "Yes", "No");

            if (answer)
            {
                if (sender is Button button)
                {
                    var selectedItem = button.CommandParameter as Users; // Replace Users with your actual type

                    if (selectedItem != null)
                    {
                        // Access the unique identifier (ItemId) associated with the selected item


                        // Check if the transaction is already confirmed
                        if (!selectedItem.IsConfirmed)
                        {
                            var itemPicture = selectedItem.soldImageItem;
                            var img2 = selectedItem.soldTranscationImage;
                            var soldName = selectedItem.soldName;
                            var soldQuantity = selectedItem.soldQuantity;
                            var soldPrice = selectedItem.soldPrice;
                            var soldTime = selectedItem.soldTime;
                            var soldDate = selectedItem.soldDate;
                            var soldToSeller = selectedItem.Seller;
                            var soldToBuyer = selectedItem.Buyer;

                            // Your existing logic for confirmation
                            var Key = await GetUserKeyByDetails(soldToBuyer, soldName, soldQuantity, soldPrice, soldToSeller);

                            var sellersKey = await App.FirebaseService.GetUserKeyByEmail(soldToSeller);
                            var sellerQuantityUpdate = await GetProductKeyByEmail(sellersKey, soldName);
                            var Productkey = sellerQuantityUpdate;

                            var quantityPath = await history.GetQuantityAsync(sellersKey, Productkey);
                            var checkQuantity = quantityPath.FirstOrDefault(app => app.ProductQuantity.Equals(soldQuantity, StringComparison.OrdinalIgnoreCase));
                            if (quantityPath != null && quantityPath.Any(app => app.ProductQuantity == "0"))
                            {
                                // Display an alert for out of stock
                                await DisplayAlert("Out of Stock", "The Item is out of stock or the quantity is not available.", "OK");
                                return;
                            }


                            var result = await history.confirmSold(itemPicture, img2, soldName, soldQuantity, soldPrice, soldTime, soldDate, soldToSeller, soldToBuyer);

                            if (result)
                            {
                                var b = await history.SubtractQuantityFromProduct(soldQuantity, sellersKey, Productkey);
                                var a = await history.UpdateConfirmationStatus(Key, true);
                                await DisplayAlert("Information", "Transaction confirmed successfully", "OK");
                                selectedItem.IsConfirmed = true;
                            }
                            else
                            {
                                // Handle confirmation failure
                                await DisplayAlert("Error", "Transaction confirmation failed", "OK");
                            }
                        }
                        else
                        {
                            // Transaction already confirmed, show a message or handle accordingly
                            await DisplayAlert("Information", "Transaction already confirmed", "OK");
                        }
                    }
                    else
                    {
                        // Handle the case when CommandParameter is not of type Users
                        await DisplayAlert("Error", "Invalid selection", "OK");
                    }
                }
            }
            else
            {
                // User clicked "No", handle accordingly or do nothing
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SellerConfirmation_Clicked: {ex.Message}");
        }
    }

    public async Task<string> GetUserKeyByDetails(string buyer, string soldName, string soldQuantity, string soldPrice, string seller)
    {
        var users = await ClientUsers
            .Child("Purchase History")
            .OnceAsync<Users>();

        var userWithKey = users.FirstOrDefault(u =>
            u.Object.Buyer == buyer &&
            u.Object.soldName == soldName &&
            u.Object.soldQuantity == soldQuantity &&
            u.Object.soldPrice == soldPrice &&
            u.Object.Seller == seller);

        return userWithKey?.Key;
    }

    public async Task<string> GetProductKeyByEmail(string userkey, string productName)
    {
        var products = await ClientUsers
            .Child($"Account/{userkey}/Product")
            .OnceAsync<Users>();

        var userWithKey = products.FirstOrDefault(u => u.Object.ProductName == productName);

        return userWithKey?.Key;
    }

}

