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
    public partial class EmpleadosViewModel : ObservableObject
    {
        public ObservableCollection<Empleado> EmpleadosList { get; set; } = new ObservableCollection<Empleado>();
        private readonly HttpClient _httpClient;
        private readonly Page _page;

        // Declaramos las propiedades como observables para que puedan actualizarse correctamente en la vista
        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private int telefono;

        [ObservableProperty]
        private string correo;

        [ObservableProperty]
        private string estado;

        [ObservableProperty]
        private int pago;

        [ObservableProperty]
        private string cedula;

        [ObservableProperty]
        private DateTime fechaNacimiento;

        [ObservableProperty]
        private string direccion;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string rol;

        [ObservableProperty]
        private Empleado empleados;

        public EmpleadosViewModel(Page page, string? empleadoId = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            _page = page;
            LoadEmpleadosCommand = new AsyncRelayCommand<string>(async (empleadoId) => await LoadEmpleado(empleadoId));
            CommandActualizar = new Command<string>(async (id) => await UpdateEmpleado());

        }

        public IAsyncRelayCommand LoadEmpleadosCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand CommandActualizar { get; set; }
        public async Task LoadEmpleado(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var response = await _httpClient.GetAsync($"/api/ControllerEmpleados/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Empleados = JsonSerializer.Deserialize<Empleado>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (Empleados != null)
                        {
                            // Asignamos las propiedades individuales desde Empleados
                            nombre = Empleados.Nombre;
                            cedula = Empleados.Cedula;
                            telefono = Empleados.Telefono;
                            estado = Empleados.Estado;
                            direccion = Empleados.Direccion;
                            correo = Empleados.Correo;
                            pago = Empleados.Pago;
                            fechaNacimiento = Empleados.FechaNacimiento;
                            password = Empleados.Password;
                            rol = Empleados.Rol;
                        }
                    }
                    else
                    {
                        await _page.DisplayAlert("Información", "Empleado no encontrado.", "OK");
                    }
                }
                else
                {
                    await _page.DisplayAlert("Error", "El ID del empleado no es válido.", "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Error", $"Ocurrió un error al cargar el empleado: {ex.Message}", "OK");
            }
        }

        public async Task UpdateEmpleado()
        {
            var id = Empleados.Id;
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/ControllerEmpleados/{id}", empleados);
                if (response.IsSuccessStatusCode)
                {
                    await _page.DisplayAlert("Exito", "Empleado Actualizado", "OK");
                }
                else
                {
                    await _page.DisplayAlert("Error", "No se pudo actualizar la cuenta", "OK");
                }
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("Excepcion", $"No se pudo actualizar el empleado: {ex.Message}", "OK");
            }
        }
    }
}
