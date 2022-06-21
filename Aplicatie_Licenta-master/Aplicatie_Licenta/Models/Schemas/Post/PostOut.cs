using Aplicatie_Licenta.Service.Schemas.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Schemas.Post
{
    public class PostOut
    {
        public int id_post { get; set; }
        public UserOut user { get; set; }
        public float price { get; set; }
        public int saves { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public IEnumerable<String> colors { get; set; }
        public IEnumerable<String> images { get; set; }
        public bool saved { get; set; }
    }
}
