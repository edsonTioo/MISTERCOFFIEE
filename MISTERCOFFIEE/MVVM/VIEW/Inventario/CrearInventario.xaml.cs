using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Inventario;

public partial class CrearInventario : ContentPage
{
    private InventarioViewModel _lienteViewModel;
    public CrearInventario()
	{
		InitializeComponent();
        _lienteViewModel = new InventarioViewModel(this);
        BindingContext = _lienteViewModel;
    }
}