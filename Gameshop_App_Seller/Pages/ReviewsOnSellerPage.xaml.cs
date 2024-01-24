using Firebase.Auth;
using Gameshop_App_Seller.Models;
using System.Reflection;

namespace Gameshop_App_Seller.Pages
{
    public partial class ReviewsOnSellerPage : ContentPage
    {
        private Users reviewseller = new();
        private string userEMAIL;
        private List<Users> userReviews;


        public ReviewsOnSellerPage()
        {
            InitializeComponent();
            OnAppearingReview();
            OnAppearing();
        }

        public ReviewsOnSellerPage(string userkey) : this()
        {
            App.key = userkey;
        }

        protected async Task OnAppearingReview()
        {
            try
            {
                base.OnAppearing();

                string userEmail = App.email.ToLower();

                var deniedApplicationsList = await reviewseller.GetReviewListAsync();

                // Filter the list to include only reviews with the user's email
                userReviews = deniedApplicationsList
     .Where(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase))
     .ToList();

                if (userReviews.Any())
                {
                    reviewontheSeller.ItemsSource = userReviews;

                    // Set the star rating for each review
                    foreach (var review in userReviews)
                    {
                        SetStarRating(review.StarReview);
                    }
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


        private void SetStarRating(string starReview)
        {
            // Map your starReview values to a numerical rating (customize based on your requirements)
            // Set the star images based on the numerical rating for each user
            foreach (var user in userReviews)
            {
                int numberOfStars = 0;

                // Map your starReview values to a numerical rating (customize based on your requirements)
                switch (user.StarReview)
                {
                    case "Excellent":
                        numberOfStars = 5;
                        break;
                    case "Good":
                        numberOfStars = 4;
                        break;
                    case "Average":
                        numberOfStars = 3;
                        break;
                    case "Poor":
                        numberOfStars = 2;
                        break;
                    case "Very Poor":
                        numberOfStars = 1;
                        break;
                    // Add cases for other possible values

                    default:
                        numberOfStars = 0; // Default to 0 stars if no match is found
                        break;
                }

                for (int i = 1; i <= 5; i++)
                {
                    string starSource = i <= numberOfStars ? "filledstar.png" : "unfilledstar.png";

                    // Assuming user is the current item in your data template
                    PropertyInfo propertyInfo = typeof(Users).GetProperty($"Star{i}Source");
                    propertyInfo?.SetValue(user, starSource);
                }
            }

            // Refresh the CollectionView to update the UI
            reviewontheSeller.ItemsSource = null;
            reviewontheSeller.ItemsSource = userReviews;


        }


        private async void btnBackImg_Clicked(object sender, EventArgs e)
        {
           
            await Navigation.PopModalAsync();
            
        }
    }
}
