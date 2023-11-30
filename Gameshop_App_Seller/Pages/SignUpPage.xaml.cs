using Firebase.Auth;
using Gameshop_App_Seller.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Gameshop_App_Seller.Pages;

public partial class SignUpPage : INotifyPropertyChanged
{
    public string webAPIKey = "AIzaSyDkunRqHTm1yzzAy59rU_1m9GSxOZkzpoA";
    private Users Addusers = new();
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
        var email = emailEntry.Text;

        progressLoading.IsVisible = false;
        try
        {
            string birthdaypicker = birthdayPicker.ToString();
            string genderpicker = genderPicker.ToString();

            if (String.IsNullOrEmpty(FNEntry.Text))
            {
                await DisplayAlert("Warning", "Type First Name", "Ok");
                progressLoading.IsVisible = false;
                return;
            }
            if (String.IsNullOrEmpty(LNEntry.Text))
            {
                await DisplayAlert("Warning", "Type name", "Ok");
                progressLoading.IsVisible = false;
                return;
            }
            if (String.IsNullOrEmpty(emailEntry.Text) && Regex.IsMatch(email,emailPattern))
            {
                await DisplayAlert("Warning", "Type your email or your email is invalid", "Ok");
                progressLoading.IsVisible = false;
                return;
            }
            if (passwordEntry.Text.Length < 6)
            {
                await DisplayAlert("Warning", "Password should be 6 digit.", "Ok");
                progressLoading.IsVisible = false;
                return;
            }
            if (String.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Warning", "Type password", "Ok");
                progressLoading.IsVisible = false;
                return;
            }
            var genderpick = genderPicker.SelectedItem.ToString();
            var selectBirthdate = birthdayPicker.Date.ToString("yyyy-MM-dd");
            var result = await Addusers.addusers(FNEntry.Text, LNEntry.Text, emailEntry.Text, passwordEntry.Text, locationEntry.Text, selectBirthdate, genderpick);
            //bool isSave = await Addusers.Register(emailEntry.Text, FNEntry.Text, LNEntry.Text, locationEntry.Text, birthdaypicker, genderpicker, passwordEntry.Text);
            if (result)
            {
                await DisplayAlert("Register user", "Registration completed", "Ok");
                progressLoading.IsVisible = true;
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Register user", "Registration failed", "Ok");
                progressLoading.IsVisible = false;
            }
        }
        catch (Exception exception)
        {
            if (exception.Message.Contains("EMAIL_EXISTS"))
            {
                await DisplayAlert("Warning", "Email exist", "Ok");
            }
            else
            {
                await DisplayAlert("Error", exception.Message, "Ok");
                progressLoading.IsVisible = false;
            }

        }
        progressLoading.IsVisible = false;

    }

}


