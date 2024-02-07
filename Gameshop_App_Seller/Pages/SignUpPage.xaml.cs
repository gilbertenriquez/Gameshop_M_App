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
            await DisplayAlert("Information", "Please agree to the terms and conditions & Privacy Policy before you can register.", "OK");
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

            if (passwordEntry.Text.Length < 8)
            {
                await DisplayAlert("Warning", "Password should be at least 8 characters.", "Ok");
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

            var registrationResult = await Regis.AddUser(FNEntry.Text, LNEntry.Text, emailEntry.Text.ToLower(), passwordEntry.Text, locationEntry.Text, birthdaypicker, genderpicker);

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


