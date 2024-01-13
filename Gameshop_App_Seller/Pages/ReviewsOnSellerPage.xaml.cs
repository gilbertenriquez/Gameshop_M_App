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

            if (!CheckInternetConnection())
            {
                // Optionally display an alert or take appropriate action if there's no internet
                return;
            }
        }

        public ReviewsOnSellerPage(string userkey) : this()
        {
           App.key = userkey;
        }

        private bool CheckInternetConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Error", "No internet connection. Please check your network settings.", "OK");
                return false;
            }
            return true;
        }

        private async void LoadReviewsAsync(string userEmail)
        {
            var reviews = await reviewseller.GetReviewsByEmail(userEmail);

            // Display reviews in your UI, e.g., set the item source of your collection view
            reviewontheSeller.ItemsSource = reviews;
        }


    }

}
