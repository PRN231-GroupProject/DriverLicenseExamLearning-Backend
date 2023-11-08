
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CarRenting.Client;
using DriverLicenseExamLearning_Service.DTOs.Response;

namespace DriverLicenseExamLearning_ClientGrpc.Pages.StaffManage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        private string _productApiUrl;


        public IndexModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = "";
        }

        public IList<UserResponse> Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../../Login");
            }

            HttpResponseMessage response = await _client.GetAsync(_productApiUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<UserResponse>? dataResponse = response.Content.ReadFromJsonAsync<List<UserResponse>>().Result;
                if (dataResponse != null)
                {
                    Customer = dataResponse;
                }
            }

            return Page();
        }
    }
}