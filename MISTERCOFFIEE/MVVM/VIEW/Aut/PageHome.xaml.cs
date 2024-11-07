using MISTERCOFFIEE.MVVM.MODELVIEW;
using MISTERCOFFIEE.MVVM.MODEL;
using MISTERCOFFIEE.MVVM.VIEW.Cliente;
using MISTERCOFFIEE.MVVM.VIEW.Reporte;
using MISTERCOFFIEE.MVVM.VIEW.Inventario;
using MISTERCOFFIEE.MVVM.VIEW.Empleado;
using MISTERCOFFIEE.MVVM.VIEW.Perfil;
using MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;
using MISTERCOFFIEE.MVVM.VIEW.Cocina;
using MISTERCOFFIEE.MVVM.VIEW.Producto;
namespace MISTERCOFFIEE.MVVM.VIEW.Aut;

public partial class PageHome : ContentPage
{
    private readonly Page _page;
    private readonly string _token;
    public PageHome(string token)
    {
		InitializeComponent();
        BindingContext = new HomeViewModel(this, token);
        BindingContext = new AuthViewModel(this);
    }
    private async void OnButtonConcina(object sender, EventArgs e)
    {
        // Navegar a la página de reportes y pasar el token
        await Navigation.PushAsync(new HomeCocina());
    }
    private async void OnReporteButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la página de reportes y pasar el token
        await Navigation.PushAsync(new HomeReporte(_token));
    }
    private async void OnProductoButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomeProducto());
    }
    private async void OnEmpleadoButton(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomeEmpleados(_token));
    }
    private async void OnClienteButton(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ClienteHome(_token));
    }
    private async void OnMesasButton(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageMesas(_token));
    }
    private async void OnCocina(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageMesas(_token));
    }
    private async void OnHomeMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageHome(_token));
    }
    private async void OnPerfilEmpleado(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PagePerfil(_token));
    }
    private async void OnAcercadeClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Acercade(_token));
    }
}