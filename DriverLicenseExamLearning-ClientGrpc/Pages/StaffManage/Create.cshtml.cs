using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CarRenting.Client;
using DriverLicenseExamLearning_Service.DTOs.Response;

namespace DriverLicenseExamLearning_ClientGrpc.Pages.StaffManage
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;
        private string _productApiUrl;

        public CreateModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = Constants.Api;
        }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../../Login");
            }

            return Page();
        }

        [BindProperty]
        public UserResponse Customer { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {

            HttpResponseMessage response = await _client.PostAsJsonAsync(_productApiUrl, Customer);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
