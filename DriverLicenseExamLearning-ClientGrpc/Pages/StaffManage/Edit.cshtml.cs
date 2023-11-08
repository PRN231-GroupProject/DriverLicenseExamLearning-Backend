using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarRenting.Client;
using Grpc.Net.Client;
using Grpc.Core;

namespace DriverLicenseExamLearning_ClientGrpc.Pages.StaffManage
{
    public class EditModel : PageModel
    {
        private string _productApiUrl;

        public EditModel()
        {
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _productApiUrl = Constants.Api;
        }

        [BindProperty] public UpdateStaffRequest Customer { get; set; } = default!;

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
                var reslut = new UpdateStaffRequest()
                {
                    UserId = response.UserId,
                    UserName = response.UserName,
                    Password = response.Password,
                    Name = response.Name,
                    Email = response.Email,
                    Address = response.Address,
                    Status = response.Status,
                    PhoneNumber = response.PhoneNumber,
                };
                Customer = reslut;
                return Page();

            }

            return NotFound();

        }


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
            var response = client.UpdateStaff(Customer, headers: metadata);

            return RedirectToPage("./Index");
        }


    }
}