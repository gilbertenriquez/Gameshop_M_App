using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class UserPurchaseHistory : ContentPage
{
    private Users GetUserPurchaseHistory = new Users();
	public UserPurchaseHistory()
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

            var deniedApplicationsList = await GetUserPurchaseHistory.GetUserPurchaseListAsync();

            // Filter the list to include only reviews with the user's email
            var userReviews = deniedApplicationsList.Where(app => app.Buyer.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userReviews.Any())
            {
                UserPurchasehistory.ItemsSource = userReviews;
            }
            else
            {
                await DisplayAlert("Information!", "No item has been purchased recently.", "OK");
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