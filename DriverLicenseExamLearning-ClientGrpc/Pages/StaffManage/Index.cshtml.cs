
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using CarRenting.Client;
using DriverLicenseExamLearning_Service.DTOs.Response;
using Grpc.Net.Client;
using Grpc.Core;

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
            _productApiUrl = Constants.Api;
        }

        public IList<StaffReponse> Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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
            var metadata = new Metadata();
            metadata.Add("Authorization", "Bearer " + jwt);

            // Tạo yêu cầu và đặt tiêu đề Metadata cho yêu cầu
            var request = new RequestModel();
            //var response = client.GetStaffs(request);
            var response = client.GetStaffs(request, headers: metadata);



            if (response.Staffs != null)
            {
                List<StaffReponse>? dataResponse = response.Staffs.ToList();
                if (dataResponse != null)
                {
                    Customer = dataResponse;
                }
            }

            return Page();
        }
    }
}