using Gameshop_App_Seller.Models;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Pages;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class VerifyingPage : ContentPage
{
    CancellationTokenSource cancellationTokenSource = new();
    private Users Valids = new();
    public VerifyingPage()
    {
        InitializeComponent();

    }

    public VerifyingPage(string userKey) : this()
    {
        InitializeAsync(userKey);

    }

    private async void InitializeAsync(string userKey)
    {
        try
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No internet connection.", "Please check your network settings.","OK");
                // Handle this case as needed, e.g., show an error message to the user
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

    private async void btnFrontimage_Clicked(object sender, EventArgs e)
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
        App._ValidIDFrontSelfie = result;
        Frontimage.Source = ImageSource.FromStream(() => stream);


    }

    private async void btnBackimage_Clicked(object sender, EventArgs e)
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
        App._ValidIDBackSelfie = result;
        backimage.Source = ImageSource.FromStream(() => stream);


        
    }

    private bool isSaveInProgress = false;

    private async void saveBtn_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        // Check if a save operation is already in progress

        if (Frontimage.Source == null && Frontimage.Source.ToString() == "selfieidfront.png")
        {
            // Display alert for front ID image not uploaded
            await DisplayAlert("Error", "Please upload the front ID image.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        if (backimage.Source == null && backimage.Source.ToString() == "selfieidfront.png")
        {
            // Display alert for back ID image not uploaded
            await DisplayAlert("Error", "Please upload the back ID image.", "OK");
            progressLoading.IsVisible = false;
            return;
        }
        if (isSaveInProgress)
        {
            return; // If yes, do nothing
        }

        try
        {
            // Set the flag to indicate that a save operation is in progress
            isSaveInProgress = true;

            string userEmail = App.email;

            var status = statusUser.SelectedItem.ToString();
            var result = await Valids.SaveValid(_ValidIDFront, _ValidIDBack, _ValidIDFrontSelfie, _ValidIDBackSelfie, userEmail, status);                     
            var emailTask = App.FirebaseService.GetUserKeyByEmailinDenied(userEmail);
            var email = await emailTask;
            var a =  Valids.DeleteDeniedApplicationAsync(email);
            await DisplayAlert("Confirmation", "Your Application for Become a Seller has been submitted. Please wait for 3 to 7 days for verification", "OK");
            await Navigation.PushModalAsync(new BuyerHomePage(App.key));
        }
        finally
        {
            // Ensure that the flag is reset even if an exception occurs
            isSaveInProgress = false;
            progressLoading.IsVisible = false;
        }
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
       
        await Navigation.PopModalAsync();
        progressLoading.IsVisible = false;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Frontimage.Source = null;
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        backimage.Source = null;
    }
}