namespace Gameshop_App_Seller.Pages;

public partial class MessagePage : ContentPage
{
    private string userKey;

    public MessagePage(string userKey)
    {
        InitializeComponent();
        this.userKey = userKey;
        // ... other initialization code
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new ChatHomepage(userKey));
    }

    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}