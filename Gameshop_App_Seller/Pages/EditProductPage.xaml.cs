namespace Gameshop_App_Seller.Pages;

public partial class EditProductPage : ContentPage
{
    private string mainImageItem;
    public EditProductPage()
    {
        InitializeComponent();
    }
    public EditProductPage(
           string productName,
           string productDesc,
           string productPrice,
           string productQuantity,
           string MainImageItem,
           string images1,
           string images2,
           string images3,
           string images4,
           string images5,
           string images6)
    {

        InitializeComponent();
        itemName.Text = productName;
        itemDescription.Text = productDesc;
        itemPrice.Text = productPrice;
        itemQuanity.Text = productQuantity;
        mainImageItem = MainImageItem; // Assign the original source to the field
        MainImage.Source = mainImageItem;
      
        mage1.Source = images1;
        mage2.Source = images2;
        mage3.Source = images3;
        mage4.Source = images4;
        mage5.Source = images5;
        mage6.Source = images6;

        // Attach tap event handlers for mage, mage1, mage2, ..., mage6      
        mage1.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnImageTapped(mage1)) });
        mage2.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnImageTapped(mage2)) });
        mage3.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnImageTapped(mage3)) });
        mage4.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnImageTapped(mage4)) });
        mage5.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnImageTapped(mage5)) });
        mage6.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnImageTapped(mage6)) });
    }



    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void OnImageTapped(Image tappedImage)
    { 
            MainImage.Source = tappedImage.Source;
    }


    private void mage1ToMain_Tapped(object sender, TappedEventArgs e)
    {
        OnImageTapped(mage1);
    }

    private void mage2ToMain_Tapped(object sender, TappedEventArgs e)
    {
        OnImageTapped(mage2);
    }

    private void mage3ToMain_Tapped(object sender, TappedEventArgs e)
    {
        OnImageTapped(mage3);
    }

    private void mage4ToMain_Tapped(object sender, TappedEventArgs e)
    {
        OnImageTapped(mage4);
    }

    private void mage5ToMain_Tapped(object sender, TappedEventArgs e)
    {
        OnImageTapped(mage5);
    }

    private void mage6ToMain_Tapped(object sender, TappedEventArgs e)
    {
        OnImageTapped(mage6);
    }
}