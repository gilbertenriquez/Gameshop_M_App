using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;
using System.Text.RegularExpressions;




namespace Gameshop_App_Seller.Pages;

public partial class ShopDetail : ContentPage
{
    CancellationTokenSource cancellationTokenSource = new();
    private Users Shop = new();
    private string userId;
    private FileResult shopprofile { get; set; }
    private FileResult shopcover { get; set; }
    private Users userSnapshots;
  

    public ShopDetail()
	{
       
        InitializeComponent();
    }

    public ShopDetail(string userId)
    {
        InitializeComponent();
        this.userId = userId; // Store the user ID
    }


    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            OnAppearing();
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
            userSnapshots = await ClientUsers
                .Child(userAccountPath)
                .OnceSingleAsync<Users>();

            // Check if the user data exists
            if (userSnapshots != null)
            {
                // Update the UI with the retrieved user data
                ShopProfilePicture.Source = !string.IsNullOrEmpty(userSnapshots.ShopProfile)
                    ? new UriImageSource
                    {
                        Uri = new Uri(userSnapshots.ShopProfile),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    }
                    : "account.png";

                ShopCoverPhoto.Source = !string.IsNullOrEmpty(userSnapshots.ShopCoverImg)
                   ? new UriImageSource
                   {
                       Uri = new Uri(userSnapshots.ShopCoverImg),
                       CachingEnabled = true,
                       CacheValidity = TimeSpan.FromDays(1)
                   }
                   : "account.png";
                shopprofile = new FileResult(userSnapshots.ShopProfile);
                shopcover = new FileResult(userSnapshots.ShopCoverImg);
                ShopNameEntry.Text = userSnapshots.ShopName;
                ShopContactEntry.Text = userSnapshots.ShopContactNumber;
                ShopMessenger.Text = userSnapshots.ShopMessengerLink;              
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


    private async void UploadCoverImage_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select Cover image",
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
        App.ShopCover = result;
        ShopCoverPhoto.Source = ImageSource.FromStream(() => stream);
        progressLoading.IsVisible = false;
    }

    private async void UploadProfileShop_Clicked(object sender, EventArgs e)
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
        App.ShopProfile = result;
        ShopProfilePicture.Source = ImageSource.FromStream(() => stream);
        progressLoading.IsVisible = false;
    }

    private void RemoveProfileShop_Clicked(object sender, EventArgs e)
    {
        ShopProfilePicture.Source = null;
    }

    private void RemoveCoverShop_Clicked(object sender, EventArgs e)
    {
        ShopCoverPhoto.Source = null;
    }



    private async void updateShopBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }


        // Validate shop name
        if (string.IsNullOrEmpty(ShopNameEntry.Text))
        {
            await DisplayAlert("Information", "Please enter a valid shop name.", "OK");
            return;
        }

        // Validate telephone number
        if (string.IsNullOrEmpty(ShopContactEntry.Text) || ShopContactEntry.Text.Length != 11 || !ShopContactEntry.Text.Trim().All(char.IsDigit))
        {
            await DisplayAlert("Information", "Please enter a valid 11-digit telephone number.", "OK");
            return;
        }


        string messengerLink = ShopMessenger.Text;

        // Regular expression pattern for a valid Facebook Messenger link
        string regexPattern = @"^(https?:\/\/)?(?:www\.)?m\.me\/[a-zA-Z0-9._-]+$";

        // Create a Regex object
        Regex regex = new Regex(regexPattern);

        if (!regex.IsMatch(messengerLink))
        {
            await DisplayAlert("Information", "Please enter a valid Facebook Messenger link.", "OK");
            return;
        }

        string shopProfilephoto = null;

        if (ShopProfile != null)
        {
            shopProfilephoto = ShopProfile.FullPath;
        }
        else if (!string.IsNullOrEmpty(userSnapshots.ShopProfile))
        {
            shopProfilephoto = userSnapshots.ShopProfile;
        }
        else
        {
            shopProfilephoto = "account.png";
        }

        string shopCoverPhoto = null;

        if (ShopCover != null)
        {
            shopCoverPhoto = ShopCover.FullPath;
        }
        else if (!string.IsNullOrEmpty(userSnapshots.ShopCoverImg))
        {
            shopCoverPhoto = userSnapshots.ShopCoverImg;
        }
        else
        {
            shopCoverPhoto = "account.png";
        }
        var result = await Shop.UpdateShop(ShopProfile, ShopCover, userSnapshots, ShopNameEntry.Text.Trim(), ShopContactEntry.Text.Trim(), ShopMessenger.Text);
        if (result)
        {
            await DisplayAlert("Information", "Successfully updated shop details", "OK");
            
        }
        else
        {
            await DisplayAlert("Information", "Failed to update shop details", "OK");
        }
        progressLoading.IsVisible = false;
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

    private void ShopContactEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        progressLoading.IsVisible = true;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            // Remove non-numeric characters
            string newText = new string(e.NewTextValue.Where(c => char.IsDigit(c)).ToArray());

            // Limit to 11 digits
            if (newText.Length > 11)
            {
                newText = newText.Substring(0, 11);
            }

            ShopContactEntry.Text = newText;
        }
        progressLoading.IsVisible = false;
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        if (shopprofile == null || shopcover == null ||
            string.IsNullOrWhiteSpace(ShopNameEntry.Text) ||
            string.IsNullOrWhiteSpace(ShopNameEntry.Text) ||
            string.IsNullOrWhiteSpace(ShopMessenger.Text))
        {
            // Display alert if any field is empty
            await DisplayAlert("Incomplete", "Please complete the process before going back.", "OK");
        }
        else
        {
            // All fields are filled, allow navigation back
            await Navigation.PopModalAsync();
        }
    }

}