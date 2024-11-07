using MISTERCOFFIEE.MVVM.VIEW.Aut;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MISTERCOFFIEE.MVVM.VIEW;

namespace MISTERCOFFIEE.MVVM.MODELVIEW
{
    public class HomeViewModel: ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly Page _page;
        private readonly string _token;
        public HomeViewModel(Page page, string token)
        {
            _httpClient = new HttpClient();
            _page = page;
            _token = token;
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:5002");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
