using System.Drawing;

namespace FloodProblem
{
    class Wall
    {
        public static int Count { get; private set; }
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public bool Horizontal { get; }
        public int Number { get; }

        public Wall(Point Start, Point End)
        {
            if (Start.X == End.X)
            {
                Horizontal = true;
                if (Start.Y < End.Y)
                {
                    this.Start = Start;
                    this.End = End;
                }
                else
                {
                    this.Start = End;
                    this.End = Start;
                }
            }
            else
            {
                Horizontal = false;
                if (Start.X < End.X)
                {
                    this.Start = Start;
                    this.End = End;
                }
                else
                {
                    this.Start = End;
                    this.End = Start;
                };
            }
            Count++;
            Number = Count;
        }
    }
}
