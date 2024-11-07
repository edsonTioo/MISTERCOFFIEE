using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Input;
using MISTERCOFFIEE.MVVM.MODEL;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MISTERCOFFIEE.MVVM.VIEW.ModuloMesa;
using MISTERCOFFIEE.MVVM.VIEW;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public partial class ModelMesas : ObservableObject
    {
        private readonly HttpClient _httpClient;
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ObservableCollection<Productos> Productos { get; } = new ObservableCollection<Productos>();
        public ObservableCollection<Productos> productosSeleccionados { get;} = new ObservableCollection<Productos>();
        public ObservableCollection<Mesas> ListMesas { get; } = new ObservableCollection<Mesas>();
        public ICommand CrearCommand { get; set; }
        public ICommand AgregarProductoCommand => new RelayCommand<Productos>((producto) => AgregarProducto(producto));
        public ICommand ConfirmarCompraCommand => new RelayCommand(async () => await ConfirmarCompra());
        public ICommand EliminarProductoCommand => new RelayCommand<Productos>((producto) => EliminarProducto(producto));
        public readonly Page _page;

        public decimal Total { get; set; }
        [ObservableProperty]
        public string producto;
        [ObservableProperty]
        public decimal precioventa;
        [ObservableProperty]
        public int cantidad;

        public decimal subtotal;
        [ObservableProperty]
        public DateTime fecha;
        [ObservableProperty]
        private Mesas mesas;
        public ModelMesas(Page page)
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://10.0.2.2:5002")};
            _page = page;
            LoadAsyncProducto();
            EliminarCommand = new Command<string>(async (id) => await EliminarMesasAsync(id));
            CrearCommand = new Command<string>(async (id) => await Update(id));
            CrearMesasCommand = new Command(async () => await Crearemesas());
            GetMesasCommand = new Command(async () => await GetMesas());
            GetMesas();
        }
        public ICommand EliminarCommand { get; set; }
        public ICommand CrearMesasCommand { get; set; }
        public async Task Crearemesas()
        {
            try
            {
                // Primero, obtenemos la lista actual de mesas para encontrar el último número de mesa
                var response = await _httpClient.GetAsync("api/ControllerMesas");
                response.EnsureSuccessStatusCode();
                var mesas = await response.Content.ReadFromJsonAsync<List<Mesas>>();

                // Determinamos el próximo número de mesa
                int nuevoNumeroMesa = 1; // Iniciar en 1 si no hay mesas existentes
                if (mesas != null && mesas.Count > 0)
                {
                    nuevoNumeroMesa = mesas.Max(m => m.NumeroMesa) + 1; // Incrementar el último número de mesa
                }

                // Crear un nuevo objeto de mesa
                var nuevaMesa = new Mesas
                {
                    NumeroMesa = nuevoNumeroMesa,
                    Estado = "Activo", // Estado inicial
                    Cliente = "", // Asignar el cliente
                    Reservacion = 1, // Estado de reservación
                    Fecha = DateTime.Now,
                    Pedidos = new List<Pedido>() // Inicializar la lista de pedidos vacía
                };

                // Enviar la nueva mesa a la API
                var createResponse = await _httpClient.PostAsJsonAsync("/api/ControllerMesas", nuevaMesa);
                createResponse.EnsureSuccessStatusCode();

                Debug.WriteLine($"Mesa creada: Número de mesa {nuevaMesa.NumeroMesa}, Estado: {nuevaMesa.Estado}");
                await Application.Current.MainPage.DisplayAlert("Mesa Creada", $"Mesa número {nuevaMesa.NumeroMesa} creada exitosamente.", "OK");
                GetMesas();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error al crear la mesa: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al crear la mesa.", "OK");
            }
        }
        public Command GetMesasCommand { get; set; }
        public async Task GetMesas()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/ControllerMesas");
                response.EnsureSuccessStatusCode();
                var mesas = await response.Content.ReadFromJsonAsync<List<Mesas>>();
                if (mesas != null)
                {
                    ListMesas.Clear();
                    foreach (var Mesas in mesas) { ListMesas.Add(Mesas); }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Información", "No hay mesas para mostrar.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar las Mesas: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", $"No se pudo cargar las Mesas: {ex.Message}", "OK");
            }
        }

        public async Task Update(string id)
        {
            var fecha = DateTime.Now;
            try
            {
                var list = new List<Pedido>();
                foreach (var productoSeleccionado in productosSeleccionados)
                {
                    var pedido = new Pedido
                    {
                        Producto = productoSeleccionado.Producto,
                        PrecioVenta = productoSeleccionado.Precio_venta,
                        Cantidad = productoSeleccionado.Cantidad,
                        Subtotal = productoSeleccionado.SubTotal
                    };
                    list.Add(pedido);
                }
                var nuevaMesa = new Mesas
                {
                    Fecha = fecha,
                    Total = Total,
                    NumeroMesa = 1,
                    Estado = "Activo",
                    Cliente = "",
                    Reservacion = 1,
                    Pedidos = list
                };

                // Envía la solicitud PUT
                var response = await _httpClient.PutAsJsonAsync($"/api/ControllerMesas/{id}", nuevaMesa);

                // Verifica si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Exito", "Empleado Actualizado", "OK");
                }
                else
                {
                    // Muestra el mensaje de error de la respuesta
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await App.Current.MainPage.DisplayAlert("Error", $"Error al actualizar: {errorContent}", "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"Error en la solicitud: {httpEx.Message}");
                await App.Current.MainPage.DisplayAlert("Error", $"Error en la solicitud: {httpEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al actualizar la Mesa: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar la Mesa: {ex.Message}", "OK");
            }
        }


        public async Task LoadAsyncProducto()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/ControllerProductos");
                response.EnsureSuccessStatusCode();
                var productos = await response.Content.ReadFromJsonAsync<List<Productos>>();
                if (productos != null)
                {
                    Productos.Clear();
                    foreach (var producto in productos)
                    {
                        Productos.Add(producto);
                        Debug.WriteLine($"Producto agregado: {producto.Producto} - Precio: {producto.Precio_venta}");
                    }
                }
                else
                {
                    Debug.WriteLine("No se encontraron productos.");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"");
            }
        }

        private async void AgregarProducto(Productos producto)
        {
            if (producto == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Producto es nulo. No se puede agregar.", "OK");
                return;
            }

            // Verifica que el producto no esté duplicado en productosSeleccionados
            if (!productosSeleccionados.Any(p => p.Producto == producto.Id))
            {
                // Crea una nueva instancia del producto
                var nuevoProductoSeleccionado = new Productos
                {
                    Producto = producto.Producto,
                    Precio_venta = producto.Precio_venta

                };
                productosSeleccionados.Add(nuevoProductoSeleccionado);
                await Application.Current.MainPage.DisplayAlert("Producto agregado", $"Producto: {producto.Producto} - Precio: {producto.Precio_venta}", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Producto duplicado", $"El producto {producto.Producto} ya está en la lista.", "OK");
            }
        }


        private async Task ConfirmarCompra()
        {
            if (productosSeleccionados.Count == 0)
            {
                // Muestra un mensaje al usuario si no hay productos seleccionados
                await Application.Current.MainPage.DisplayAlert("Advertencia", "No se ha seleccionado ningún producto.", "OK");
                return;
            }

            // Muestra un mensaje de confirmación antes de navegar a la página de ventas
            bool confirmar = await Application.Current.MainPage.DisplayAlert("Confirmar Compra",
                $"Está a punto de confirmar la compra de {productosSeleccionados.Count} productos. ¿Desea continuar?",
                "Sí", "No");

            if (confirmar)
            {
                Debug.WriteLine($"Navegando a la página con {productosSeleccionados.Count} productos.");
                // Navega a la página de ventas y pasa la lista de productos como parámetro
                await Application.Current.MainPage.Navigation.PushAsync(new PageVentas(productosSeleccionados));
            }
            else
            {
                Debug.WriteLine("El usuario canceló la compra.");
            }
        }

        public async Task EliminarMesasAsync(string id)
        {
            System.Diagnostics.Debug.WriteLine($"ID del mesa a eliminar: {id}"); // Verifica el ID
            bool confirm = await App.Current.MainPage.DisplayAlert("Confirmar", "¿Desea eliminar esta Mesa?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"/api/ControllerMesas/{id}");
                    response.EnsureSuccessStatusCode();

                    await App.Current.MainPage.DisplayAlert("Éxito", "Mesa eliminado correctamente", "OK");
                    GetMesas();
                }
                catch (HttpRequestException httpEx)
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"No se pudo eliminar el mesa: {httpEx.Message}", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"No se pudo eliminar el mesa: {ex.Message}", "OK");
                }
            }
        }
        private async void EliminarProducto(Productos producto)
        {
            if (producto != null)
            {
                // Eliminar el producto de la colección
                productosSeleccionados.Remove(producto);
                await Application.Current.MainPage.DisplayAlert("Producto eliminado", $"Producto: {producto.Producto} ha sido eliminado de la lista.", "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new PageVentas(productosSeleccionados));
            }
            else
            {
                // Si el producto no se encuentra en la lista, mostrar mensaje de error
                await Application.Current.MainPage.DisplayAlert("Error", "Producto no encontrado.", "OK");
            }
        }
    }
}

