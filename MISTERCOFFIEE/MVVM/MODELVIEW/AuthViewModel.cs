using CommunityToolkit.Mvvm.ComponentModel;
using MISTERCOFFIEE.MVVM.VIEW.Aut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MISTERCOFFIEE.MVVM.MODEL;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly Page _Page;
        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public AuthViewModel(Page page)
        {
            _httpClient = new HttpClient();
            _Page = page;
            LoginCommand = new Command(async () => await LoginAsync());
            LogoutCommand = new Command(async () => await LogoutAsync());
        }
        [ObservableProperty]
        private string correo;
        [ObservableProperty]
        private string password;
        public async Task LoginAsync()
        {
            var user = new User
            {
                Correo = correo,
                Password = password
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://10.0.2.2:5002/api/controller/login", user);
                if (response.IsSuccessStatusCode)
                {
                    var JsonReponse = await response.Content.ReadAsStringAsync();
                    var tokenObje = JsonSerializer.Deserialize<TokenResponse>(JsonReponse);
                    
                        var token = tokenObje.Token;
                        await _Page.Navigation.PushAsync(new PageHome(token));
                }
                else
                {
                    await _Page.DisplayAlert("Error", "Error en el login", "OK");
                }

            }
            catch (Exception ex)
            {
                await _Page.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
        public async Task LogoutAsync()
        {
            // Aquí puedes limpiar cualquier dato de sesión si es necesario
            await _Page.DisplayAlert("Cerrar sesión", "Se ha cerrado la sesión exitosamente.", "OK");
            await _Page.Navigation.PopToRootAsync(); // Regresa a la página raíz (inicio de sesión)
        }
    }
}
