using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;
using Firebase.Auth;
using System;
using System.Text;
using System.Security.Cryptography;

namespace Gameshop_App_Seller.Pages;

public partial class ProfileDetail : ContentPage
{
    CancellationTokenSource cancellationTokenSource = new();
    private string userId;
    private Users updateProfile = new();
    private Users userSnapshot;
    public ProfileDetail()
	{
        
        InitializeComponent();
        OnAppearing();

    }

    public ProfileDetail(string userId)
    {
        InitializeComponent();
        this.userId = userId;
        OnAppearing();
      // Store the user ID
    }

    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            // Perform the data refreshing logic here
            OnAppearing();
            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Assuming App.key is the unique identifier for the user
            var userId = App.key;

            // Construct the path to the user's account node
            var userAccountPath = $"Account/{userId}";

            // Retrieve the user data from the database
            userSnapshot = await ClientUsers
                .Child(userAccountPath)
                .OnceSingleAsync<Users>();

            // Check if the user data exists
            if (userSnapshot != null)
            {
                // Update the UI with the retrieved user data
                ProfilePictureUser.Source = !string.IsNullOrEmpty(userSnapshot.ProfilePicture)
                    ? new UriImageSource
                    {
                        Uri = new Uri(userSnapshot.ProfilePicture),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    }
                    : "account.png";
         
                Passwordentry.Text = userSnapshot.PASSWORD;
                fnameEntry.Text = userSnapshot.FNAME;
                lnameEntry.Text = userSnapshot.LNAME;
                Emailentry.Text = userSnapshot.MAIL;
                Addressentry.Text = userSnapshot.Haddress;
                birthdayPicker.Date = DateTime.Parse(userSnapshot.BIRTHDAY);
                genderPicker.SelectedItem = userSnapshot.GENDER;
            }          
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during data retrieval
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

  






    private async void UploadProfileImage_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
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
        ProfilePictureUser.Source = ImageSource.FromStream(() => stream);
        progressLoading.IsVisible = false;
    }

    private async void updateProfileBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        string profilePictureSource = null;

        if (_mainimgResult != null)
        {
            // If _mainimgResult is not null, use its FullPath
            profilePictureSource = _mainimgResult.FullPath;
            progressLoading.IsVisible = false;
        }
        else if (!string.IsNullOrEmpty(userSnapshot.ProfilePicture))
        {
            // If _mainimgResult is null, use the existing ProfilePicture from userSnapshot
            profilePictureSource = userSnapshot.ProfilePicture;
            progressLoading.IsVisible = false;
        }
        else
        {
            // If both _mainimgResult and userSnapshot.ProfilePicture are null, use a default image
            profilePictureSource = "account.png";
            progressLoading.IsVisible = false;
        }

        // Assuming _mainimgResult is an instance of FileResult obtained from a file picker
        progressLoading.IsVisible = true;
        var result = await updateProfile.Update(
            _mainimgResult, // Pass the FileResult directly
            userSnapshot,
            Emailentry.Text,
            birthdayPicker.Date.ToString("dd-MM-yyyy"),
            fnameEntry.Text,
            lnameEntry.Text,
            genderPicker.SelectedItem?.ToString(),
            Addressentry.Text,
            // Pass null if Passwordentry.Text is empty or null
            string.IsNullOrEmpty(Passwordentry.Text) ? null : Passwordentry.Text);

        if (result)
        {
            progressLoading.IsVisible = true;
            await DisplayAlert("Message", "Update Successfully", "OK");
            progressLoading.IsVisible = false;
        }
        else
        {
            await DisplayAlert("Message", "Update Not Successfully", "OK");
            progressLoading.IsVisible = false;
        }
        // Update the ProfilePictureUser.Source based on the condition above
        progressLoading.IsVisible = false;
    }


    private void RemoveIMGbtn_Clicked(object sender, EventArgs e)
    {

        ProfilePictureUser.Source = null;

    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void Linkuptosite_Tapped(object sender, TappedEventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        try
        {
            progressLoading.IsVisible = true; // Show the loading indicator

            // Open the URI asynchronously
            await Launcher.OpenAsync(new Uri("https://privacy.gov.ph/data-privacy-act/"));
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., if the URI is invalid or the app doesn't have permission to open the URI.
            Console.WriteLine($"Error opening URI: {ex.Message}");
        }
        finally
        {
            progressLoading.IsVisible = false; // Hide the loading indicator in both success and failure cases
        }
    }
}