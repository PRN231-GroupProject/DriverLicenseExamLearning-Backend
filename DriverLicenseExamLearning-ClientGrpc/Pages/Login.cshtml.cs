using System.Net.Http.Headers;
using CarRenting.Client;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DriverLicenseExamLearning_ClientGrpc.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        private string _apiUrl;

        public LoginModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUrl = "https://localhost:7018/api/user/login";
        }

        [BindProperty] public UserLoginRequest MemberAccount { get; set; } = default!;

        public async Task<IActionResult> OnPostLogin()
        {
            if (MemberAccount == null)
            {
                return Page();
            }
            var response = await _client.PostAsJsonAsync(_apiUrl, new UserLoginRequest()
            {
                Email = MemberAccount.Email,
                Password = MemberAccount.Password
            });
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = await response.Content.ReadFromJsonAsync<Logined>();
                HttpContext.Session.SetString("JWToken", dataResponse.accessToken);
                HttpContext.Session.SetString("UserName", dataResponse.name);
                if (dataResponse.role.roleName != "Admin")
                {
                    ViewData["notification"] = "Required admin";
                    return Page();
            
                }
                return RedirectToPage("/Admin2/Customer/Index");
            }
            else
            {
                ViewData["notification"] = response.Content;
                return Page();
            }

        }

        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }

    public class Role
    {
        public string roleName { get; set; }
    }

    public class Logined
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Role role { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

}