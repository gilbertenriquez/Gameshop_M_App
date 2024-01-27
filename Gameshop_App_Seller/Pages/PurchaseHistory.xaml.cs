using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class PurchaseHistory : ContentPage
{
    private Users history = new Users();
	public PurchaseHistory()
	{
		InitializeComponent();
        OnAppearingPurchase();

    }


    protected async Task OnAppearingPurchase()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var deniedApplicationsList = await history.GetPurchaseListAsync();

            // Print the type of items in the list to the console
            Console.WriteLine($"Type of items in the list: {deniedApplicationsList.FirstOrDefault()?.GetType()?.Name}");

            // Filter the list to include only reviews with the user's email
            var userReviews = deniedApplicationsList.Where(app => app.Seller.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userReviews.Any())
            {
                purchaselist.ItemsSource = userReviews;
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
                    var selectedProduct = button.CommandParameter as Users; // Replace Users with your actual type

                    if (selectedProduct != null)
                    {
                        var itemPicture = selectedProduct.soldImageItem;
                        var img2 = selectedProduct.soldTranscationImage;
                        var soldName = selectedProduct.soldName;
                        var soldQuantity = selectedProduct.soldQuantity;
                        var soldPrice = selectedProduct.soldPrice;
                        var soldTime = selectedProduct.soldTime;
                        var soldDate = selectedProduct.soldDate;
                        var soldToSeller = selectedProduct.Seller;
                        var soldToBuyer = selectedProduct.Buyer;

                        // Example: Call your confirmSold method with the retrieved data
                        var result = await history.confirmSold(itemPicture, img2, soldName, soldQuantity, soldPrice, soldTime, soldDate, soldToSeller, soldToBuyer);

                        if (result)
                        {
                            // Handle successful confirmation
                            await DisplayAlert("Information", "Transaction confirmed successfully", "OK");
                        }
                        else
                        {
                            // Handle confirmation failure
                            await DisplayAlert("Error", "Transaction confirmation failed", "OK");
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
}