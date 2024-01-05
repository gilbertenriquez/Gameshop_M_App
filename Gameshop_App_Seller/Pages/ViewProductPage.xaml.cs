using CommunityToolkit.Maui.Core;
using static Gameshop_App_Seller.App;


namespace Gameshop_App_Seller.Pages;


public partial class ViewProductPage : ContentPage
{
   
    public ViewProductPage()
    {
        InitializeComponent();
    }

    public ViewProductPage(string productemail,string productName, string productPrice, string ProductDescrip, string productQuantity, string image1, string image2, string image3, string image4, string image5, string image6)
    {
        InitializeComponent();

        // Use the passed parameters to set up your ViewProductPage UI
        // For example, you can set the Source of Image controls to the provided image URLs
        // Similarly, set up other UI elements as needed.
        ProductName.Text = productName;
        ProductDescription.Text = ProductDescrip;
        ProductPrice.Text = productPrice;

        PhotoCarousel.ItemsSource = new List<string>
            {
                image1,
                image2,
                image3,
                image4,
                image5,
                image6
            };
    }




    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    } 
}