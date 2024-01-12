using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class ReportPage : ContentPage
{

    private Users Sickboi = new  Users();
    
    public string UserEmail { get; }
    public string UserKey { get; }
    private string ImageUri { get; set; }
    private string MainImageOriginalSource { get; set; }


    public ReportPage()
    {
        InitializeComponent();
        LoadUserData();
    }

    public ReportPage(string SellerName,string productName, string productPrice, string userEmail, string imageUri, string reporterMail)
    {
        InitializeComponent();

        LoadUserData();
        Productname.Text = productName;
        Productprice.Text = productPrice;
        Emailtxt.Text = userEmail;
        SellerShopname.Text = SellerName;

        // Set MainImage.Source to a placeholder or user-friendly representation
        MainImage.Source = imageUri; // Replace with your placeholder image

        reporterEmail.Text = reporterMail;

        // Store the actual URI in the ImageUri property for later use
        ImageUri = imageUri;

        // Set the actual URI in the MainImage.Source property (not displayed)
        MainImageOriginalSource = imageUri;
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
        var result = await Sickboi.ReportedProduct(ImageUri, Productname.Text, Productprice.Text, Emailtxt.Text, reporttxt.Text, ReporteruserEmail.ToString());
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
        Productname.Text = string.Empty;
        Productprice.Text = string.Empty;
        Emailtxt.Text = string.Empty;
        MainImage.Source = null;
        reporterEmail.Text = string.Empty;

        await Navigation.PopModalAsync();
    }
}