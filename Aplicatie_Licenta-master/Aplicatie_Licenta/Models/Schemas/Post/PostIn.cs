using Aplicatie_Licenta.Models;
using System;
using System.Collections.Generic;

namespace Aplicatie_Licenta.Service.Schemas.Post
{
    public class PostIn
    {
        public float price { get; set; }
        public string description { get; set; }
        public IEnumerable<Image> images { get; set; }
    }
}
