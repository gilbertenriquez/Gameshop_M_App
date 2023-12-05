namespace Gameshop_App_Seller.Pages;

public partial class ReportPage : ContentPage
{
	public ReportPage()
	{
		InitializeComponent();
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