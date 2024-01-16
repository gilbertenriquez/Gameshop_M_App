using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages;

public partial class CustomerSeeSellerReview : ContentPage
{
    private Users seeReviewofSeller = new Users();
    private string Reviews;
	public CustomerSeeSellerReview()
	{
		InitializeComponent();
        
    }

    public CustomerSeeSellerReview(string sellerEMAIL): this() 
    {
        InitializeComponent();
        this.Reviews = sellerEMAIL;
        OnAppearingReview();    

    }

    protected async Task OnAppearingReview()
    {
        try
        {
            base.OnAppearing();

            string userEmail = Reviews.ToLower();

            var deniedApplicationsList = await seeReviewofSeller.GetReviewListAsync();

            // Filter the list to include only reviews with the user's email
            var userReviews = deniedApplicationsList.Where(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userReviews.Any())
            {
                sellerReviews.ItemsSource = userReviews;
            }
            else
            {
                await DisplayAlert("Information!", "No Reviews on the Seller", "OK");
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