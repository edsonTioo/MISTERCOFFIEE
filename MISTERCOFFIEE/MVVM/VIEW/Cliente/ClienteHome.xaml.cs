using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Cliente;

public partial class ClienteHome : ContentPage
{
    private ClienteViewModel viewModel;
    public ClienteHome(string token)
	{
		InitializeComponent();
        viewModel = new ClienteViewModel(this); // Asigna el ViewModel al campo de instancia
        BindingContext = viewModel;
        viewModel.GetCustomers();
    }
}