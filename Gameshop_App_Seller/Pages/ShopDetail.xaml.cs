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

    public ShopDetail()
	{
        if (!CheckInternetConnection())
        {
            // Optionally display an alert or take appropriate action if there's no internet
            return;
        }

        InitializeComponent();
    }

    public ShopDetail(string userId)
    {
        InitializeComponent();
        this.userId = userId; // Store the user ID
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
                ShopProfilePicture.Source = !string.IsNullOrEmpty(userSnapshot.ShopProfile)
                    ? new UriImageSource
                    {
                        Uri = new Uri(userSnapshot.ShopProfile),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(1)
                    }
                    : "account.png";

                ShopCoverPhoto.Source = !string.IsNullOrEmpty(userSnapshot.ShopCoverImg)
                   ? new UriImageSource
                   {
                       Uri = new Uri(userSnapshot.ShopCoverImg),
                       CachingEnabled = true,
                       CacheValidity = TimeSpan.FromDays(1)
                   }
                   : "account.png";
                shopprofile = new FileResult(userSnapshot.ShopProfile);
                shopcover = new FileResult(userSnapshot.ShopCoverImg);
                ShopNameEntry.Text = userSnapshot.ShopName;
                ShopContactEntry.Text = userSnapshot.ShopContactNumber;
                ShopMessenger.Text = userSnapshot.ShopMessengerLink;              
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
        progressLoading.IsVisible = true;
        ShopProfilePicture.Source = null;
        progressLoading.IsVisible = false;
    }

    private void RemoveCoverShop_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        ShopCoverPhoto.Source = null;
        progressLoading.IsVisible = false;
    }



    private async void updateShopBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (!termsCheckbox.IsChecked)
        {
            await DisplayAlert("Information", "Please agree to the terms and conditions before updating shop details.", "OK");
            return;
        }

        string shopName = ShopNameEntry.Text.Trim();
        string shopContact = ShopContactEntry.Text.Trim();

        // Validate shop name
        if (string.IsNullOrEmpty(shopName))
        {
            await DisplayAlert("Information", "Please enter a valid shop name.", "OK");
            return;
        }

        // Validate telephone number
        if (string.IsNullOrEmpty(shopContact) || shopContact.Length != 11 || !shopContact.All(char.IsDigit))
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



        if (ShopProfile == null)
        {
            // One or both of the images is null, show an alert
            await DisplayAlert("Information", "Please upload An shop profile.", "OK");
            return;
        }


        if (ShopCover == null)
        {
            await DisplayAlert("Information", "Please upload An cover images.", "OK");
            return;
        }

        var result = await Shop.UpdateShop(ShopProfile, ShopCover, shopName, shopContact, ShopMessenger.Text);



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




    private async void termsCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        progressLoading.IsVisible = true;
        var result = await DisplayAlert("Terms and Conditions", "Welcome to GameShop!\r\n\r\nBy using our application, you agree to comply with and be bound by the following terms and conditions: Please read these terms carefully before using our services.\r\n\r\n1. **User Registration:**\r\n   a. You agree to provide accurate and complete information during the registration process.\r\n   b. You are responsible for maintaining the confidentiality of your account information.\r\n\r\n2. **User Content:**\r\n   a. You are solely responsible for the content you upload, post, or share on the platform.\r\n   b. Do not post content that violates intellectual property rights, privacy, or any applicable laws.\r\n\r\n3. **Data Collection:**\r\n   a. We collect personal information as outlined in our privacy policy.\r\n   b. You agree to the collection, use, and disclosure of your data as described in our privacy policy.\r\n\r\n4. **Product Listings:**\r\n   a. Sellers are responsible for providing accurate and truthful information about their products.\r\n   b. Buyers are encouraged to verify product details before making a purchase.\r\n\r\n5. **Shop Profiles:**\r\n   a. Shop owners are responsible for the accuracy of their shop information.\r\n   b. We reserve the right to verify and validate shop profiles.\r\n\r\n6. **Verification:**\r\n   a. Some features may require user verification.\r\n   b. Falsifying information during the verification process is prohibited.\r\n\r\n7. **Code of Conduct:**\r\n   a. Be respectful and considerate in all interactions.\r\n   b. Do not engage in harassment, bullying, or any harmful behavior.\r\n\r\n8. **Security:**\r\n   a. Keep your account credentials secure.\r\n   b. Report any security vulnerabilities promptly.\r\n\r\n9. **Compliance:**\r\n   a. Users must comply with all applicable laws and regulations.\r\n   b. Non-compliance may result in account suspension or termination.\r\n\r\n10. **Termination:**\r\n    a. We reserve the right to terminate accounts for violations of these terms.\r\n    b. Users may terminate their accounts at any time.\r\n\r\n11. **Dispute Resolution:**\r\n    a. Any disputes will be resolved in accordance with our dispute resolution policy.\r\n\r\n12. **Changes to Terms:**\r\n    a. We reserve the right to modify these terms at any time.\r\n    b. Users will be notified of significant changes.\r\n\r\nBy using our application, you acknowledge that you have read and understood these terms and agree to be bound by them. If you do not agree with any part of these terms, please do not use our services.\r\n\r\nFor any questions or concerns, please contact us at GAMESHOP@GMAIL.COM .\r\n\r\nThank you for using GameShop!", "Agree", "Disagree");
        if (result)
        {
            // User agreed to the terms and conditions
            // You can add your logic here
        }
        else
        {

            termsCheckbox.IsChecked = false;
            return;// Uncheck the checkbox if the user disagrees
        }
        progressLoading.IsVisible = false;
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
        await Navigation.PopModalAsync();
    }
}