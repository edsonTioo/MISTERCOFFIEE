using MISTERCOFFIEE.MVVM.MODELVIEW;

namespace MISTERCOFFIEE.MVVM.VIEW.Cocina;

public partial class HomeCocina : ContentPage
{
    private CocinaViewModel CocinaViewModel { get; set; }
    public HomeCocina()
	{
		InitializeComponent();
        CocinaViewModel = new CocinaViewModel(this);
        BindingContext = CocinaViewModel;
    }
}