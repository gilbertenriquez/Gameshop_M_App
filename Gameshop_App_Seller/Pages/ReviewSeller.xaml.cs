using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages
{
    public partial class ReviewSeller : ContentPage
    {
        CancellationTokenSource cancellationTokenSource = new();
        private List<ImageButton> starButtons;
        private Users ReviewUser = new Users();
        private string userEmails;
        public ReviewSeller() 
        {
         

            InitializeComponent();
            InitializeStarButtons();
           
        }

        public ReviewSeller(string email) : this()
        {
            this.userEmails = email;
        }



    

        private void InitializeStarButtons()
        {
            starButtons = new List<ImageButton> { star1, star2, star3, star4, star5 };

            foreach (var starButton in starButtons)
            {
                starButton.Clicked += StarButton_Clicked;
            }
        }

        private void StarButton_Clicked(object sender, EventArgs e)
        {
            var clickedStar = (ImageButton)sender;
            var clickedIndex = starButtons.IndexOf(clickedStar);

            // Update the source of the clicked star and previous stars
            for (int i = 0; i <= clickedIndex; i++)
            {
                starButtons[i].Source = "filledstar.png";
            }

            for (int i = clickedIndex + 1; i < starButtons.Count; i++)
            {
                starButtons[i].Source = "unfilledstar.png";
            }

            // Update the rating label based on the selected index
            ratingLabel.Text = GetRatingText(clickedIndex + 1);
        }

        private string GetRatingText(int rating)
        {
            switch (rating)
            {
                case 1:
                    return "Very Poor";
                case 2:
                    return "Poor";
                case 3:
                    return "Average";
                case 4:
                    return "Good";
                case 5:
                    return "Excellent";
                default:
                    return string.Empty;
            }
        }

        private void commentTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            int charCount = e.NewTextValue.Length;
            wordCountLabel.Text = $"{charCount}/100 characters";

            if (charCount > 100)
            {
                // Disable further input
                ((Editor)sender).IsEnabled = false;

                // Display an alert message
                DisplayAlert("Character Limit Exceeded", "You have reached the maximum limit of 100 characters.", "OK");
            }
        }

       

       private async void BTNsubmit_Clicked(object sender, EventArgs e)
{
            progressLoading.IsVisible = true;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
                return;
            }
            string username = await App.FirebaseService.GetUserFullNameByEmail(App.email);

            var TodayDate = DateTime.Now.ToString("MM-dd-yyyy");

            var result = await ReviewUser.UploadReview(ratingLabel.Text, commentTXT.Text, userEmails,App.email, TodayDate, username);

    if (result)
    {
        await DisplayAlert("Success", "Review submitted successfully!", "OK");
    }
    else
    {
        await DisplayAlert("Error", "Failed to submit review. Please try again.", "OK");
    }
            progressLoading.IsVisible = false;
        }


        private async void btnBackImg_Clicked(object sender, EventArgs e)
        {
            
            await Navigation.PopModalAsync();
          
        }
    }
    
}
