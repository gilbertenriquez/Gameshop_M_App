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

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        if (!termsCheckbox.IsChecked)
        {
            await DisplayAlert("Information", "Please agree to the terms and conditions & Privacy Policy before updating shop details.", "OK");
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

        string Shopprofile = null;

        if (ShopProfile != null)
        {
            Shopprofile = _mainimgResult.FullPath;
        }
        else if (!string.IsNullOrEmpty(userSnapshots.ShopProfile))
        {
            Shopprofile = userSnapshots.ShopProfile;
        }
        else
        {            
            Shopprofile = "account.png";
        }

        string ShopcoverPhoto = null;

        if (ShopProfile != null)
        {
            ShopcoverPhoto = _mainimgResult.FullPath;
        }
        else if (!string.IsNullOrEmpty(userSnapshots.ShopCoverImg))
        {
            ShopcoverPhoto = userSnapshots.ShopCoverImg;
        }
        else
        {
            ShopcoverPhoto = "account.png";
        }

        var result = await Shop.UpdateShop(ShopProfile, ShopCover, userSnapshots ,shopName, shopContact, ShopMessenger.Text);



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
        var result = await DisplayAlert("Terms and Conditions & Privacy Policy for Gameshop",
  "Welcome to GameShop!\r\n\r\n" +
  "By using our application, you agree to comply with and be bound by the following terms and conditions: Please read these terms carefully before using our services.\r\n\r\n" +
  "1. **User Registration:**\r\n" +
  "   a. You agree to provide accurate and complete information during the registration process.\r\n" +
  "   b. You are responsible for maintaining the confidentiality of your account information.\r\n\r\n" +
  "2. **User Content:**\r\n" +
  "   a. You are solely responsible for the content you upload, post, or share on the platform.\r\n" +
  "   b. Do not post content that violates intellectual property rights, privacy, or any applicable laws.\r\n\r\n" +
  "3. **Data Collection:**\r\n" +
  "   a. We collect personal information as outlined in our privacy policy.\r\n" +
  "   b. You agree to the collection, use, and disclosure of your data as described in our privacy policy.\r\n\r\n" +
  "4. **Product Listings:**\r\n" +
  "   a. Sellers are responsible for providing accurate and truthful information about their products.\r\n" +
  "   b. Buyers are encouraged to verify product details before making a purchase.\r\n\r\n" +
  "5. **Shop Profiles:**\r\n" +
  "   a. Shop owners are responsible for the accuracy of their shop information.\r\n" +
  "   b. We reserve the right to verify and validate shop profiles.\r\n\r\n" +
  "6. **Verification:**\r\n" +
  "   a. Some features may require user verification.\r\n" +
  "   b. Falsifying information during the verification process is prohibited.\r\n\r\n" +
  "7. **Code of Conduct:**\r\n" +
  "   a. Be respectful and considerate in all interactions.\r\n" +
  "   b. Do not engage in harassment, bullying, or any harmful behavior.\r\n\r\n" +
  "8. **Security:**\r\n" +
  "   a. Keep your account credentials secure.\r\n" +
  "   b. Report any security vulnerabilities promptly.\r\n\r\n" +
  "9. **Compliance:**\r\n" +
  "   a. Users must comply with all applicable laws and regulations.\r\n" +
  "   b. Non-compliance may result in account suspension or termination.\r\n\r\n" +
  "10. **Termination:**\r\n" +
  "    a. We reserve the right to terminate accounts for violations of these terms.\r\n" +
  "    b. Users may terminate their accounts at any time.\r\n\r\n" +
  "11. **Dispute Resolution:**\r\n" +
  "    a. Any disputes will be resolved in accordance with our dispute resolution policy.\r\n\r\n" +
  "12. **Changes to Terms:**\r\n" +
  "    a. We reserve the right to modify these terms at any time.\r\n" +
  "    b. Users will be notified of significant changes.\r\n\r\n" +
  "By using our application, you acknowledge that you have read and understood these terms and agree to be bound by them. If you do not agree with any part of these terms, please do not use our services.\r\n\r\n" +
  "For any questions or concerns, please contact us at GAMESHOP@GMAIL.COM.\r\n\r\n" +
  "Privacy Policy for Gameshop\r\n" +
  "Updated: 06/01/2024\r\n" +
  "1. **Introduction:**\r\n" +
  "   a. Welcome to Gameshop! This Privacy Policy describes how we collect, use, and disclose personal information when you use our application. By accessing or using Gameshop, you agree to the terms outlined in this policy.\r\n\r\n" +
  "2. **Information We Collect:**\r\n" +
  "   a. Personal Information: We may collect personal information such as your name, email address, and contact details when you register or use our services.\r\n" +
  "   b. Transaction Information: If you engage in transactions through Gameshop, we may collect information related to those transactions, including payment information.\r\n" +
  "   c. Usage Data: We may collect information about how you interact with Gameshop, such as your browsing history, search queries, and device information.\r\n\r\n" +
  "3. **How We Use Your Information:**\r\n" +
  "   a. We use the collected information for various purposes, including:\r\n" +
  "      1. Providing and improving Gameshop services.\r\n" +
  "      2. Processing transactions and payments.\r\n" +
  "      3. Customizing content and recommendations.\r\n" +
  "      4. Communicating with you about our services and updates.\r\n" +
  "      5. Analyzing usage patterns to enhance user experience.\r\n\r\n" +
  "4. **User-Provided Payment Information:**\r\n" +
  "   a. Gameshop allows users to choose their preferred payment methods for transactions. We do not store or process payment information directly. All payment transactions are handled by third-party payment processors, and their terms and policies apply.\r\n\r\n" +
  "5. **Data Security:**\r\n" +
  "   a. We implement security measures to protect your personal information. However, no method of transmission over the internet or electronic storage is entirely secure. We encourage you to take steps to protect your information, such as using strong passwords.\r\n\r\n" +
  "6. **Third-Party Links and Services:**\r\n" +
  "   a. Gameshop may contain links to third-party websites or services. These third-party sites have their privacy policies, and we are not responsible for their practices. We encourage you to review the privacy policies of these third parties.\r\n\r\n" +
  "7. **Changes to Privacy Policy:**\r\n" +
  "   a. We may update this Privacy Policy from time to time. We will notify you of any changes by posting the new policy on this page. We recommend reviewing this Privacy Policy periodically for any updates.\r\n\r\n" +
  "8. **Contact Us:**\r\n" +
  "   a. If you have any questions or concerns about this Privacy Policy, please contact us at GAMESHOP@GMAIL.COM.",
   "Agree", "Disagree");
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
        await Navigation.PopModalAsync();
    }
}