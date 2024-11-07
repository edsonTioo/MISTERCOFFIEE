using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MISTERCOFFIEE.MVVM.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public partial class ClientesViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly Page _page;
        private readonly string? _clienteId;
        public string nombre { get; set; }
        public string cedula { get; set; }
        public int telefono { get; set; }
        public string estado { get; set; }
        public string direccion { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraReservacion { get; set; }
        public TimeSpan HoraFinanReservacion { get; set; }
        [ObservableProperty]
        private Clientes cliente;

        public ClientesViewModel(Page page, string? clienteId = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            _page = page;
            _clienteId = clienteId;
            LoadClienteByIdCommand = new AsyncRelayCommand(LoadClienteByIdAsync);
            CommandoUpdate = new Command<string>(async (id) => await UpdateCliente());
            Task.Run(async () => await LoadClienteByIdAsync()).Wait();
        }
        public IAsyncRelayCommand LoadClienteByIdCommand { get; set; }
        public ICommand CommandoUpdate { get; set; }
        public ObservableCollection<Clientes> clientes { get; } = new ObservableCollection<Clientes>();

        public async Task LoadClienteByIdAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(_clienteId))
                {
                    var response = await _httpClient.GetAsync($"api/ControllerCliente/{_clienteId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Cliente = JsonSerializer.Deserialize<Clientes>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });// aqui en jsonReponse


                    }
                    else
                    {
                        await _page.DisplayAlert("Información", "Cliente no encontrado.", "OK");
                    }
                }
                else
                {
                    await _page.DisplayAlert("Error", "El ID del cliente no es válido.", "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Error", $"Ocurrió un error al cargar el cliente: {ex.Message}", "OK");
            }
        }
        public async Task UpdateCliente()
        {
            var id = Cliente.Id;
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/ControllerCliente/{id}", cliente);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Exito", "Cliente Actualizado", "OK");
                }
                else
                {
                    await _page.DisplayAlert("Error", "No se pudo actualizar la cuenta", "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Excepcion", $"No se pudo actualizar el cliente: {ex.Message}", "OK");
            }
        }
        public async Task CrearReservacion(string id)
        {

            try
            {
                var reservacion = new Clientes
                {
                    Fecha = Fecha,
                    HoraReservacion = HoraReservacion.ToString(@"hh\:mm"),
                    HoraFinanReservacion = HoraFinanReservacion.ToString(@"hh\:mm"),
                };

                var response = await _httpClient.PutAsJsonAsync($"/api/ControllerCliente/actualizarreservacion/{id}", reservacion);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Éxito", "Reservación creada correctamente", "Ok");
                }
                else
                {
                    await _page.DisplayAlert("Error", "No se pudo crear la reservación", "Ok");
                }
            }
            catch (Exception e)
            {
                await _page.DisplayAlert("Excepción", $"No se pudo crear la reservación: {e.Message}", "Ok");
            }
        }
    }
}
