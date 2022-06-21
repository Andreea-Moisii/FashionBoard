using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicatie_Licenta.Service.Schemas.Post
{
    public class PostIn
    {
        public float price { get; set; }
        public string description { get; set; }
        public IEnumerable<String> images { get; set; }
        public IEnumerable<String> colors { get; set; }
    }
}
