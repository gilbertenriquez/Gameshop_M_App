namespace Gameshop_App_Seller.Pages;

public partial class PreviewItem : ContentPage
{
    public string productemail { get; set; }
    public string reporter { get; set; }
    public string productItemName { get; set; }
    public string productItemprice { get; set; }
    public string imageProduct { get; set; }

    private bool isExpanded = false;
    public PreviewItem()
	{
		InitializeComponent();
    }



    public PreviewItem( string productemail, string productName, string productPrice, string ProductDescrip, string productQuantity, string mainImage, string image1, string image2, string image3, string image4, string image5, string image6)
    {
        InitializeComponent();
        this.productemail = productemail;
        this.reporter = App.email;
        this.productItemName = productName;
        this.productItemprice = productPrice;
        this.imageProduct = mainImage;
        ProductName.Text = productName;
        ProductDescription.Text = ProductDescrip;
        ProductPrice.Text = productPrice;
        if (productQuantity == "0")
        {
            itemQuantity.Text = "Sold Out";
        }
        else
        {
            itemQuantity.Text = productQuantity;
        }
        PhotoCarousel.ItemsSource = new List<string>
            {
                mainImage,
                image1,
                image2,
                image3,
                image4,
                image5,
                image6
            };

    }


    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {


            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }

    private void OnSeeMore_Tapped(object sender, TappedEventArgs e)
    {
        if (isExpanded)
        {
            // If expanded, show less
            ProductDescription.MaxLines = 1;
            SeeMoreLabel.Text = "See More";
        }
        else
        {
            // If not expanded, show more
            ProductDescription.MaxLines = int.MaxValue; // Set a large value to allow as many lines as needed
            SeeMoreLabel.Text = "See Less";
        }

        isExpanded = !isExpanded;
    }
    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}