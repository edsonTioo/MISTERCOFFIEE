using MISTERCOFFIEE.MVVM.MODEL;
using MISTERCOFFIEE.MVVM.MODELVIEW;
using MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;

public partial class PagePedido : ContentPage
{
	public PagePedido(Page page)
	{
		InitializeComponent();
        BindingContext = new ModelMesas(page);
        //BindingContext = new AuthViewModel(this);
    }
}