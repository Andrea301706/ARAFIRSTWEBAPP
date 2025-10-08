using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace RomanAra.MyfirstWebapp.Pages
{
    public class Posts : PageModel
    {
        private string userId = "1";
        private string id = "1";
        private string title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit";
        private string body = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto";

        public string? ApiResponse { get; set; }

        public async Task OnGet()
        {
            using HttpClient apiClient = new()
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
            };

            using HttpResponseMessage response = await apiClient.GetAsync($"/posts");
            var jsonresponse = await response.Content.ReadAsStringAsync();
            this.ApiResponse = $"{jsonresponse}";
        }
    }
}


