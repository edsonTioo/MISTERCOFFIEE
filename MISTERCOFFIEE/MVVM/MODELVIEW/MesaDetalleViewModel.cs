using CommunityToolkit.Mvvm.ComponentModel;
using MISTERCOFFIEE.MVVM.MODEL;
using MISTERCOFFIEE.MVVM.VIEW.Cocina;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public class MesaDetalleViewModel : ObservableObject
    {
        public ObservableCollection<Pedido> PedidosMesa { get; set; } = new ObservableCollection<Pedido>();
        private readonly HttpClient _httpClient;
        private readonly Page _page;

        public MesaDetalleViewModel(string mesaId, Page page)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            _page = page;
            LoadPedidosMesa(mesaId);
            AceptarCommand = new Command<string>(async (id) => await AceptarOrden(mesaId));
            TerminarCommand = new Command<string>(async (id) => await TerminarOrden(mesaId));
        }
        public Command AceptarCommand { get; set; }
        public Command TerminarCommand { get; set; }
        private async void LoadPedidosMesa(string mesaId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/ControllerEstadoMesa/{mesaId}/pedidos");
                response.EnsureSuccessStatusCode();
                var pedidos = await response.Content.ReadFromJsonAsync<List<Pedido>>();
                if (pedidos != null)
                {
                    PedidosMesa.Clear();
                    foreach (var pedido in pedidos)
                    {
                        PedidosMesa.Add(pedido);
                    }
                }
                else
                {
                    await _page.DisplayAlert("Información", "No hay pedidos para esta mesa.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Excepción", $"Error al cargar los pedidos: {ex.Message}", "Ok");
            }
        }
        public async Task AceptarOrden(string id)
        {
            try
            {
                var estadoorden = new Mesas
                {
                    Estado = "Orden Aceptada"
                };
                var response = await _httpClient.PutAsJsonAsync($"api/ControllerMesas/{id}", estadoorden);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Se acepto el pedido", $"Se acepto el pedido de le mesa perteneciente a la id:{id}", "OK");
                }
                var existingResponse = await _httpClient.GetFromJsonAsync<Mesas>($"api/ControllerMesas/{id}");
                if (existingResponse == null)
                {
                    await _page.DisplayAlert("Error", "No se encontró la mesa especificada.", "OK");
                    return;
                }

                // Update only the `Estado` field
                existingResponse.Estado = "Orden Aceptada";

                // Send the updated object with the full `PUT` request
                response = await _httpClient.PutAsJsonAsync($"api/ControllerMesas/{id}", existingResponse);

                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Se aceptó el pedido", "Se aceptó el pedido de la mesa ", "OK");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await _page.DisplayAlert("Error", $"Ocurrió un error al aceptar el pedido. Detalles: {errorContent}", "OK");
                }
            }
            catch (Exception e)
            {
                await _page.DisplayAlert("Excepcion", $"La excepcion identificada:{e.Message}", "Ok");
            }
        }
        public async Task TerminarOrden(string id)
        {
            try
            {
                var estadoorden = new Mesas
                {
                    Estado = "Orden Terminada"
                };
                var response = await _httpClient.PutAsJsonAsync($"api/ControllerMesas/{id}", estadoorden);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Se termino el pedido", $"Se acepto el pedido de le mesa perteneciente a la id:{id}", "OK");
                }
                var existingResponse = await _httpClient.GetFromJsonAsync<Mesas>($"api/ControllerMesas/{id}");
                if (existingResponse == null)
                {
                    await _page.DisplayAlert("Error", "No se encontró la mesa especificada.", "OK");
                    return;
                }

                // Update only the `Estado` field
                existingResponse.Estado = "Orden Terminada";

                // Send the updated object with the full `PUT` request
                response = await _httpClient.PutAsJsonAsync($"api/ControllerMesas/{id}", existingResponse);

                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Se termino el pedido", $"Se aceptó el pedido de la mesa", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new HomeCocina());
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await _page.DisplayAlert("Error", $"Ocurrió un error al terminar el pedido. Detalles: {errorContent}", "OK");
                }
            }
            catch (Exception e)
            {
                await _page.DisplayAlert("Excepcion", $"La excepcion identificada:{e.Message}", "Ok");
            }
        }
    }
}
