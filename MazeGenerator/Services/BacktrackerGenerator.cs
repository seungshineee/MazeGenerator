using MazeGenerator.Models;

namespace MazeGenerator.Services
{
    public class BacktrackerGenerator : IMazeGenerator
    {
        public Cell[,] Generate(Maze maze, int? seed = null)
        {
            var cells = CreateCells(maze);
            var random = seed.HasValue ? new Random(seed.Value) : new Random();
            var stack = new Stack<Cell>();

            var current = cells[0, 0];
            current.Visited = true;
            stack.Push(current);

            while (stack.Count > 0)
            {
                current = stack.Peek();
                var neighbors = GetUnvisitedNeighbors(maze, cells, current);

                if (neighbors.Count == 0)
                {
                    stack.Pop();
                    continue;
                }

                var next = neighbors[random.Next(neighbors.Count)];
                RemoveWall(current, next);
                next.Visited = true;
                stack.Push(next);
            }

            return cells;
        }

        private static Cell[,] CreateCells(Maze maze)
        {
            var cells = new Cell[maze.Width, maze.Height];

            for (int x = 0; x < maze.Width; x++)
                for (int y = 0; y < maze.Height; y++)
                    cells[x, y] = new Cell { X = x, Y = y };

            return cells;
        }

        private static List<Cell> GetUnvisitedNeighbors(Maze maze, Cell[,] cells, Cell c)
        {
            var list = new List<Cell>();
            if (c.Y > 0 && !cells[c.X, c.Y - 1].Visited) list.Add(cells[c.X, c.Y - 1]);
            if (c.X < maze.Width - 1 && !cells[c.X + 1, c.Y].Visited) list.Add(cells[c.X + 1, c.Y]);
            if (c.Y < maze.Height - 1 && !cells[c.X, c.Y + 1].Visited) list.Add(cells[c.X, c.Y + 1]);
            if (c.X > 0 && !cells[c.X - 1, c.Y].Visited) list.Add(cells[c.X - 1, c.Y]);
            return list;
        }

        private static void RemoveWall(Cell a, Cell b)
        {
            if (b.X == a.X && b.Y == a.Y - 1) { a.Top = false; b.Bottom = false; }
            else if (b.X == a.X + 1 && b.Y == a.Y) { a.Right = false; b.Left = false; }
            else if (b.X == a.X && b.Y == a.Y + 1) { a.Bottom = false; b.Top = false; }
            else if (b.X == a.X - 1 && b.Y == a.Y) { a.Left = false; b.Right = false; }
        }
    }
}
