using Aplicatie_Licenta.Models.Schemas;
using Aplicatie_Licenta.Service.Schemas.User;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service
{
    public static class AuthService
    {
        // login token
        public static string? LoginToken { get; set; }

        // --------------------- login --------------------- //
        public static async Task<bool> Login(string username, string password)
        {
            using HttpClient client = new();
            var user = new UserLogin
            {
                username = username,
                password = password
            };
            var jsonUser = JsonConvert.SerializeObject(user);

            // set httpContent the user
            var httpContent = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            var request = await client.PostAsync("http://localhost:8000/api/login", httpContent);
            var response = await request.Content.ReadAsStringAsync();

            var auth = JsonConvert.DeserializeObject<AuthToken>(response);
            LoginToken = auth?.token;

            if (request.IsSuccessStatusCode)
            {
                var r = await UserService.GetUserByUsername(username).ContinueWith(t =>
               {
                   if (t.Result != null)
                   {
                       UserService.CurrentUser = t.Result;
                       return true;
                   }
                   else
                   {
                       return false;
                   }
               });
                return r;
            }
            else
            {
                return false;
            }
        }

        // --------------------- register a new user --------------------- //
        public static async Task Register(string username, string password, string email)
        {
            using HttpClient client = new();
            var user = new UserRegister
            {
                username = username,
                password = password,
                email = email
            };

            var jsonUser = JsonConvert.SerializeObject(user);
            // set httpContent the user
            var httpContent = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            var request = await client.PostAsync("http://localhost:8000/api/register", httpContent);
        }
    }
}
