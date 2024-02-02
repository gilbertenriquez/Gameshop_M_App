using Firebase.Auth;
using Gameshop_App_Seller.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Gameshop_App_Seller.Models.Users;

namespace Gameshop_App_Seller.Pages;

public partial class SignUpPage : INotifyPropertyChanged
{
    public string webAPIKey = "AIzaSyDkunRqHTm1yzzAy59rU_1m9GSxOZkzpoA";
    private Users Regis = new();
	public SignUpPage()
	{


        InitializeComponent();
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PopModalAsync();
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



    private async void nextBTN_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        if (!termsCheckbox.IsChecked)
        {
            await DisplayAlert("Information", "Please agree to the terms and conditions before you can register.", "OK");
            return;
        }

        string emailPattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
        progressLoading.IsVisible = false;

        try
        {
            string birthdaypicker = birthdayPicker.Date.ToString("yyyy-MM-dd");
            string genderpicker = genderPicker.SelectedItem?.ToString(); // Handle potential null

            if (String.IsNullOrEmpty(FNEntry.Text))
            {
                await DisplayAlert("Warning", "Type First Name", "Ok");
                return;
            }

            if (String.IsNullOrEmpty(LNEntry.Text))
            {
                await DisplayAlert("Warning", "Type Last Name", "Ok");
                return;
            }

            if (String.IsNullOrEmpty(emailEntry.Text))
            {
                await DisplayAlert("Warning", "Please enter your email", "Ok");
                return;
            }

            if (!Regex.IsMatch(emailEntry.Text, emailPattern))
            {
                await DisplayAlert("Warning", "Enter a valid email (e.g., example@gmail.com)", "Ok");
                return;
            }

            if (String.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Warning", "Type password", "Ok");
                return;
            }

            if (passwordEntry.Text.Length < 6)
            {
                await DisplayAlert("Warning", "Password should be at least 6 characters.", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(locationEntry.Text))
            {
                await DisplayAlert("Warning", " Type your Home Address", "Ok");
            }


            if (genderPicker.SelectedItem == null)
            {
                await DisplayAlert("Warning", "Select your gender", "Ok");
                return;
            }

            var registrationResult = await Regis.AddUser(FNEntry.Text, LNEntry.Text, emailEntry.Text, passwordEntry.Text, locationEntry.Text, birthdaypicker, genderpicker);

            switch (registrationResult)
            {
                case RegistrationResult.Success:
                    await DisplayAlert("Register user", "Registration completed", "Ok");
                    progressLoading.IsVisible = true;
                    await Navigation.PopModalAsync();
                    break;
                case RegistrationResult.EmailExists:
                    await DisplayAlert("Warning", "Email already exists. Please try another email.", "Ok");
                    break;
                case RegistrationResult.Error:
                    await DisplayAlert("Error", "Registration failed", "Ok");
                    break;
            }
        }
        catch (Exception exception)
        {
            if (exception.Message.Contains("EMAIL_EXISTS"))
            {
                await DisplayAlert("Warning", "Email already exists. Please try another email.", "Ok");
            }
            else
            {
                await DisplayAlert("Error", exception.Message, "Ok");
            }
        }
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


