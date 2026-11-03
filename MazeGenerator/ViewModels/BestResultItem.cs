using MazeGenerator.Models.Enums;

namespace MazeGenerator.ViewModels
{
    public class BestResultItem
    {
        public MazeType MazeType { get; set; }
        public DifficultType Difficulty { get; set; }
        public double BestTimeSeconds { get; set; }
        public int TimesCompleted { get; set; }
    }
}
