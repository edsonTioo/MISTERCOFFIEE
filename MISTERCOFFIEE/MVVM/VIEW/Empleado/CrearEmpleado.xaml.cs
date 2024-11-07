

using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Empleado;

public partial class CrearEmpleado : ContentPage
{
    public EmpleadoViewModel model;
    public string _token;
    public CrearEmpleado(string token)
    {
		InitializeComponent();
        token = _token;
        model = new EmpleadoViewModel(this,_token);
        BindingContext = model;
    }
}