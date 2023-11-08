using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.Client;


namespace DriverLicenseExamLearning_ClientGrpc.Pages.StaffManage
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;
        private string _productApiUrl;

        public DeleteModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = Constants.Api;
        }

        [BindProperty] public UserResponse Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../../Login");
            }

            HttpResponseMessage response = await _client.GetAsync(_productApiUrl + "/" + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = response.Content.ReadFromJsonAsync<UserResponse>().Result;
                if (dataResponse != null)
                {
                    Customer = dataResponse;
                    return Page();
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            await _client.DeleteAsync(_productApiUrl + "/" + id);
            return RedirectToPage("./Index");
        }
    }
}