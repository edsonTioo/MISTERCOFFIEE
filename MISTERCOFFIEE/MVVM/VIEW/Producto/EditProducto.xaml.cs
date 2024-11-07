using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Producto;

public partial class EditProducto : ContentPage
{
    private ProductosViewModel viewModel;
    private string IventarioID;
    public EditProducto(string id)
	{
		InitializeComponent();
        IventarioID = id;
        viewModel = new ProductosViewModel(this, id);
        this.BindingContext = viewModel;

        Task.Run(async () =>
        {
            await viewModel.LoadProduct(id);
            // Notificar que los datos han cambiado
            OnPropertyChanged(nameof(viewModel.Ventarioss));
        });
    }
}