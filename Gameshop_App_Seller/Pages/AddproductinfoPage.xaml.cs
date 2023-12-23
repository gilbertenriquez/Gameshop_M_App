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
        await Navigation.PushModalAsync(new HomePage());
    }

    private async void saveBTN_Clicked(object sender, EventArgs e)
    {


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
            return;
        }
        else
        {
            await DisplayAlert("Message", "Product Not Successfully Added", "OK");
            return;
        }
    
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}