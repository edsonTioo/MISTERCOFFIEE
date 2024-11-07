using MISTERCOFFIEE.MVVM.MODEL;
using MISTERCOFFIEE.MVVM.MODELVIEW;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace MISTERCOFFIEE.MVVM.VIEW.Aut;

public partial class PageRestablecerPassword : ContentPage, INotifyPropertyChanged
{
    private HttpClient _httpClient;
    private string _correo;

    public event PropertyChangedEventHandler PropertyChanged;
    public string Password { get; set; }
    public string Correo
    {
        get => _correo;
        set
        {
            _correo = value;
            OnPropertyChanged();
        }
    }
    public PageRestablecerPassword()
	{
		InitializeComponent();
        BindingContext = this;
        BindingContext = new AuthViewModel(this);
    }

    private async void OnclickVerificarCorreo(object sender, EventArgs e)
    {
        try
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };
            var response = await _httpClient.GetAsync($"api/controller/verificar-correo?correo={Correo}");

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Resultado", "Correo verificado", "OK");
                stacpassword.IsVisible = true; btnverificar.IsVisible = false;
                btnactualizar.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", "No se pudo verificar el correo.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepción", $"Ocurrió una excepción: {ex.Message}", "OK");
        }
    }
    private async void ActualizarContrasena(object sender, EventArgs e)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://10.0.2.2:5002") };

        // Crear el objeto con los datos que queremos enviar en el cuerpo de la solicitud
        var datos = new CambiarPasswordRequest
        {
            Correo = Correo,
            NuevaPassword = Password
        };

        // Usa `PutAsJsonAsync` para enviar los datos en el cuerpo
        var response = await _httpClient.PutAsJsonAsync("/api/controller/cambiar-password", datos);

        if (response.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "Contraseña actualizada correctamente.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la contraseña.", "OK");
        }
    }
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}