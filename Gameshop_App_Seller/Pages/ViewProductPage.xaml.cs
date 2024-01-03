namespace Gameshop_App_Seller.Pages;

public partial class ViewProductPage : ContentPage
{
	public ViewProductPage()
	{
		InitializeComponent();
	}

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}