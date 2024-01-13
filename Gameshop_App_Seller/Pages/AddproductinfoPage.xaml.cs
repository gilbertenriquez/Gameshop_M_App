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

        if (!CheckInternetConnection())
        {
            // Optionally display an alert or take appropriate action if there's no internet
            return;
        }
        InitializeComponent();
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
            await Navigation.PushModalAsync(new AppShell(App.key));
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