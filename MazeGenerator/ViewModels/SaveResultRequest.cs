namespace MazeGenerator.ViewModels
{
    public class SaveResultRequest
    {
        public string Difficulty { get; set; } = "";
        public double TimeSeconds { get; set; }
        public string MazeType { get; set; } = "Standart";
    }
}
