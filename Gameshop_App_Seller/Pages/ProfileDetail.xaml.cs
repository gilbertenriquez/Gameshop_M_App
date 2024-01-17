using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class ProfileDetail : ContentPage
{
    CancellationTokenSource cancellationTokenSource = new();
    private string userId;
    private Users updateProfile = new();
    public ProfileDetail()
	{
      


        InitializeComponent();
	}

    public ProfileDetail(string userId)
    {
        InitializeComponent();
        this.userId = userId; // Store the user ID
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
            var userSnapshot = await ClientUsers
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

                fnameEntry.Text = userSnapshot.FNAME;
                lnameEntry.Text = userSnapshot.LNAME;
                Emailentry.Text = userSnapshot.MAIL;
                Passwordentry.Text = userSnapshot.PASSWORD;
                Addressentry.Text = userSnapshot.Haddress;
                birthdayPicker.Date = DateTime.Parse(userSnapshot.BIRTHDAY);

                // Set the value for Picker (assuming GENDER is a string property)
                genderPicker.SelectedItem = userSnapshot.GENDER;
            }
            else
            {
                // Handle the case where user data is not found
                // You might want to display an error message or handle it accordingly
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
            return;
        }
        // Assuming _mainimgResult is an instance of FileResult obtained from a file picker
        var result = await updateProfile.Update(
            _mainimgResult, // Assuming you want to pass the file path or null if _mainimgResult is null
            Emailentry.Text,
            birthdayPicker.Date.ToString("dd-MM-yyyy"), // Use Date property to get the selected date
            fnameEntry.Text,
            lnameEntry.Text,
            genderPicker.SelectedItem?.ToString(), // Use SelectedItem property to get the selected item or null if nothing is selected
            Addressentry.Text,
            Passwordentry.Text
        );

        if (result)
        {
            await DisplayAlert("Message", "Update Successfully", "OK");
        }
        else
        {
            await DisplayAlert("Message", "Update Not Successfully", "OK");
        }
        progressLoading.IsVisible = false;
    }

    private void RemoveIMGbtn_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        ProfilePictureUser.Source = null;
        progressLoading.IsVisible = false;
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PopModalAsync();
        progressLoading.IsVisible = false;
    }
}