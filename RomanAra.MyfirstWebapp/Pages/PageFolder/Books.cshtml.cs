using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RomanAra.MyfirstWebapp.Pages
{
    public class Books : PageModel
    {
        public List<BookDto>? ApiResponse { get; set; }  // must be public with getter/setter

        public async Task OnGet()
        {
            using HttpClient apiClient = new()
            {
                BaseAddress = new Uri("https://openlibrary.org/search.json?q=harry+potter")
            };

            var response = await apiClient.GetAsync("/posts");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                ApiResponse = JsonConvert.DeserializeObject<List<BookDto>>(jsonResponse);
            }
            else
            {
                ApiResponse = new List<BookDto>(); // or handle errors as needed
            }
        }
    }

    public class BookDto
    {
        public List<string> AuthorKey { get; set; } = new List<string>();
        public List<string> AuthorName { get; set; } = new List<string>();
        public string CoverEdition { get; set; } = "";
            public int FirstPublishYear { get; set; }
            public string BookAccess { get; set; } = "";
            public int EditionCount { get; set; }
            public List<string> Fulltext { get; set; } = new List<string>();
            public List<string> LendingEdition { get; set; } = new List<string>();
            public List<string> LendingIdentifier { get; set; } = new List<string>();
    }
}

