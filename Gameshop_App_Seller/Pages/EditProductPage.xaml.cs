using Gameshop_App_Seller.Models;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Pages;
using static Gameshop_App_Seller.App;


namespace Gameshop_App_Seller.Pages;

public partial class EditProductPage : ContentPage
{
    CancellationTokenSource cancellationTokenSource = new();
    private string mainImageItem;
    private Users updateProduct = new Users();
    private string productpath;
    private string productemail;
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
      string images6,
      string productpath,
      string productemail)
    {
       
        InitializeComponent();
        Initialize(productName, productDesc, productPrice, productQuantity,
                   MainImageItem, images1, images2, images3, images4, images5, images6,productpath, productemail);
    }

    private void Initialize(
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
        string images6,
        string productpath,
        string productemail)
    {
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
        this.productpath = productpath;
        this.productemail = productemail;
    }


    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        _mainimgResult = null;
        _img1Result = null;
        _img2Result = null;
        _img3Result = null;
        _img4Result = null;
        _img5Result = null;
        _img6Result = null;
        itemName.Text = null;
        itemDescription.Text = null;
        itemPrice.Text = null;
        itemQuanity.Text = null;
        productpath = null;
        await Navigation.PopModalAsync();
        
    }

    private async void updateMainImageItemBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select main image",
            FileTypes = FilePickerFileType.Images
        });
        if (result == null) return;

        FileInfo fi = new(result.FullPath);
        var size = fi.Length;

        if (size > 524288)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromRgb(32, 32, 40),
                TextColor = Colors.WhiteSmoke,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(10),
                ActionButtonFont = Font.SystemFontOfSize(10)
            };
            const string text = "The image you have selected is more than 0.50MB please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }
        var stream = await result.OpenReadAsync();
        App._mainimgResult = result;
        MainImage.Source = ImageSource.FromStream(() => stream);
        progressLoading.IsVisible = false;
    }

    private void RemoveImageBTN_Clicked(object sender, EventArgs e)
    {
   
        MainImage.Source = null;
 
    }

    private async void updateSupportImageItemBTN_Clicked(object sender, EventArgs e)
    {
        {
            progressLoading.IsVisible = true;
            var ctr = 0;
            var results = await FilePicker.PickMultipleAsync();
            foreach (var result in results)
            {
                
                ctr++;
                switch (ctr)
                {
                    //first image
                    case 1:
                        {
                            _img1Result = result;
                            var stream = await result.OpenReadAsync();
                            mage1.Source = ImageSource.FromStream(() => stream);
                            break;
                        }
                    case 2:
                        {
                            _img2Result = result;
                            var stream = await result.OpenReadAsync();
                            mage2.Source = ImageSource.FromStream(() => stream);
                            break;
                        }
                    case 3:
                        {
                            _img3Result = result;
                            var stream = await result.OpenReadAsync();
                            mage3.Source = ImageSource.FromStream(() => stream);
                            break;
                        }
                    case 4:
                        {
                            _img4Result = result;
                            var stream = await result.OpenReadAsync();
                            mage4.Source = ImageSource.FromStream(() => stream);
                            break;
                        }
                    case 5:
                        {
                            _img5Result = result;
                            var stream = await result.OpenReadAsync();
                            mage5.Source = ImageSource.FromStream(() => stream);
                            break;
                        }
                    case 6:
                        {
                            _img6Result = result;
                            var stream = await result.OpenReadAsync();
                            mage6.Source = ImageSource.FromStream(() => stream);
                            break;
                        }

                }
            }
            progressLoading.IsVisible = false;
        }
    }

    private void BTNremoveSuppportImage_Clicked(object sender, EventArgs e)
    {
        mage1.Source = null;
        mage2.Source = null;
        mage3.Source = null;
        mage4.Source = null;
        mage5.Source = null;
        mage6.Source = null;
    }

    private async void UpdateBTNitem_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        // Check if any of the image results is empty or null
        if (_mainimgResult == null &&
            _img1Result == null &&
            _img2Result == null &&
            _img3Result == null &&
            _img4Result == null &&
            _img5Result == null &&
            _img6Result == null)
        {        
            await DisplayAlert("Alert!", "Please upload a new image for all fields to proceed.", "OK");
            return;
        }

        var result = await updateProduct.UpdateProductUser(
            _mainimgResult,
            _img1Result,
            _img2Result,
            _img3Result,
            _img4Result,
            _img5Result,
            _img6Result,
            itemName.Text,
            itemDescription.Text,
            itemPrice.Text,
            itemQuanity.Text,
            productpath);

        if (result)
        {
            // Product update successful
            await DisplayAlert("Success", "Product updated successfully!", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            // Product update failed
            await DisplayAlert("Error", "Failed to update product.", "OK");
        }
        progressLoading.IsVisible = false;
    }


}
