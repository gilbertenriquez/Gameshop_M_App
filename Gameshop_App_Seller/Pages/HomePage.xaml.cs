namespace Gameshop_App_Seller.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void AddProdsBTN_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new addproductPage());
    }

    private async void EditProdsBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new EditProductPage());
    }

    private async void ChatBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ChatHomepage());
    }
}