using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MISTERCOFFIEE.MVVM.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public partial class CocinasViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly Page _page;
        public string idcocina;
        [ObservableProperty]
        private Mesas mesas;
        public CocinasViewModel(Page page, string idCocina = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            _page = page;
            idCocina = idcocina;
            LoadCocinaByIdCommand = new AsyncRelayCommand(LoadCocinaAsync);
        }
        public IAsyncRelayCommand LoadCocinaByIdCommand { get; set; }
        public Command AceptarOrdenes { get; set; }
        public Command TerminarOrdenes { get; set; }
        public async Task LoadCocinaAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(idcocina))
                {
                    var response = await _httpClient.GetAsync($"api/ControllerMesas/{idcocina}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        mesas = JsonSerializer.Deserialize<Mesas>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    else { await _page.DisplayAlert("Información", "Los Pedidos no sean encontrado.", "Ok"); }
                }
                else
                {
                    await _page.DisplayAlert("Error", "El id de la mesas no es valido.", "OK");
                }
            }
            catch (Exception ex) { await _page.DisplayAlert("Excepcion", $"Excepcion {ex.Message}", "OK"); }
        }
        public async Task AceptarOrden()
        {
            try
            {
                var estadoorden = new Mesas
                {
                    Estado = "Orden Aceptada"
                };
                var response = await _httpClient.PutAsJsonAsync($"api/ControllerMesas/{idcocina}", mesas);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Se acepto el pedido", $"Se acepto el pedido de le mesa perteneciente a la id:{idcocina}", "OK");
                }
                else
                {
                    await _page.DisplayAlert("Error", "Ocurrio un error al aceptar el pedido.", "OK");
                }
            }
            catch (Exception e)
            {
                await _page.DisplayAlert("Excepcion", $"La excepcion identificada:{e.Message}", "Ok");
            }
        }

        public async Task TerminarOrden()
        {
            try
            {
                var estadoorden = new Mesas
                {
                    Estado = "Orden Terminada"
                };
                var response = await _httpClient.PutAsJsonAsync($"api/ControllerMesas/{idcocina}", mesas);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Se acepto el pedido", $"Se acepto el pedido de le mesa perteneciente a la id:{idcocina}", "OK");
                }
                else
                {
                    await _page.DisplayAlert("Error", "Ocurrio un error al aceptar el pedido.", "OK");
                }
            }
            catch (Exception e)
            {
                await _page.DisplayAlert("Excepcion", $"La excepcion identificada:{e.Message}", "Ok");
            }
        }
    }
}
