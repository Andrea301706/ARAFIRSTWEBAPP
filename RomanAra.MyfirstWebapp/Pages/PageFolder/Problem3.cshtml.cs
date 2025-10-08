   using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace YourAppNamespace.Pages
{
    public class AverageCalculatorModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Number 1 is required.")]
        public double? Num1 { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Number 2 is required.")]
        public double? Num2 { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Number 3 is required.")]
        public double? Num3 { get; set; }

        public double Average { get; set; }

        public bool ShowResult { get; set; } = false;

        public void OnGet()
        {
            // Initialize default state if needed
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Stay on page with validation messages
                return Page();
            }

            Average = (Num1.Value + Num2.Value + Num3.Value) / 3.0;
            ShowResult = true;

            return Page();
        }
    }
}
