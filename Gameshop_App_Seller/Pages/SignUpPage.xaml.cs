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
        await Navigation.PopModalAsync();
    }


    private async void nextBTN_Clicked(object sender, EventArgs e)
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
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

}


