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

            // Filter the list to include only reviews with the user's email
            var userReviews = deniedApplicationsList.Where(app => app.Seller.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userReviews.Any())
            {
                purchaselist.ItemsSource = userReviews;
            }
            else
            {
                await DisplayAlert("Information!","No Item has been Sold","OK");
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
}