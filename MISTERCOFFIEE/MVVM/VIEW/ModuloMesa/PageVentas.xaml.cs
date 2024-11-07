using MISTERCOFFIEE.MVVM.MODEL;
using MISTERCOFFIEE.MVVM.MODELVIEW;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;

namespace MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;

public partial class PageVentas : ContentPage
{
    private ObservableCollection<Productos> Productos2;
    public Page page;
    private ModelMesas model;
    public PageVentas(ObservableCollection<Productos> productos)
	{
		InitializeComponent();
        Productos2 = productos;
        BindingContext = productos;
        BindingContext = new ModelMesas(page);
    }
    private async void AggPro(object sender, EventArgs e)
    {
        // Navegar a la p�gina PagePedido de manera as�ncrona
        await Navigation.PushAsync(new PagePedido(page));
    }
    // M�todo que se llama al hacer clic en el bot�n
    public void OnCalcularButtonClicked(object sender, EventArgs e)
    {
        CalcularTotal();
    }
    public void CalcularTotal()
    {
        if (Productos2 == null || TotalLabel == null)
        {
            Console.WriteLine("Productos2 o TotalLabel no est�n inicializados.");
            return;
        }

        decimal total = 0;
        decimal subtotal = 0;

        foreach (var producto in Productos2)
        {
            producto.SubTotal = producto.Cantidad * producto.Precio_venta;
            total += producto.Cantidad * producto.Precio_venta;
        }
        //SubTotalLabel.Text = $"{subtotal}";
        TotalLabel.Text = $"{total}";
        
    }
}