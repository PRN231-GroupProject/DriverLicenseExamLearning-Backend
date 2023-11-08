using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.Client;
using Grpc.Net.Client;
using Grpc.Core;

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

        [BindProperty] public StaffReponse Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            var response = client.GetStaff(new StaffLookUpModel()
            {
                UserId = (int)id
            }, headers: metadata);

      
            if (response != null)
            {
                var dataResponse = response;
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
            var response = client.DeleteStaff(new StaffLookUpModel()
            {
                UserId = (int)id
            }, headers: metadata);



            return RedirectToPage("./Index");
        }
    }
}