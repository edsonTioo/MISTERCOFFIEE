using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Inventario;

public partial class EditInventario : ContentPage
{
    private InventariosViewModel model;
    private string idinventario;
    public EditInventario(string id)
	{
		InitializeComponent();
        idinventario = id;
        model = new InventariosViewModel(this, id);
        this.BindingContext = model;

        Task.Run(async () =>
        {
            await model.LoadInventario(id);
            // Notificar que los datos han cambiado
            OnPropertyChanged(nameof(model.Inttt));
        });
    }
}