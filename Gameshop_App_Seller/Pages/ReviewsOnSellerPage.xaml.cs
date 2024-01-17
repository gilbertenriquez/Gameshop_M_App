using Gameshop_App_Seller.Models;

namespace Gameshop_App_Seller.Pages
{
    public partial class ReviewsOnSellerPage : ContentPage
    {
        private Users reviewseller = new();
        private string userEMAIL;


        public ReviewsOnSellerPage()
        {
            InitializeComponent();
            OnAppearingReview();
            OnAppearing();
        }

        protected async Task OnAppearingReview()
        {
            try
            {
                base.OnAppearing();

                string userEmail = App.email.ToLower();

                var deniedApplicationsList = await reviewseller.GetReviewListAsync();

                // Filter the list to include only reviews with the user's email
                var userReviews = deniedApplicationsList.Where(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase)).ToList();

                if (userReviews.Any())
                {
                    reviewontheSeller.ItemsSource = userReviews;
    

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



        public ReviewsOnSellerPage(string userkey) : this()
        {
            App.key = userkey;
        }

        private async void btnBackImg_Clicked(object sender, EventArgs e)
        {
           
            await Navigation.PopModalAsync();
            
        }
    }
}
