namespace Gameshop_App_Seller.Pages;

public partial class TermsAndConditionPage : ContentPage
{
	public TermsAndConditionPage()
	{

        InitializeComponent();
    }

	private async void btnBackImg_Clicked(object sender, EventArgs e)
	{
		
		await Navigation.PopModalAsync();
        
    }


    
}