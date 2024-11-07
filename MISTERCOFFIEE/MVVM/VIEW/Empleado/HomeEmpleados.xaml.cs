

using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Empleado;

public partial class HomeEmpleados : ContentPage
{
    public EmpleadoViewModel model;
    public HomeEmpleados(string token)
    {
		InitializeComponent();
        model = new EmpleadoViewModel(this, token);
        BindingContext = model;
        model.GetCustomers();
    }
}