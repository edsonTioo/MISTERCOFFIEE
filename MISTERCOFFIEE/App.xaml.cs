using MISTERCOFFIEE.MVVM.MODEL;
using MISTERCOFFIEE.MVVM.MODELVIEW;
using MISTERCOFFIEE.MVVM.VIEW.Aut;
using MISTERCOFFIEE.MVVM.VIEW.Empleado;
using MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;


namespace MISTERCOFFIEE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new Login());
            // Crear el NavigationPage con la página de inicio de sesión
            var navigationPage = new NavigationPage(new Login());

            // Añadir el botón de "Cerrar sesión" a la barra de navegación
            navigationPage.ToolbarItems.Add(new ToolbarItem
            {
                Text = "Cerrar sesión",
                Command = new Command(async () =>
                {
                    // Obtener el BindingContext de la página actual y ejecutar el comando de cierre de sesión
                    if (navigationPage.CurrentPage.BindingContext is AuthViewModel authViewModel)
                    {
                        await authViewModel.LogoutAsync();
                    }
                }),
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            });

            // Configurar MainPage con el NavigationPage
            MainPage = navigationPage;
        }
    }
}
