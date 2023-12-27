using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class ReportPage : ContentPage
{

    private Users Sickboi = new  Users();
    
    public string UserEmail { get; }
    public string UserKey { get; }
    public ReportPage()
    {
        InitializeComponent();
        LoadUserData();
    }

    public ReportPage(string productName, string productPrice, string userEmail, string image1, string reportermail)
    {
        InitializeComponent();


        LoadUserData();
        Productname.Text = productName;
        Productprice.Text = productPrice;
        Emailtxt.Text = userEmail;
        MainImage.Source = image1;
        reporterEmail.Text = reportermail;

        // Rest of your initialization code...
    }


    private async void LoadUserData()
    {
        if (!string.IsNullOrEmpty(App.email))
        {
            // Assuming you have a method to get user data by email
            var allUserData = await Sickboi.GetUserDataByEmailAsync(App.email);

            if (allUserData != null)
            {
                // Update UI elements with user data

                Productname.Text = allUserData.ProductName;
                Productprice.Text = allUserData.ProductPrice;
                Emailtxt.Text = allUserData.MAIL;
                MainImage.Source = allUserData.image1;
                reporterEmail.Text = allUserData.ReporterEmail;

            }
            else
            {
                // Handle the case where user data is not available
                // Display an error message or take appropriate action
            }
        }
    }




    private async void btnSubmit_Clicked(object sender, EventArgs e)
    {
        var ReporteruserEmail = App.email;
        var result = await Sickboi.ReportedProduct(MainImage.Source.ToString(), Productname.Text, Productprice.Text, Emailtxt.Text, reporttxt.Text, ReporteruserEmail.ToString());
        if (String.IsNullOrEmpty(reporttxt.Text))
        {
            await DisplayAlert("Message", "Fill up the empty field", "OK");
            return;
        }
        if (result)
        {
            await DisplayAlert("Message", "Your Report has been Submitted", "OK");
            reporttxt.Text = "";
            return;
        }
        else
        {
            await DisplayAlert("Message", "Your Report has not been Submitted", "OK");
            return;
        }
       
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}