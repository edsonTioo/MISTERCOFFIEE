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
    public partial class InventariosViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly Page _page;
        private string? idinventario;
        private string nombre;
        private int cantidad;
        private string descri;
        [ObservableProperty]
        private Inventario inttt;
        public InventariosViewModel(Page page, string? InventarioId = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            _page = page;
            idinventario = InventarioId;
            //LoadIventario = new AsyncRelayCommand(UpdateCliente());
            Task.Run(async () => await LoadInventario(InventarioId)).Wait();
            IcommandUpdate = new Command<string>(async (id) => await UpdateCliente());
        }
        public IAsyncRelayCommand LoadIventario { get; set; }
        public ICommand IcommandUpdate { get; set; }
        public ObservableCollection<Inventario> inventarioList { get; set; } = new ObservableCollection<Inventario>();
        public async Task LoadInventario(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var response = await _httpClient.GetAsync($"/api/ControllerInventario/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonReponse = await response.Content.ReadAsStringAsync();
                        inttt = JsonSerializer.Deserialize<Inventario>(jsonReponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });// aqui en jsonReponse}
                    }
                    else
                    {
                        await _page.DisplayAlert("Información", "Producto no encontrado.", "OK");
                    }
                }
                else
                {
                    await _page.DisplayAlert("Error", "El ID del producto no es válido.", "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Error", $"Ocurrió un error al cargar el producto: {ex.Message}", "OK");
            }
        }
        public async Task UpdateCliente()
        {
            var id = inttt.Id;
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/ControllerInventario/{id}", inttt);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Exito", "Inventario Actualizado", "OK");
                }
                else
                {
                    await _page.DisplayAlert("Error", "No se pudo actualizar la cuenta", "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Excepcion", $"No se pudo actualizar el inventario: {ex.Message}", "OK");
            }
        }
    }
}
