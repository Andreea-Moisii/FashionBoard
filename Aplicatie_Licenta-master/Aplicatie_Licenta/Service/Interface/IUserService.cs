using Aplicatie_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Interface
{
    public interface IUserService
    {
        // get user by username
        Task<User> GetUserByUsername(string username);

        // update user
        Task UpdateUser(User user);

        // delete user 
        Task DeleteUser();
    }
}
