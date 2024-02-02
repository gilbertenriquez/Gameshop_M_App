using CommunityToolkit.Maui.Core;
using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;


namespace Gameshop_App_Seller.Pages;


public partial class ViewProductPage : ContentPage
{
    private string userId;
    private Users GetUSERdata = new Users();
    public string productemail { get; set; }
    public string reporter {  get; set; }
    public string productItemName { get; set; }
    public string productItemprice { get; set; }
    public string imageProduct { get; set; }

    private bool isExpanded = false;

    public ViewProductPage()
    {
        InitializeComponent();
    }


    public ViewProductPage(string userId,string reporteremail ,string productemail, string productName, string productPrice, string ProductDescrip, string productQuantity,string mainImage ,string image1, string image2, string image3, string image4, string image5, string image6)
    {
        InitializeComponent();   
        this.userId = userId;
        this.productemail = productemail;
        this.reporter = App.email;
        this.productItemName = productName;
        this.productItemprice = productPrice;
        this.imageProduct = mainImage;
        ProductName.Text = productName;
        ProductDescription.Text = ProductDescrip;
        ProductPrice.Text = productPrice;
        itemQuantity.Text = productQuantity;
        OnAppearingSellerSolds();
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


    public async Task<string> GetUserKeyByEmail(string email)
    {
        var users = await ClientUsers
            .Child("Account")
            .OnceAsync<Users>();

        var userWithKey = users.FirstOrDefault(u => u.Object.MAIL == email);

        return userWithKey?.Key;
    }


    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            // Perform the data refreshing logic here 
            OnAppearingSellerSolds();
            OnAppearing();
            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }


    protected  async void OnAppearingSellerSolds()
    {
        try
        {
            base.OnAppearing();

            string userEmail = productemail.ToLower();

            var deniedApplicationsList = await GetUSERdata.GetPurchaseListAsync();

            if (deniedApplicationsList.Any())
            {
                // Order the list by confirmation status, unconfirmed items first
                var orderedList = deniedApplicationsList.OrderBy(item => item.IsConfirmed).ToList();

                // Calculate and display total sold quantity for each seller
                var sellerSoldQuantities = orderedList
                    .GroupBy(item => item.Seller)
                    .Select(group => new
                    {
                        Seller = group.Key,
                        TotalSoldQuantity = group.Sum(item => Convert.ToInt32(item.soldQuantity))
                    });

                // Clear previous content
                shopsolds.Text = "";

                // Display total sold quantities in the label
                foreach (var sellerSoldQuantity in sellerSoldQuantities)
                {
                    shopsolds.Text += $"SOLDS: {sellerSoldQuantity.TotalSoldQuantity}";
                }
            }
            else
            {
                // Clear the label if there is no data
                shopsolds.Text = "No Item has been Sold";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAppearingSellerSolds: {ex.Message}");
        }
    }


    protected override async void OnAppearing()
    {
        try
        {         
            // Use the productemail parameter instead of App.email
            string userEmail = productemail.ToLower();
            string userKey = await GetUserKeyByEmail(userEmail);

            // Check if userKey is null or empty
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("User key is null or empty. Unable to proceed.");
                // Handle this case as needed, e.g., show an error message to the user
                return;
            }

            Console.WriteLine($"User key: {userKey}");

            var usersInfo = await GetUSERdata.GetUsersinfoAsync(userKey);

            if (usersInfo != null && usersInfo.Count > 0)
            {
                // Set the ShopName text in your UI
                Shopnames.Text = usersInfo[0].ShopName;
                imglogo.Source = usersInfo[0].ShopProfile;
                containMessengerLink.Text = usersInfo[0].ShopMessengerLink;
                imgCover.Source = usersInfo[0].ShopCoverImg;
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



    private async void btnBackImg_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No internet connection.", "Please check your network settings.", "OK");
            // Handle this case as needed, e.g., show an error message to the user
            return;
        }
        // Navigate back to BuyerHomePage with the userId parameter
        await Navigation.PushModalAsync(new BuyerHomePage(userId));
        progressLoading.IsVisible = false;
    }

    private async void ReviewBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No internet connection.", "Please check your network settings.", "OK");
            // Handle this case as needed, e.g., show an error message to the user
            return;
        }

        // Check if the user is trying to rate their own shop
        if (App.email.Equals(productemail, StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Information!", "You cannot rate your own shop.", "OK");
            progressLoading.IsVisible = false;
            return;
        }

        await Navigation.PushModalAsync(new ReviewSeller(productemail));
        progressLoading.IsVisible = false;
    }

    private async void reportsBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No internet connection.", "Please check your network settings.", "OK");
            // Handle this case as needed, e.g., show an error message to the user
            return;
        }

        await Navigation.PushModalAsync(new ReportPage(Shopnames.Text, productItemName, productItemprice, productemail, imageProduct, reporter));
        progressLoading.IsVisible = false;
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

    private async void BuynowBTN_Clicked(object sender, EventArgs e)
    {
        progressLoading.IsVisible = true;

        try
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No internet connection.", "Please check your network settings.", "OK");
                // Handle this case as needed, e.g., show an error message to the user
                return;
            }
            string quantityToBuy = itemQuantity.Text;

            if (int.TryParse(quantityToBuy, out int quantity) && quantity == 0)
            {
                await DisplayAlert("Out of Stock", "The product is out of stock.", "OK");
                return;
            }
            await Navigation.PushModalAsync(new BuyNowPage(containMessengerLink.Text, Shopnames.Text, productItemName, reporter, productemail, quantityToBuy, imageProduct, ProductPrice.Text));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in BuynowBTN_Clicked: {ex.Message}");
        }
        finally
        {
            progressLoading.IsVisible = false;
        }
    }


    private async void viewSellerReview_Tapped(object sender, TappedEventArgs e)
    {
        progressLoading.IsVisible = true;

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("No internet connection.", "Please check your network settings.", "OK");
            // Handle this case as needed, e.g., show an error message to the user
            return;
        }
        

        await Navigation.PushModalAsync(new SellerReviews(productemail));
        progressLoading.IsVisible = false;
    }


    //private async void linkTOmessenger_Clicked(object sender, EventArgs e)
    //{
    //    string messengerLink = containMessengerLink.Text;

    //    if (!string.IsNullOrEmpty(messengerLink))
    //    {
    //        if (Uri.TryCreate(messengerLink, UriKind.Absolute, out Uri result))
    //        {
    //            // The link is a valid absolute URI
    //            await Launcher.OpenAsync(result);
    //        }
    //        else
    //        {
    //            // The link is not a valid URI, you can handle it accordingly
    //            // For example, open the link in the browser
    //            await Launcher.OpenAsync($"https://{messengerLink}");
    //        }
    //    }
    //    else
    //    {
    //        // The messenger link is empty, handle it accordingly
    //        // For example, display an alert or take any other action
    //    }
    //}
}