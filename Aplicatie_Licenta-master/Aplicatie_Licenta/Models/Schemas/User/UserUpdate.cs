using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Schemas.User
{
    public class UserUpdate
    {
        public string email { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }
    }
}
