namespace Gameshop_App_Seller.Pages;

public partial class editInfoPage : ContentPage
{
	public editInfoPage()
	{
		InitializeComponent();
	}

    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {

    }

    private async void Back_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}