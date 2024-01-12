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
            this.userEMAIL = App.email;
            LoadReviewsAsync(userEMAIL);
        }

        public ReviewsOnSellerPage(string userkey) : this()
        {
            // Additional initialization if needed for the constructor with a user key
        }

        private async void LoadReviewsAsync(string userEmail)
        {
            var reviews = await reviewseller.GetReviewsByEmail(userEmail);

            // Display reviews in your UI, e.g., set the item source of your collection view
            reviewontheSeller.ItemsSource = reviews;
        }


    }

}
