using Gameshop_App_Seller.Models;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Pages;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class addproductPage : ContentPage
{
    FileResult _mainimage;

    CancellationTokenSource cancellationTokenSource = new();
    Users uploads = new();

    public addproductPage()
    {
      
        InitializeComponent();
    }
 

    private async void addimageBTN_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select main image",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null) return;

        FileInfo fi = new(result.FullPath);
        var sizeLimitInBytes = 50 * 1024 * 1024; // 50MB
        var size = fi.Length;

        if (size > sizeLimitInBytes)
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

            const string text = "The image you have selected is larger than 50MB. Please ensure that the size of the image is less than 50MB. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }

        var stream = await result.OpenReadAsync();
        App._mainimgResult = result;
        mainimage.Source = ImageSource.FromStream(() => stream);

        progressLoading.IsVisible = false;


    }
   

    private async void addsupportimgBTN_Clicked(object sender, EventArgs e)
    {
        var ctr = 0;
        var results = await FilePicker.PickMultipleAsync();
        foreach (var result in results)
        {
            progressLoading.IsVisible = true;
            ctr++;
            switch (ctr)
            {
                //first image
                case 1:
                    {
                        _img1Result = result;
                        var stream = await result.OpenReadAsync();
                        img1.Source = ImageSource.FromStream(() => stream);
                        break;
                    }
                case 2:
                    {
                        _img2Result = result;
                        var stream = await result.OpenReadAsync();
                        img2.Source = ImageSource.FromStream(() => stream);
                        break;
                    }
                case 3:
                    {
                        _img3Result = result;
                        var stream = await result.OpenReadAsync();
                        img3.Source = ImageSource.FromStream(() => stream);
                        break;
                    }
                case 4:
                    {
                        _img4Result = result;
                        var stream = await result.OpenReadAsync();
                        img4.Source = ImageSource.FromStream(() => stream);
                        break;
                    }
                case 5:
                    {
                        _img5Result = result;
                        var stream = await result.OpenReadAsync();
                        img5.Source = ImageSource.FromStream(() => stream);
                        break;
                    }
                case 6:
                    {
                        _img6Result = result;
                        var stream = await result.OpenReadAsync();
                        img6.Source = ImageSource.FromStream(() => stream);
                        break;
                    }

            }
        }
        progressLoading.IsVisible = false;
    }


    private async void NEXTBTN_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        // Check if any of the images is empty or null
        if (mainimage.Source == null
            || img1.Source == null
            || img2.Source == null
            || img3.Source == null
            || img4.Source == null
            || img5.Source == null
            || img6.Source == null)
        {
            await DisplayAlert("Warning", "Fill Up All Fields", "OK");
            return;
        }

        // Proceed with navigation if all images are provided
        await Navigation.PushModalAsync(new AddproductinfoPage());
    }


    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        mainimage.Source = null;
    }
}