using Gameshop_App_Seller.Models;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Gameshop_App_Seller.Pages;
using static Gameshop_App_Seller.App;



namespace Gameshop_App_Seller.Pages;


public partial class BuyNowPage : ContentPage   
{
    CancellationTokenSource cancellationTokenSource = new();
    private string buyer { get; set; }
    private Users reciept = new Users();
    private string FBMessengerLink {  get; set; }
    private string ItemImageSold {get; set;}
    private string shopemail { get; set; }
    public BuyNowPage()
    {
        InitializeComponent();
        SetCurrentTimeAndDate();
    }

    public BuyNowPage(string Mlink, string shopnames, string productItemName, string buyermail, string productemail, string itemquantity, string imageProduct,string price)
    {
        InitializeComponent();
        GetTime.Text = DateTime.Now.ToString("h:mm tt");
        GetDateToday.Text = DateTime.Now.ToString("MM-dd-yyyy");
        this.buyer = buyermail;
        itemQuantity.Text = itemquantity;
        itemname.Text = productItemName;
        SellerName.Text = shopnames;
        ItemImage.Source = imageProduct;
        QuantityLabel.Text = itemquantity;
        this.FBMessengerLink = Mlink;
        ItemImageSold = imageProduct;
        itemPRICE.Text = price;
        this.shopemail = productemail;

    }




    private void SetCurrentTimeAndDate()
    {
        // Set current time
        GetTime.Text = DateTime.Now.ToString("h:mm tt");


        // Set current date
        GetDateToday.Text = DateTime.Now.ToString("MM-dd-yyyy");
    }


    protected override async void OnAppearing()
    {
        try
        {
            // Use the productemail parameter instead of App.email
            string userEmail = buyer.ToLower();
            string userKey = await GetUserKeyByEmail(userEmail);

            // Check if userKey is null or empty
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("User key is null or empty. Unable to proceed.");
                // Handle this case as needed, e.g., show an error message to the user
                return;
            }

            Console.WriteLine($"User key: {userKey}");

            var usersInfo = await reciept.GetUsersinfoAsync(userKey);

            if (usersInfo != null && usersInfo.Count > 0)
            {
                // Set the ShopName text in your UI
                BuyerName.Text = usersInfo[0].FNAME + " " + usersInfo[0].LNAME;
                //containMessengerLink.Text = usersInfo[0].ShopMessengerLink;
            }
            else
            {
                Console.WriteLine("User information is null or empty. Handle this case if needed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in OnAppearing: {ex.Message}");
        }
    }

    public async Task<string> GetUserKeyByEmail(string email)
    {
        var users = await ClientUsers
            .Child("Account")
            .OnceAsync<Users>();

        var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

        return userWithKey?.Key;
    }


    private void MinusButton_Clicked(object sender, EventArgs e)
    {
        int currentQuantity = int.Parse(QuantityLabel.Text);
        if (currentQuantity > 1)
            QuantityLabel.Text = (currentQuantity - 1).ToString();
    }

    private void PlusButton_Clicked(object sender, EventArgs e)
    {
        int currentQuantity = int.Parse(QuantityLabel.Text);
        int initialQuantity = int.Parse(itemQuantity.Text); // Initial value set in the constructor

        if (currentQuantity < initialQuantity)
            QuantityLabel.Text = (currentQuantity + 1).ToString();
    }


    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        await Navigation.PopModalAsync();
        progressLoading.IsVisible = false;
    }
    private async void addimageBTN_Clicked(object sender, EventArgs e)
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

        if (size > 52428800) // 50MB in bytes
        {
            // Display a message about the file size limit
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromRgb(32, 32, 40),
                TextColor = Colors.WhiteSmoke,
                ActionButtonTextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(10),
                ActionButtonFont = Font.SystemFontOfSize(10)
            };
            const string text = "The image you have selected is more than 50MB. Please ensure that the size of the image is less than the maximum size. Thank you!";
            const string actionButtonText = "Got it!";
            var duration = TimeSpan.FromSeconds(10);
            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
            return;
        }

        var stream = await result.OpenReadAsync();
        Prooftransraction.Source = ImageSource.FromStream(() => stream);

        // Store the result for later use if needed
        App.TransactionImage = result;
        progressLoading.IsVisible = false;
    }

    private void removeBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        Prooftransraction.Source = null;
        progressLoading.IsVisible = false;
    }

    private async void MessengerLink_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        string messengerLink = FBMessengerLink;

        if (!string.IsNullOrEmpty(messengerLink))
        {
            if (Uri.TryCreate(messengerLink, UriKind.Absolute, out Uri result))
            {
                // The link is a valid absolute URI
                await Launcher.OpenAsync(result);
            }
            else
            {
                // The link is not a valid URI, you can handle it accordingly
                // For example, open the link in the browser
                await Launcher.OpenAsync($"https://{messengerLink}");
            }
        }
        else
        {
            // The messenger link is empty, handle it accordingly
            // For example, display an alert or take any other action
        }
        progressLoading.IsVisible = false;
    }

    private async void ConfirmationBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Alert!", "No internet connection. Please check your network settings.", "OK");
            return;
        }

        if (TransactionImage == null)
        {
            await DisplayAlert("Information!", "Please Upload your Transaction Image or Screenshot", "OK");
            return;
        }


        var result = await reciept.PurchaseH(
                ItemImageSold,
                TransactionImage,
                itemname.Text,
                QuantityLabel.Text,
                itemPRICE.Text,
                GetTime.Text,
                GetDateToday.Text,
                shopemail,
                buyer
            );

            if (result)
            {
            // Purchase was successful, show alert
            await DisplayAlert("Purchase Confirmed", "Your purchase has been confirmed!", "OK");
            }
            else
            {
                // Purchase failed, show an error alert
                await DisplayAlert("Purchase Failed", "There was an error processing your purchase. Please try again.", "OK");
            }
        progressLoading.IsVisible = false;
    }
 
}