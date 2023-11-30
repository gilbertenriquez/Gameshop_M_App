namespace Gameshop_App_Seller.Pages;

public partial class EditProductPage : ContentPage
{
	public EditProductPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {

        //base.OnAppearing();
        //entryFirst.Text = firstname;
        //entryMname.Text = middlename;
        //entryLast.Text = lastname;
        //entryHaddress.Text = homeaddress;
        //entryphone.Text = phone;
        //entryjobpos.Text = jobposition;

    }


    private async void NEXTBTN_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new editInfoPage());
    }

    private async void BTNBACK_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}