using Gameshop_M_App.Pages;
namespace Gameshop_M_App.Pages;

public partial class WelcomePage : ContentPage
{
    private List<string> AllImage;
    private Random rnd = new Random();

	public List<string> image1 {get; set; }
    public List<string> image2 { get; set; }
    public List<string> image3 { get; set; }
    public List<string> image4 { get; set; }

    public WelcomePage()
	{
		InitializeComponent();

        GenerateData();

        image1 = Randomize(AllImage); 
        image2 = Randomize(AllImage);
        image3 = Randomize(AllImage);
        image4 = Randomize(AllImage);

        BindingContext = this;
	}

    public void GenerateData()
    {
        AllImage = new();
            for (var i =1; i <=4; i++)
        {
            AllImage.Add($"image{i.ToString("0")}.jpg");
        }
           
    }
    public List<T> Randomize<T>(List<T> source) =>
        source.OrderBy<T, int>((item) => rnd.Next()).ToList();


    private async void GetStart_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new SellerBuyerPage());
        
    }
}