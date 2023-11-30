namespace Gameshop_App_Seller.Pages;

public partial class MessagePage : ContentPage
{
	public MessagePage()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new ChatHomepage());
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}