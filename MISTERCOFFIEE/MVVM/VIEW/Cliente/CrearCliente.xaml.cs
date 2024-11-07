using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Cliente;

public partial class CrearCliente : ContentPage
{
    private ClienteViewModel _lienteViewModel;
    public CrearCliente()
	{
		InitializeComponent();
        _lienteViewModel = new ClienteViewModel(this); // Asigna el ViewModel al campo de instancia
        BindingContext = _lienteViewModel;
    }
}