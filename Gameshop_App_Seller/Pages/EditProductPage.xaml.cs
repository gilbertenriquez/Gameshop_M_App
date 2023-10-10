namespace Gameshop_App_Seller.Pages;

public partial class EditProductPage : ContentPage
{
	public EditProductPage()
	{
		InitializeComponent();
	}

    private async void NEXTBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new editInfoPage());
    }

    private async void BTNBACK_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}