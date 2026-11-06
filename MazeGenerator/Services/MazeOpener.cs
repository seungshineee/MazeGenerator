using MazeGenerator.Models;

namespace MazeGenerator.Services
{
    public static class MazeOpener
    {
        public static void OpenWalls(Cell[,] cells, int width, int height, double chance)
        {
            var random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < width - 1 && random.NextDouble() < chance)
                    {
                        cells[x, y].Right = false;
                        cells[x + 1, y].Left = false;
                    }
                    if (y < height - 1 && random.NextDouble() < chance)
                    {
                        cells[x, y].Bottom = false;
                        cells[x, y + 1].Top = false;
                    }
                }
            }
        }
    }
}
