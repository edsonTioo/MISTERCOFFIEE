

using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Reporte;

public partial class HomeReporte : ContentPage
{
	public HomeReporte(string token)
    {
		InitializeComponent();
        BindingContext = new ReporteViewModel(token, this);
    }
}