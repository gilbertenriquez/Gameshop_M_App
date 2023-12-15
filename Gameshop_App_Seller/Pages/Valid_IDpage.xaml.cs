using Gameshop_App_Seller.Models;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Pages;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class Valid_IDpage : ContentPage
{
    FileResult _mainimage;
    CancellationTokenSource cancellationTokenSource = new();
    public Valid_IDpage()
    {
        InitializeComponent();
    }


    public Valid_IDpage(string userKey) : this()
    {
        InitializeAsync(userKey);
    }

    private async void InitializeAsync(string userKey)
    {
        try
        {
            string userEmail = App.email;

            // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
            string obtainedUserKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            UserKey = obtainedUserKey;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (log, display, etc.)
            Console.WriteLine($"Error in AppShell initialization: {ex.Message}");
        }
    }



    private async void btnFontimage_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select main image",
            FileTypes = FilePickerFileType.Images
        });
        if (result == null) return;

        FileInfo fi = new(result.FullPath);
        var size = fi.Length;

        if (size > 524288)
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
            const string text = "The image you have selected is more than 0.50MB please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }
        var stream = await result.OpenReadAsync();
        App._ValidIDFront = result;
        Frontimage.Source = ImageSource.FromStream(() => stream);


        progressLoading.IsVisible = false;
    }

    private async void backimage_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select main image",
            FileTypes = FilePickerFileType.Images
        });
        if (result == null) return;

        FileInfo fi = new(result.FullPath);
        var size = fi.Length;

        if (size > 524288)
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
            const string text = "The image you have selected is more than 0.50MB please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }
        var stream = await result.OpenReadAsync();
        App._ValidIDBack = result;
        Backimage.Source = ImageSource.FromStream(() => stream);


        progressLoading.IsVisible = false;
    }

    private async void nextBack_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PushModalAsync(new VerifyingPage(UserKey));
        progressLoading.IsVisible = false;
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
        progressLoading.IsVisible = false;
    }
}