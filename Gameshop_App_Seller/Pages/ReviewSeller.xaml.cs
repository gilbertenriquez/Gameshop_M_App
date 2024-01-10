using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Gameshop_App_Seller.Pages
{
    public partial class ReviewSeller : ContentPage
    {
        private List<ImageButton> starButtons;

        public ReviewSeller()
        {
            InitializeComponent();
            InitializeStarButtons();
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

    }
}
