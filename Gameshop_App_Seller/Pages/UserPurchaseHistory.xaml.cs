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



    protected async Task OnAppearingPurchase()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var deniedApplicationsList = await GetUserPurchaseHistory.GetPurchaseListAsync();

            // Filter the list to include only reviews with the user's email
            var userReviews = deniedApplicationsList.Where(app => app.Buyer.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userReviews.Any())
            {
                UserPurchasehistory.ItemsSource = userReviews;
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
}