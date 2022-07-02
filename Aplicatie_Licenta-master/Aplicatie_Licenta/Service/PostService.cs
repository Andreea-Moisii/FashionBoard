using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.Post;
using Aplicatie_Licenta.Service.Schemas.User;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service
{
    public static class PostService
    {
        public static async Task CreatePost(Post post)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);
            var postIn = new PostIn
            {
                price = post.Price,
                description = post.Description,
                images = post.Images,
                colors = post.Colors
            };

            var json = JsonConvert.SerializeObject(postIn);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:8000/api/posts", content);
        }

        public static async Task DeletePost(int id)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.DeleteAsync($"http://localhost:8000/api/posts/{id}");
        }

        public static async Task<IEnumerable<Post>> GetAllPosts()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.GetAsync("http://localhost:8000/api/posts");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        public static async Task<IEnumerable<Post>> GetFiterPosts(int sortId = 0, string color = "")
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);
            
            // remove first letter
            if (color!= "")
                color = color.Remove(0,3); // remove first 3 leters: # and 2 letters for transparency

            HttpResponseMessage response = await client.GetAsync($"http://localhost:8000/api/filter/posts?sortId={sortId}&color={color}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        public static async Task<IEnumerable<Post>> GetAllPostsForUser(string Username)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.GetAsync($"http://localhost:8000/api/posts/{Username}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        public static async Task UpdatePost(Post post)
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);
            var postUpdate = ToPostUpdate(post);

            var json = JsonConvert.SerializeObject(postUpdate);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"http://localhost:8000/api/posts/{post.Id_post}", content);
        }

        public static async Task UpdatePostImages(int id_post, IEnumerable<string> postImages)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var json = JsonConvert.SerializeObject(postImages);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"http://localhost:8000/api/posts/{id_post}/images", content);
        }

        public static async Task<IEnumerable<Post>> GetAllSavedPost()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.GetAsync($"http://localhost:8000/api/saves");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        public static async Task AddSave(int id_post)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"http://localhost:8000/api/saves/{id_post}", content);
        }

        public static async Task DeleteSave(int id_post)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.DeleteAsync($"http://localhost:8000/api/saves/{id_post}");
        }


        // convertors 
        private static Post ToPost(PostOut post)
        {
            return new Post
            {
                Id_post = post.id_post,
                User = ToUser(post.user),
                Price = post.price,
                Saves = post.saves,
                Description = post.description,
                Date = post.date,
                Colors = post.colors,
                Images = post.images,
                Saved = post.saved
            };
        }

        private static PostUpdate ToPostUpdate(Post post)
        {
            return new PostUpdate
            {
                price = post.Price,
                description = post.Description
            };
        }

        private static User ToUser(UserOut user)
        {
            return new User(user.username, user.email, user.description, user.image_url);
        }
    }
}
