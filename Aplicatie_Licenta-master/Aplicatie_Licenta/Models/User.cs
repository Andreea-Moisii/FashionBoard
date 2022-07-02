namespace Aplicatie_Licenta.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ProfileImage { get; set; }

        public User()
        {
            Username = "";
            Email = "";
            Description = "";
            ProfileImage = "";
        }
        public User(string username, string email, string desc, string profileImage)
        {
            Username = username;
            Email = email;
            Description = desc;
            ProfileImage = profileImage;
        }
    }
}
