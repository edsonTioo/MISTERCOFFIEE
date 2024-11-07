using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Cliente;

public partial class EditCliente : ContentPage
{
    private readonly string? _cliente;
    private readonly ClientesViewModel _viewModel;
    public EditCliente(string clienteId)
	{
		InitializeComponent();
        _cliente = clienteId;
        this.BindingContext = new ClientesViewModel(this, _cliente);
    }
}