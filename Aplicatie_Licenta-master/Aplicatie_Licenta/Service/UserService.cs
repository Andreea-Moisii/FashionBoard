using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.User;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service
{
    public static class UserService
    {
        public static User? CurrentUser { get; set; }

        // ------------------- delete user acount ------------------- //
        public static async Task DeleteUser()
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.DeleteAsync("http://localhost:8000/api/users");
        }

        // ------------------- get user info ------------------- //
        public static async Task<User> GetUserByUsername(string username)
        {
            using HttpClient client = new();

            HttpResponseMessage response = await client.GetAsync($"http://localhost:8000/api/users/{username}");
            var result = await response.Content.ReadAsStringAsync();
            var userOut = JsonConvert.DeserializeObject<UserOut>(result);

            return ToUser(userOut);
        }

        // ------------------- update user info ------------------- //
        public static async Task UpdateUser(User user)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);
            var userUpdate = ToUserUpdate(user);

            var json = JsonConvert.SerializeObject(userUpdate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync("http://localhost:8000/api/users", content);
        }

        // convertors 
        public static User ToUser(UserOut user)
        {
            return new User
            {
                Username = user.username,
                Email = user.email,
                Description = user.description,
                ProfileImage = user.image_url
            };
        }

        public static UserUpdate ToUserUpdate(User user)
        {
            return new UserUpdate
            {
                email = user.Email,
                description = user.Description,
                image_url = user.ProfileImage
            };
        }
    }
}
