using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.Post;
using Aplicatie_Licenta.Service.Schemas.User;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Image = Aplicatie_Licenta.Models.Image;

namespace Aplicatie_Licenta.Service
{
    public static class PostService
    {
        // ---------------- creates a new post ---------------- //
        public static async Task CreatePost(Post post)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            // post images // 
            List<Image> images = new();
            foreach (var image in post.Images)
            {
                var img = await ImageService.UploadImage(image.url.Remove(0, 8));
                images.Add(img);
            }

            // create postIn for server //
            var postIn = new PostIn
            {
                price = post.Price,
                description = post.Description,
                images = images
            };

            var json = JsonConvert.SerializeObject(postIn);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:8000/api/posts", content);
        }

        // ---------------- delete a post by id ---------------- //
        public static async Task DeletePost(int id)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.DeleteAsync($"http://localhost:8000/api/posts/{id}");
        }

        // ---------------- get all post (with filters) ---------------- //
        public static async Task<IEnumerable<Post>> GetFiterPosts(int sortId = 0, string color = "", string word = "")
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var link = $"http://localhost:8000/api/filter/posts?sortId={sortId}";
            if (color != "")
            {
                color = color.Remove(0, 3); // remove first 3 leters: # and 2 letters for transparency
                link += $"&color={color}";
            }
            if (word != "")
            {
                link += $"&word={word}";
            }

            HttpResponseMessage response = await client.GetAsync(link);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        // ---------------- get all posts for user (with filter) ---------------- //
        public static async Task<IEnumerable<Post>> GetAllFilteredPostsForUser(string username, int sortId, string color)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var link = $"http://localhost:8000/api/filter/{username}/posts?sortId={sortId}";
            if (color != "")
            {
                color = color.Remove(0, 3); // remove first 3 leters: # and 2 letters for transparency
                link += $"&color={color}";
            }
            
            HttpResponseMessage response = await client.GetAsync(link);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        // ---------------- update a post ---------------- //
        public static async Task UpdatePost(PostUpdate post)
        {
            using HttpClient client = new();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var json = JsonConvert.SerializeObject(post);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"http://localhost:8000/api/posts/{post.id}", content);
        }


        // ---------------- get all saves (with filters) ---------------- //
        public static async Task<IEnumerable<Post>> GetAllFilteredSavedPost(int sortId = 0, string color = "", string word = "")
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

   
            var link = $"http://localhost:8000/api/filter/saves?sortId={sortId}";
            if (color != "")
            {
                color = color.Remove(0, 3); // remove first 3 leters: # and 2 letters for transparency
                link += $"&color={color}";
            }

            HttpResponseMessage response = await client.GetAsync(link);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        // ---------------- add a save ---------------- //
        public static async Task AddSave(int id_post)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"http://localhost:8000/api/saves/{id_post}", content);
        }

        // ---------------- remove a save ---------------- //
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
            return new User
            {
                Username = user.username,
                Email = user.email,
                Description = user.description,
                ProfileImage = user.image_url
            };
        }
    }
}
