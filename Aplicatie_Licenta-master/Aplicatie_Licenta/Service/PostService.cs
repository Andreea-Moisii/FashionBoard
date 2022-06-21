using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Interface;
using Aplicatie_Licenta.Service.Schemas.Post;
using Aplicatie_Licenta.Service.Schemas.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service
{
    public class PostService : IPostService
    {
        // singleton instace
        private static PostService _instance = new PostService();
        public static PostService Instance { get { return _instance; } }
        private PostService() { }


        public async Task CreatePost(Post post)
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

        public async Task DeletePost(int id)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.DeleteAsync($"http://localhost:8000/api/posts/{id}");
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.GetAsync("http://localhost:8000/api/posts");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        public async Task<IEnumerable<Post>> GetAllPostsForUser(string Username)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            HttpResponseMessage response = await client.GetAsync("http://localhost:8000/api/posts");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<PostOut>>(jsonResponse);
            return posts.Select(p => ToPost(p));
        }

        public async Task UpdatePost(Post post)
        {
            using HttpClient client = new();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);
            var postUpdate = ToPostUpdate(post);

            var json = JsonConvert.SerializeObject(postUpdate);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"http://localhost:8000/api/posts/{post.Id_post}", content);
        }

        public async Task UpdatePostColors(int id_post, IEnumerable<string> postColors)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var json = JsonConvert.SerializeObject(postColors);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"http://localhost:8000/api/posts/{id_post}/colors", content);
        }
        
        public async Task UpdatePostImages(int id_post, IEnumerable<string> postImages)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var json = JsonConvert.SerializeObject(postImages);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"http://localhost:8000/api/posts/{id_post}/images", content);
        }

        public async Task SavePost(int id_post)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.LoginToken);

            var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"http://localhost:8000/api/saves/{id_post}", content);
        }

        public async Task DeleteSave(int id_post)
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
