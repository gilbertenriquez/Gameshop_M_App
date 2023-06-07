using Gameshop_M_App.Models;
using Microsoft.Maui.Controls;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gameshop_M_App.Pages;

public partial class SignUpPage : ContentPage
{
    private Users gamers = new();
	public SignUpPage()
	{
		InitializeComponent();
	}

    private async void SaveBTN_Clicked(object sender, EventArgs e)
    {
        //var birthday = (string)birthdayPicker.Date.ToString("yyyy-MM-dd");

        //var Gender = (string)genderPicker.SelectedItem;


        //if (string.IsNullOrEmpty(FNEntry.Text) || string.IsNullOrEmpty(LNEntry.Text) || string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
        //{
        //    await DisplayAlert("Alert!", "Fill up the Empty Fields", "Got it!");
        //    return;
        //}

        //progressLoading.IsVisible = true;
        //var result = await gamers.AddUsers(FNEntry.Text, LNEntry.Text, 
        //    emailEntry.Text, 
        //    passwordEntry.Text, 
        //    birthday, 
        //    Gender);

        //if (result == true)
        //{
        //    await DisplayAlert("Alert!", "Account Successfully Created", "Ok!");
        await Navigation.PopModalAsync();
        //    progressLoading.IsVisible = false;
        //    return;

        //}
        //else
        //{

        //    await DisplayAlert("Alert", "Account Not Registered or your Email is already Exist or No Internet Connection", " OK!");
        //    progressLoading.IsVisible = false;
        //}


    }
}
