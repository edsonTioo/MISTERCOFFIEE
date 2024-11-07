using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public class ReporteViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly Page _Page;
        private readonly string _token;
        public ReporteViewModel(string token, Page page)
        {
            _Page = page;
            _token = token;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:5002");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }
}
