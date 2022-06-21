using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicatie_Licenta.Models
{
    public class Post
    {
        public int Id_post { get; set; }
        public User User { get; set; }
        public float Price { get; set; }
        public int Saves { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<string> Colors { get; set; }
        public IEnumerable<string> Images { get; set; }
        public String MainImage => Images?.FirstOrDefault("https://data.whicdn.com/images/353151432/original.jpg") ?? "https://data.whicdn.com/images/353151432/original.jpg";
        public bool Saved { get; set; }


        public Post()
        {
            Id_post = 0;
            User = new User();
            Price = 0;
            Saves = 0;
            Description = "";
            Date = DateTime.Now;
            Colors = new List<string>();
            Images = new List<string>();
            Saved = false;
        }

        public Post(int id_post, User user, float price, int saves, string description,
            DateTime date, IEnumerable<string> colors, IEnumerable<string> images, bool saved)
        {
            Id_post = id_post;
            User = user;
            Price = price;
            Saves = saves;
            Description = description;
            Date = date;
            Colors = colors;
            Images = images;
            Saved = saved;
        }

    }
}
