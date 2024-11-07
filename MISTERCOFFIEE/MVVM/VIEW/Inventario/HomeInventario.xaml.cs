using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Inventario;

public partial class HomeInventario : ContentPage
{
    private InventarioViewModel model_;
    public HomeInventario(string token)
	{
		InitializeComponent();
        // Inicializar el ViewModel
        model_ = new InventarioViewModel(this);

        // Asignar el BindingContext
        BindingContext = model_;

        // Ejecutar el método para cargar los inventarios
        model_.GetInventori();
    }
}