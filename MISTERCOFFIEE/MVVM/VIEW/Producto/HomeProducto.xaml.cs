using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Producto;

public partial class HomeProducto : ContentPage
{
    private ProductoViewModel viewModel;
    public HomeProducto()
	{
		InitializeComponent();
        viewModel = new ProductoViewModel(this); // Asigna el ViewModel al campo de instancia
        BindingContext = viewModel;
        viewModel.GetInventori();
    }
}