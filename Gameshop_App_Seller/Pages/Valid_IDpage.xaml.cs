namespace Gameshop_App_Seller.Pages;

public partial class Valid_IDpage : ContentPage
{
	public Valid_IDpage()
	{
		InitializeComponent();
	}

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }

    

    private async void saveBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginPage());
    }
}