using MISTERCOFFIEE.MVVM.MODELVIEW;
namespace MISTERCOFFIEE.MVVM.VIEW.Aut;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = new AuthViewModel(this);
    }
}