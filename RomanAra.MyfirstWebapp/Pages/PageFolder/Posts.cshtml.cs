using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RomanAra.MyfirstWebapp.Pages
{
    public class Posts : PageModel
    {
        public List<UserDto>? ApiResponse { get; set; }  // must be public with getter/setter

        public async Task OnGet()
        {
            using HttpClient apiClient = new()
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
            };

            var response = await apiClient.GetAsync("/posts");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                ApiResponse = JsonConvert.DeserializeObject<List<UserDto>>(jsonResponse);
            }
            else
            {
                ApiResponse = new List<UserDto>(); // or handle errors as needed
            }
        }
    }

    public class UserDto
    {
        public string userId { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}
