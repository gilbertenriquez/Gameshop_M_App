using static Gameshop_App_Seller.App;
    using Gameshop_App_Seller.Models;


namespace Gameshop_App_Seller.Pages;

public partial class SellerReviews : ContentPage
{
    private string userEmails;
    private Users ReviewsSeller = new();
    public SellerReviews()
    {
        InitializeComponent();


    }

    public SellerReviews(string email) : this()
    {
        this.userEmails = email;
        OnAppearingReview();
    }

    protected async Task OnAppearingReview()
    {
        try
        {
            base.OnAppearing();

            string userEmail = userEmails.ToLower();

            var deniedApplicationsList = await ReviewsSeller.GetReviewListAsync();

            // Filter the list to include only reviews with the user's email
            var userReviews = deniedApplicationsList.Where(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

            if (userReviews.Any())
            {
                sellerreviews.ItemsSource = userReviews;

            }
            else
            {
                await DisplayAlert("Information!", "No Item Reviews", "OK");
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