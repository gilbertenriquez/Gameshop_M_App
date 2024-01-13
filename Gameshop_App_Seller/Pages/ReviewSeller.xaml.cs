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
            if (!CheckInternetConnection())
            {
                // Optionally display an alert or take appropriate action if there's no internet
                return;
            }


            InitializeComponent();
            InitializeStarButtons();
           
        }

        public ReviewSeller(string email) : this()
        {
            this.userEmails = email;
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
            wordCountLabel.Text = $"{charCount}/600 characters";

            if (charCount > 600)
            {
                // Disable further input
                ((Editor)sender).IsEnabled = false;

                // Display an alert message
                DisplayAlert("Character Limit Exceeded", "You have reached the maximum limit of 600 characters.", "OK");
            }
        }

        private async void BTNupload_Clicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select main image",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                return;

            FileInfo fi = new(result.FullPath);
            var size = fi.Length;

            // Set the maximum allowed size to 50MB (50 * 1024 * 1024 bytes)
            var maxSize = 50 * 1024 * 1024;

            if (size > maxSize)
            {
                var snackbarOptions = new SnackbarOptions
                {
                    BackgroundColor = Color.FromRgb(32, 32, 40),
                    TextColor = Colors.WhiteSmoke,
                    ActionButtonTextColor = Colors.White,
                    CornerRadius = new CornerRadius(10),
                    Font = Font.SystemFontOfSize(10),
                    ActionButtonFont = Font.SystemFontOfSize(10)
                };
                const string text = "The image you have selected is more than 50MB. Please ensure that the size of the image is less than the maximum size (50MB). Thank you!";
                const string actionButtonText = "Got it!";
                var duration = TimeSpan.FromSeconds(10);
                var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

                await snackbar.Show(cancellationTokenSource.Token);
                return;
            }

            var stream = await result.OpenReadAsync();
            App._mainimgResult = result;
            photoITEM.Source = ImageSource.FromStream(() => stream);
        }

        private void BTNremove_Clicked(object sender, EventArgs e)
        {
            photoITEM.Source = null;
        }

       private async void BTNsubmit_Clicked(object sender, EventArgs e)
{
    var result = await ReviewUser.UploadReview(_mainimgResult, ratingLabel.Text, commentTXT.Text, userEmails);

    if (result)
    {
        await DisplayAlert("Success", "Review submitted successfully!", "OK");
    }
    else
    {
        await DisplayAlert("Error", "Failed to submit review. Please try again.", "OK");
    }
}


        private async void btnBackImg_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
    
}
