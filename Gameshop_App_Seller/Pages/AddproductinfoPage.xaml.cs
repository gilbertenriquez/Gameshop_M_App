namespace Gameshop_App_Seller.Pages;

public partial class AddproductinfoPage : ContentPage
{
	public AddproductinfoPage()
	{
		InitializeComponent();
	}


    private async void exit_Button_Clicked(object sender, EventArgs e)
    {
             await Navigation.PushModalAsync(new HomePage());
    }


}