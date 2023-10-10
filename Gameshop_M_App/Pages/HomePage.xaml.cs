using Gameshop_M_App.Pages;
namespace Gameshop_M_App.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void AddProdsBTN_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new HomePage());
    }

  
    
}