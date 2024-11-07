using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;

public partial class PageMesas : ContentPage
{
    public Page page;
	public PageMesas(string token)
	{
		InitializeComponent();
        BindingContext = new ModelMesas(this);
        
    }

    private async void AggProducto(object sender, EventArgs e)
    {
        if (App.Current.MainPage is NavigationPage)
        {
            // Si ya está dentro de NavigationPage, navega normalmente
            await Navigation.PushAsync(new PagePedido(page));
        }
        else
        {
            // Si no está en un NavigationPage, envuélvelo
            App.Current.MainPage = new NavigationPage(new PagePedido(page));
        }
    }
}