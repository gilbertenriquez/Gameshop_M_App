namespace Gameshop_M_App.Pages;

public partial class BuyerHomePage : ContentPage
{
	public BuyerHomePage()
	{
		InitializeComponent();
	}

    private async void btnchat_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new ChatPageBuyer());
    }
}