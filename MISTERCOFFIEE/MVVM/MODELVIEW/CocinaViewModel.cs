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
    public partial class CocinaViewModel : ObservableObject
    {
        public ObservableCollection<Mesas> ListMesas { get; set; } = new ObservableCollection<Mesas>();
        [ObservableProperty]
        private Mesas mesas;
        public Page _page;
        public readonly HttpClient _httpClient;
        public CocinaViewModel(Page page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(Page)); _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            IrADetalleMesaCommand = new Command<string>(async (mesaId) => await NavegarADetalleMesa(mesaId));
            GetCocina();
        }
        public Command GetCommand { get; set; }
        public Command IrADetalleMesaCommand { get; set; }
        public async Task GetCocina()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/ControllerMesas");
                response.EnsureSuccessStatusCode();
                var cocina = await response.Content.ReadFromJsonAsync<List<Mesas>>();
                if (cocina != null)
                {
                    ListMesas.Clear();
                    foreach (var item in cocina)
                    {
                        ListMesas.Add(item);
                    }
                }
                else
                {
                    await _page.DisplayAlert("informacino", "No hay pedidos para mostrar.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Excepcion", $"Error al cargar los datos{ex}", "ok");
            }
        }
        private async Task NavegarADetalleMesa(string mesaId)
        {
            await _page.DisplayAlert("Navegación", $"Mesa seleccionada con ID: {mesaId}", "OK");
            var detallePage = new MesaDetallePage
            {
                BindingContext = new MesaDetalleViewModel(mesaId, _page)
            };
            await _page.Navigation.PushAsync(detallePage);
        }
    }
}
