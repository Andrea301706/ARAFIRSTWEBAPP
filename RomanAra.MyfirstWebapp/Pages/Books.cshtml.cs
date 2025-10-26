using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RomanAra.MyfirstWebapp.Pages
{
    public class BooksModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string OpenLibraryBase = "https://openlibrary.org";

        public BooksModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Book> ApiResponse { get; set; } = new List<Book>();
        public string SearchTerm { get; set; } = "Harry Potter";
        public string SearchField { get; set; } = "q"; // q | title | author
        public bool HasFulltext { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync([FromQuery] string? searchTerm, [FromQuery] string? searchField, [FromQuery] bool? hasFulltext)
        {
            ErrorMessage = null;
            SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? SearchTerm : searchTerm!;
            SearchField = string.IsNullOrWhiteSpace(searchField) ? SearchField : searchField!;
            HasFulltext = hasFulltext ?? HasFulltext;

            var client = _httpClientFactory.CreateClient();
            try
            {
                // Build fielded query depending on selection
                string query;
                var esc = Uri.EscapeDataString(SearchTerm);
                if (SearchField == "title") query = $"title={esc}";
                else if (SearchField == "author") query = $"author={esc}";
                else query = $"q={esc}";

                if (HasFulltext) query += "&has_fulltext=true";

                var res = await client.GetAsync($"{OpenLibraryBase}/search.json?{query}");
                if (!res.IsSuccessStatusCode)
                {
                    ErrorMessage = $"API returned {(int)res.StatusCode} {res.ReasonPhrase}";
                    ApiResponse = new List<Book>();
                    return;
                }

                var json = await res.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("docs", out var docs) || docs.ValueKind != JsonValueKind.Array)
                {
                    ErrorMessage = "Unexpected API response format.";
                    ApiResponse = new List<Book>();
                    return;
                }

                var list = new List<Book>();
                foreach (var item in docs.EnumerateArray())
                {
                    try
                    {
                        var b = new Book
                        {
                            AuthorKey = GetStringFromArray(item, "author_key", 0),
                            AuthorName = GetStringFromArray(item, "author_name", 0),
                            CoverEdition = item.TryGetProperty("cover_edition_key", out var ck) ? ck.GetString() ?? string.Empty : string.Empty,
                            FirstPublishYear = item.TryGetProperty("first_publish_year", out var fy) && fy.TryGetInt32(out var y) ? y : 0,
                            BookAccess = item.TryGetProperty("lending_identifier_s", out _) ? "Available" : "Not Available",
                            EditionCount = item.TryGetProperty("edition_count", out var ec) && ec.TryGetInt32(out var c) ? c : 0,
                            Fulltext = item.TryGetProperty("has_fulltext", out var hf) && hf.GetBoolean() ? "Yes" : "No",
                            LendingEdition = item.TryGetProperty("lending_edition_s", out var le) ? le.GetString() ?? string.Empty : string.Empty,
                            LendingIdentifier = item.TryGetProperty("lending_identifier_s", out var li) ? li.GetString() ?? string.Empty : string.Empty
                        };
                        list.Add(b);
                    }
                    catch
                    {
                        // ignore malformed item
                    }
                }

                ApiResponse = list;
            }
            catch (JsonException)
            {
                ErrorMessage = "Error parsing API response.";
                ApiResponse = new List<Book>();
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Network error while calling API: " + ex.Message;
                ApiResponse = new List<Book>();
            }
        }

        private static string GetStringFromArray(JsonElement el, string prop, int index)
        {
            if (el.TryGetProperty(prop, out var arr) && arr.ValueKind == JsonValueKind.Array && arr.GetArrayLength() > index)
            {
                return arr[index].GetString() ?? string.Empty;
            }
            return string.Empty;
        }
    }

    public class Book
    {
        public string AuthorKey { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string CoverEdition { get; set; } = string.Empty;
        public int FirstPublishYear { get; set; }
        public string BookAccess { get; set; } = string.Empty;
        public int EditionCount { get; set; }
        public string Fulltext { get; set; } = string.Empty;
        public string LendingEdition { get; set; } = string.Empty;
        public string LendingIdentifier { get; set; } = string.Empty;
    }
}
