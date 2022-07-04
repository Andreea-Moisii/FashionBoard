using Aplicatie_Licenta.Models;
using System.Collections.Generic;

namespace Aplicatie_Licenta.Service.Schemas.Post
{
    public class PostUpdate
    {
        public int id { get; set; } = -1;
        public float price { get; set; } = 0;
        public string description { get; set; } = "";
        public IEnumerable<Image> images_add { get; set; } = new List<Image>();
        public IEnumerable<Image> images_remove { get; set; } = new List<Image>();
    }
}
