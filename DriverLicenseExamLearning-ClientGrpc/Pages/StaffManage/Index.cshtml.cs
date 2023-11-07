using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Client.Pages.Admin2.Customer
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {


            return Page();
        }
    }
}
