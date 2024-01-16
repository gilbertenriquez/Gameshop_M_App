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
                else{
                    await DisplayAlert("Information!", "No Item Reviews", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnAppearingDenied: {ex.Message}");
            }
        }


        //private void SetStarRatingImages(string starReview)
        //{
            
        //    // Set the appropriate stars based on the rating
        //    switch (starReview.ToLower())
        //    {
        //        case "excellent":
        //            SetStarImage(stars1, "filledstar.png");
        //            SetStarImage(stars2, "filledstar.png");
        //            SetStarImage(stars3, "filledstar.png");
        //            SetStarImage(stars4, "filledstar.png");
        //            SetStarImage(stars5, "filledstar.png");
        //            break;
        //        case "good":
        //            SetStarImage(stars1, "filledstar.png");
        //            SetStarImage(stars2, "filledstar.png");
        //            SetStarImage(stars3, "filledstar.png");
        //            SetStarImage(stars4, "filledstar.png");
        //            break;
        //        case "average":
        //            SetStarImage(stars1, "filledstar.png");
        //            SetStarImage(stars2, "filledstar.png");
        //            SetStarImage(stars3, "filledstar.png");
        //            break;
        //        case "poor":
        //            SetStarImage(stars1, "filledstar.png");
        //            SetStarImage(stars2, "filledstar.png");
        //            break;
        //        case "very poor":
        //            SetStarImage(stars1, "filledstar.png");
        //            break;
        //            // Add cases for other ratings as needed
        //    }
        //}

        private void SetStarImage(Image star, string imageName)
        {
            star.Source = imageName;
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
