using MazeGenerator.Models;

namespace MazeGenerator.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public Character Character { get; set; }
        public List<LevelResults> BestResults { get; set; } = new();
    }
}
