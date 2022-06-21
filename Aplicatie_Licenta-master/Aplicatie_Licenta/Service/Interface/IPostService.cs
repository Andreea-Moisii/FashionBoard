using Aplicatie_Licenta.Models;
using Aplicatie_Licenta.Service.Schemas.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Interface
{
    public interface IPostService
    {
        // get all posts
        Task<IEnumerable<Post>> GetAllPosts();

        // create a new post
        Task CreatePost(Post post);

        // delete a post
        Task DeletePost(int id);

        // update a post
        Task UpdatePost(Post post);

        // update post Colors
        Task UpdatePostColors(int id_post, IEnumerable<string> postColors);

        // update post images
        Task UpdatePostImages(int id_post, IEnumerable<string> postImages);

        // add save
        Task SavePost(int id_post);

        // delete save
        Task DeleteSave(int id_post);
    }
}
