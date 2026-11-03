using MazeGenerator.Models;

namespace MazeGenerator.Services
{
    public class MazeGeneration
    {
        public int Width { get; }
        public int Height { get; }
        public Cell[,] Cells { get; }

        public MazeGeneration(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Cells[x, y] = new Cell { X = x, Y = y };
        }
    }
}
