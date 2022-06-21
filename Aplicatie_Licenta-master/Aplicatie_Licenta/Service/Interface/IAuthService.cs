using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Interface
{
    public abstract class IAuthService
    {
        public static string ?LoginToken { get; set; }

        // register a new user
        public abstract Task Register(string username, string password, string email);

        // login a user
        public abstract Task<bool> Login(string username, string password);
    }
}
