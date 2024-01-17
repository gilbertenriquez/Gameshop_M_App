using Gameshop_App_Seller.Models;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using static Gameshop_App_Seller.App;
using Firebase.Auth;

namespace Gameshop_App_Seller.Pages;

public partial class AddproductinfoPage : ContentPage
{
    private Users imgSave = new();
    public AddproductinfoPage()
    {
        InitializeComponent();
    }

  


    private async void exit_Button_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PushModalAsync(new HomePage());
        progressLoading.IsVisible = false;
    }

    private async void saveBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;


        var a = await imgSave.Save(_mainimgResult,
                           _img1Result,
                           _img2Result,
                           _img3Result,
                           _img4Result,
                           _img5Result,
                           _img6Result,
                           entryName.Text,
                           entryDescription.Text,
                           entryPrice.Text,
                           entryQuantity.Text,
                           email);

        string userEmail = imgSave.MAIL;
        var userKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);



        if (string.IsNullOrEmpty(userKey))
        {
            await DisplayAlert("Message", "Product Successfully Added", "OK");
            await Navigation.PushModalAsync(new HomePage());
            progressLoading.IsVisible = false;
            return;
        }
        else
        {
            await DisplayAlert("Message", "Product Not Successfully Added", "OK");
            progressLoading.IsVisible = false;
            return;
        }
        
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PopModalAsync();
        progressLoading.IsVisible = false;
    }

    private void entryPrice_TextChanged(object sender, TextChangedEventArgs e)
    {

        if (sender is Entry entry)
        {
            // Remove non-numeric characters
            string cleanedText = new string(entry.Text.Where(c => Char.IsDigit(c) || c == '.').ToArray());

            // Convert the cleaned text to a numeric value
            if (double.TryParse(cleanedText, out double numericValue))
            {
                // Format the numeric value as currency
                entry.Text = string.Format("{0:C}", numericValue);
            }
            else
            {
                // Handle invalid input (e.g., non-numeric characters)
                entry.Text = string.Empty;
            }
        }
    }
}