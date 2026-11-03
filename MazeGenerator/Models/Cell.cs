namespace MazeGenerator.Models
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Top { get; set; } = true;
        public bool Right { get; set; } = true;
        public bool Bottom { get; set; } = true;
        public bool Left { get; set; } = true;
        public bool Visited { get; set; }
    }
}
