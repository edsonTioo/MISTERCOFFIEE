using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Producto;

public partial class CrearProducto : ContentPage
{
    private ProductoViewModel viewModel;
    public CrearProducto()
	{
		InitializeComponent();
        viewModel = new ProductoViewModel(this);
        BindingContext = viewModel;
    }
}