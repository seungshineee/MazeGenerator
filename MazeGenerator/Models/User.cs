using Microsoft.AspNetCore.Identity;

namespace MazeGenerator.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Password { get; set; }
        public int CharacterId { get; set; } = 1;
        public string Theme { get; set; } = "light";
        public List<LevelResults> Results { get; set; } = [];
    }
}
