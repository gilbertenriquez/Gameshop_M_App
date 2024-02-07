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


            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Error", "No internet connection. Please check your network settings.", "OK");
                return;
            }

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

        if (result == null)
            return;

        FileInfo fi = new(result.FullPath);
        var sizeInBytes = fi.Length;
        var sizeInMegabytes = sizeInBytes / (1024 * 1024); // Convert bytes to megabytes

        const int maxSizeInMegabytes = 50; // Maximum allowed file size in megabytes

        if (sizeInMegabytes > maxSizeInMegabytes)
        {
            // Display a message indicating that the selected image is too large
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromRgb(32, 32, 40),
                TextColor = Colors.WhiteSmoke,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(10),
                ActionButtonFont = Font.SystemFontOfSize(10)
            };
            const string text = "The image you have selected is more than 50MB. Please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }

        var stream = await result.OpenReadAsync();
        App._ValidIDFront = result;
        Frontimage.Source = ImageSource.FromStream(() => stream);


    }


    private async void backimage_Clicked(object sender, EventArgs e)
    {

        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select main image",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
            return;

        FileInfo fi = new(result.FullPath);
        var sizeInBytes = fi.Length;
        var sizeInMegabytes = sizeInBytes / (1024 * 1024); // Convert bytes to megabytes

        const int maxSizeInMegabytes = 50; // Maximum allowed file size in megabytes

        if (sizeInMegabytes > maxSizeInMegabytes)
        {
            // Display a message indicating that the selected image is too large
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromRgb(32, 32, 40),
                TextColor = Colors.WhiteSmoke,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(10),
                ActionButtonFont = Font.SystemFontOfSize(10)
            };
            const string text = "The image you have selected is more than 50MB. Please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }

        var stream = await result.OpenReadAsync();
        App._ValidIDBack = result;
        Backimage.Source = ImageSource.FromStream(() => stream);


    }
    private async void nextBack_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }


        if (IsDefaultImage(Frontimage.Source, "valid1.png"))
        {
            // Display alert for front ID image not uploaded
            await DisplayAlert("Error", "Please upload the front ID image.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        if (IsDefaultImage(Backimage.Source, "valid2.png"))
        {
            // Display alert for back ID image not uploaded
            await DisplayAlert("Error", "Please upload the back ID image.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        if (_ValidIDFront == null)
        {
            await DisplayAlert("Information", "Please upload the front ID image.", "OK");
            progressLoading.IsVisible = false;
            return;

        }


        if (_ValidIDBack == null)
        {
            await DisplayAlert("Information", "Please upload the back ID image.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        // Continue with navigation if both images are uploaded
        await Navigation.PushModalAsync(new VerifyingPage(UserKey));
        progressLoading.IsVisible = false;
    }

    private bool IsDefaultImage(ImageSource source, string defaultImagePath)
    {
        if (source == null)
        {
            return true; // Treat null as a default image
        }

        if (source is FileImageSource fileImageSource)
        {
            return string.IsNullOrEmpty(fileImageSource.File) || fileImageSource.File == defaultImagePath;
        }

        return false;
    }




    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
        progressLoading.IsVisible = false;
    }

    private  void Button_Clicked(object sender, EventArgs e)
    {
        Frontimage.Source = null;
       
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Backimage.Source = null;
    }
}