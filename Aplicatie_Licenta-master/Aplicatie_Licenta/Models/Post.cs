using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicatie_Licenta.Models
{
    public class Post
    {
        public int Id_post { get; set; } = -1;
        public User User { get; set; } = new User();
        public float Price { get; set; } = 0;
        public int Saves { get; set; } = 0;
        public string Description { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public List<Image> Images { get; set; } = new List<Image>();
        public bool Saved { get; set; } = false;
    }
}
