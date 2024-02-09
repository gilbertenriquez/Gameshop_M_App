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
    private string image1;
    private string image2;
    private string image3;
    private string image4;
    private string image5;
    private string image6;
    private Users ItemMages;

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
        image1 = images1;
        image2 = images2;
        image3 = images3;
        image4 = images4;
        image5 = images5;
        image6 = images6;
        OnAppearing();       
    }
     protected override async void OnAppearing()
    {
        ItemMages = await ClientUsers.Child(productpath).OnceSingleAsync<Users>();
        ItemMages.Imagae_1_link = mainImageItem;
        ItemMages.image1 = image1;
        ItemMages.image2 = image2;
        ItemMages.image3 = image3;
        ItemMages.image4 = image4;
        ItemMages.image5 = image5;
        ItemMages.image6 = image6;
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
   

        string mainItem = null;

        if (_mainimgResult != null)
        {
            mainItem = _mainimgResult.FullPath;
        }
        else if (!string.IsNullOrEmpty(mainImageItem))
        {
            mainItem = mainImageItem;
        }
        else
        {
            mainItem = "account.png";
        }

        string Itemimage1 = null;
        if (_img1Result != null)

        {
            Itemimage1 = _img1Result.FullPath;
        }
        else if (!string.IsNullOrEmpty(image1))
        {
            Itemimage1 = image1;
        }
        else
        {
            Itemimage1 = "account.png";
        }

        string Itemimage2 = null;
        if (_img2Result != null)

        {
            Itemimage2 = _img2Result.FullPath;
        }
        else if (!string.IsNullOrEmpty(image2))
        {
            Itemimage2 = image2;
        }
        else
        {
            Itemimage2 = "account.png";
        }

        string Itemimage3 = null;
        if (_img3Result != null)

        {
            Itemimage3 = _img3Result.FullPath;
        }
        else if (!string.IsNullOrEmpty(image3))
        {
            Itemimage3 = image3;
        }
        else
        {
            Itemimage3 = "account.png";
        }

        string Itemimage4 = null;
        if (_img4Result != null)

        {
            Itemimage4 = _img4Result.FullPath;
        }
        else if (!string.IsNullOrEmpty(image4))
        {
            Itemimage4 = image4;
        }
        else
        {
            Itemimage4 = "account.png";
        }

        string Itemimage5 = null;
        if (_img5Result != null)

        {
            Itemimage5 = _img5Result.FullPath;
        }
        else if (!string.IsNullOrEmpty(image5))
        {
            Itemimage5 = image5;
        }
        else
        {
            Itemimage5 = "account.png";
        }

        string Itemimage6 = null;
        if (_img6Result != null)

        {
            Itemimage6 = _img6Result.FullPath;
        }
        else if (!string.IsNullOrEmpty(image6))
        {
            Itemimage6 = image6;
        }
        else
        {
            Itemimage6 = "account.png";
        }


        var result = await updateProduct.UpdateProductUser(
            _mainimgResult,
            _img1Result,
            _img2Result,
            _img3Result,
            _img4Result,
            _img5Result,
            _img6Result,
            ItemMages,
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
