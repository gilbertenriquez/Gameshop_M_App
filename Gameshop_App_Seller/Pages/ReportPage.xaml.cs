using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;

namespace Gameshop_App_Seller.Pages;

public partial class ReportPage : ContentPage
{

    private Users Sickboi = new  Users();
    public ReportPage()
    {
        InitializeComponent();
    }

    public ReportPage(string userKey)
    {
        InitializeComponent();
        UserKey = userKey;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        Productname.Text = productname;
        Productprice.Text = productprice;
        Emailtxt.Text = email;
    }




    private async void btnSubmit_Clicked(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(reporttxt.Text))
        {
            await DisplayAlert("Message", "Fill up the empty field", "OK");
            return;
        }
        await DisplayAlert("Message", "Your Report has been Submitted", "OK");
        reporttxt.Text = "";
        return;
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}