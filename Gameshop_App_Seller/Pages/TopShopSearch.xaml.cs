using Gameshop_App_Seller.Models;
using static Gameshop_App_Seller.App;


namespace Gameshop_App_Seller.Pages;

public partial class TopShopSearch : ContentPage
{
    private Users shops = new();
    private List<Users> shopDataList;
    public TopShopSearch()
    {
        InitializeComponent();
        OnAppearingDenied();
        OnAppearingVerified();
        LoadDataAsync();
        OnBackButtonPressed();

    }

    public TopShopSearch(string userKey) : this()
    {
        InitializeAsync(userKey);
    }


    protected override bool OnBackButtonPressed()
    {
        // Implement the logic to allow the back button press only on this page
        // Return true to indicate that the back button press has been handled
        return true;
    }
    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            // Perform the data refreshing logic here
            await LoadDataAsync();
            OnAppearingDenied();
            OnAppearingVerified();
            OnAppearing();
            // Stop the refreshing animation
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during refresh: {ex.Message}");
        }
    }

    private async Task LoadDataAsync()
    {
        var userProductLists = await shops.GetAllShopProfileAndFilteredReviewsAsync();

        shopDataList = new List<Users>();

        foreach (var userProduct in userProductLists)
        {
            var aggregatedRating = CalculateAggregatedRating(userProduct.StarReview);
            var shopData = new Users
            {
                ShopProfile = userProduct.ShopProfile,
                ShopName = userProduct.ShopName,
                StarReview = userProduct.StarReview,
                AggregatedRating = aggregatedRating,
                MAIL = userProduct.MAIL // Set the MAIL property to Email property
            };
            shopDataList.Add(shopData);
        }

        // Sort shopDataList by AggregatedRating from highest to lowest
        shopDataList = shopDataList.OrderByDescending(data => data.AggregatedRating).ToList();

        // Assuming 'shopRanking' is your CollectionView in XAML
        shopRanking.ItemsSource = shopDataList;
    }





    private int CalculateAggregatedRating(string starReview)
    {
        // Assuming you have a dictionary to map star reviews to their corresponding scores
        var ratingScores = new Dictionary<string, int>
    {
        { "Excellence", 5 },
        { "Good", 4 },
        { "Average", 3 },
        { "Poor", 2 },
        { "Very Poor", 1 }
    };

        // Initialize total score to zero
        int totalScore = 0;

        // Split the string by comma and trim any whitespace
        var reviews = starReview.Split(',').Select(r => r.Trim());

        // Calculate the total score by summing up the scores of all reviews
        foreach (var review in reviews)
        {
            if (ratingScores.ContainsKey(review))
            {
                // Instead of reinitializing totalScore, accumulate the scores
                totalScore += ratingScores[review];
            }
        }

        return totalScore;
    }





    protected async void OnAppearingDenied()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var deniedApplicationsList = await shops.GetDeniedApplicationsListAsync();
            var deniedApplication = deniedApplicationsList.FirstOrDefault(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (deniedApplication != null)
            {
                await DisplayAlert("Denied Application", $"Denied Reason: {deniedApplication.DeniedReason}", "OK");

                // User is denied, show DeniedImage
                DeniedImage.IsVisible = true;
                VerifiedImage.IsVisible = false;
            }
            else
            {
                // User is no status, 
                VerifiedImage.IsVisible = false;
                DeniedImage.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAppearingDenied: {ex.Message}");
        }
    }


    protected async void OnAppearingVerified()
    {
        try
        {
            base.OnAppearing();

            string userEmail = App.email.ToLower();

            var VerifiedStats = await shops.GetVerifiedStatus();
            var VerifiedStatus = VerifiedStats.FirstOrDefault(app => app.MAIL.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (VerifiedStatus != null)
            {
                // User is verified, show DeniedImage
                DeniedImage.IsVisible = false;
                VerifiedImage.IsVisible = true;
            }
            else
            {
                // User is no status, 
                VerifiedImage.IsVisible = false;
                DeniedImage.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAppearingDenied: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            // Call InitializeAsync and await the result
            string userKey = await InitializeAsync(UserKey);

            // Check if App.key is null or empty
            if (string.IsNullOrEmpty(userKey))
            {
                Console.WriteLine("App.key is null or empty. Unable to proceed.");
                // Handle this case as needed, e.g., show an error message to the user
                return;
            }

            Console.WriteLine($"User key: {userKey}");

            var usersInfo = await shops.GetUsersinfoAsync(userKey);

            if (usersInfo != null)
            {
                username.ItemsSource = usersInfo;
                string profilePictureUrl = usersInfo[0].ProfilePicture;
                // Set the image source
                SetImageSource(profilePictureUrl);
            }
            else
            {
                Console.WriteLine("User information is null. Handle this case if needed.");
                // Handle the case where user information is not available
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"Exception in OnAppearing: {ex.Message}");
        }
    }
    private void SetImageSource(string imageUrl)
    {
        if (!string.IsNullOrEmpty(imageUrl))
        {
            imglogo.Source = new UriImageSource
            {
                Uri = new Uri(imageUrl),
                CachingEnabled = true,
                CacheValidity = TimeSpan.FromDays(1) // Set an appropriate cache validity period
            };
        }
        else
        {
            // Set a default image URL or file path if needed
            imglogo.Source = "account.png";
        }
    }

    private async Task<string> InitializeAsync(string userKey)
    {
        try
        {
            string userEmail = App.email;

            // Use the App.FirebaseService.GetUserKeyByEmail method to get the user key
            string obtainedUserKey = await App.FirebaseService.GetUserKeyByEmail(userEmail);

            // Update UserKey property
            UserKey = obtainedUserKey;

            // Update App.key if needed
            App.key = obtainedUserKey;

            // Return the obtainedUserKey
            return obtainedUserKey;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (log, display, etc.)
            Console.WriteLine($"Error in AppShell initialization: {ex.Message}");
            return null; // Return null or handle the error accordingly
        }
    }


    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue.ToLower(); // Convert search text to lowercase for case-insensitive comparison

        // Filter shop data based on search text
        var filteredShopData = shopDataList.Where(user =>
            user.ShopName.ToLower().Contains(searchText)).ToList();

        // Update CollectionView with filtered shop data
        shopRanking.ItemsSource = filteredShopData;
    }

    private async void ViewShopBTN_Tapped(object sender, TappedEventArgs e)
    {
        var selectedProduct = e.Parameter as Users; // Replace Users with your actual type

        if (selectedProduct != null)
        { 
            string productemail = selectedProduct.MAIL ?? string.Empty;
            string userkey = await App.FirebaseService.GetUserKeyByEmail(productemail);
            await Navigation.PushModalAsync(new ShopProduct(productemail, userkey));
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}