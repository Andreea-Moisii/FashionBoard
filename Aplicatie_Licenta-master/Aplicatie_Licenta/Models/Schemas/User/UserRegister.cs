using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Schemas.User
{
    public class UserRegister
    {
        public string username { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
        public string email { get; set; }
    }
}
