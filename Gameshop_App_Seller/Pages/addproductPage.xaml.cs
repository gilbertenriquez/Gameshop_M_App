namespace Gameshop_App_Seller.Pages;

public partial class addproductPage : ContentPage
{
	public addproductPage()
	{
		InitializeComponent();
	}

    private async void backBTN_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }

    private async void NEXTBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddproductinfoPage());  
    }
}