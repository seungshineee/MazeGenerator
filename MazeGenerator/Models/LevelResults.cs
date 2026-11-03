using MazeGenerator.Models.Enums;

namespace MazeGenerator.Models;

public class LevelResults
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public MazeType MazeType { get; set; }
    public DifficultType Difficulty { get; set; }
    public double TimeSeconds { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
