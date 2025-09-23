using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RomanAra.MyfirstWebapp.Pages
{
    public class IndexModel : PageModel
    {
        // Properties to bind the form inputs
        [BindProperty]
        public double Number1 { get; set; }

        [BindProperty]
        public double Number2 { get; set; }

        [BindProperty]
        public double Number3 { get; set; }

        // Property to store the calculated average
        public double? Average { get; private set; }

        // Handle GET requests (no-op here)
        public void OnGet()
        {
        }

        // Handle POST requests (form submission)
        public IActionResult OnPost()
        {
            Average = (Number1 + Number2 + Number3) / 3;

            // Return the same page to display result
            return Page();
        }
    }
}

