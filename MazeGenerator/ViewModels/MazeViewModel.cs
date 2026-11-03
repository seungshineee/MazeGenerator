using MazeGenerator.Models.Enums;

namespace MazeGenerator.ViewModels
{
    public class MazeViewModel
    {
        public DifficultType Difficulty { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string CharacterImage { get; set; } = "";
        public double? BestTimeSeconds { get; set; }
    }
}
