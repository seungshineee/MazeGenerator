using MazeGenerator.Models;

namespace MazeGenerator.Services
{
    public interface IMazeGenerator
    {
        Cell[,] Generate(Maze maze, int? seed = null);
    }
}
