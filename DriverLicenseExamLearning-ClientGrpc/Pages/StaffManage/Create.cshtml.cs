using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CarRenting.Client;
using DriverLicenseExamLearning_Service.DTOs.Response;
using Grpc.Net.Client;
using Grpc.Core;

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
        public NewStaffRequest Customer { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {

            var userId = HttpContext.Session.GetInt32("User");
            if (userId == null || userId != -1)
            {
                return RedirectToPage("../Login");
            }
            var jwt = HttpContext.Session.GetString("JWToken");
            var channel = GrpcChannel.ForAddress(_productApiUrl);
            var client = new Staff.StaffClient(channel);

            // Tạo một đối tượng Metadata để chứa JWT token
            var metadata = new Metadata
            {
                { "Authorization", "Bearer " + jwt }
            };

            //var response = client.GetStaffs(request);
            var response = client.CreateNewStaff(Customer, headers: metadata);

            return RedirectToPage("./Index");
        }
    }
}
