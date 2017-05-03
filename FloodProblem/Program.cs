using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace FloodProblem
{
    class Program
    {
        static int fillCount;

        static void Output(List<Wall> Walls)
        {
            Console.WriteLine(Walls.Count);
            foreach (Wall wall in Walls)
                Console.WriteLine(wall.Number);
        }

        static void BuildWalls(List<Wall> Walls, Cell[,] territory)
        {
            foreach (var wall in Walls)
                if (wall.Horizontal)
                    for (int i = wall.Start.Y; i < wall.End.Y; i++)                    
                        territory[wall.Start.X, i].WallL = true;                   
                else
                    for (int i = wall.Start.X; i < wall.End.X; i++)                   
                        territory[i, wall.Start.Y].WallD = true;                    
        }

        static void FillWater(Cell[,] territory, int MaxX, int MaxY)
        {
            for (int k = 0; k < MaxX; k++)
                for (int m = 0; m < MaxY; m++)
                {
                    if (territory[k, m].Flooded == true)
                    {
                        var Stack = new Stack<Point>();
                        Stack.Push(new Point(k, m));
                        while (!(Stack.Count == 0))
                        {
                            Point p = Stack.Pop();
                            int i = p.X;
                            int j = p.Y;
                            if (i != MaxX - 1 && territory[i + 1, j].Flooded == false && !territory[i+1, j].WallL)
                            {
                                territory[i + 1, j].Flooded = true;
                                fillCount++;
                                Stack.Push(new Point(i + 1, j));
                            }
                            if (i != 0 && !territory[i - 1, j].Flooded  && !territory[i, j].WallL)
                            {
                                territory[i - 1, j].Flooded = true;
                                fillCount++;
                                Stack.Push(new Point(i - 1, j));
                            }
                            if (j != MaxY - 1 && territory[i, j + 1].Flooded == false && !territory[i, j+1].WallD)
                            {
                                territory[i, j + 1].Flooded = true;
                                fillCount++;
                                Stack.Push(new Point(i, j + 1));
                            }
                            if (j != 0 && territory[i, j - 1].Flooded == false && !territory[i, j].WallD)
                            {
                                territory[i, j - 1].Flooded = true;
                                fillCount++;
                                Stack.Push(new Point(i, j - 1));
                            }
                        }
                    }
                }
        }

        static void BreakWalls(List<Wall> Walls, Cell[,] territory)
        {
            int j = 0;
            while (j < Walls.Count)
                if (Walls[j].Horizontal)               
                    if (territory[Walls[j].Start.X - 1, Walls[j].Start.Y].Flooded != territory[Walls[j].Start.X, Walls[j].Start.Y].Flooded)
                    {
                        for (int i = Walls[j].Start.Y; i < Walls[j].End.Y; i++)
                            territory[Walls[j].Start.X, i].WallL = false;
                        Walls.RemoveAt(j);
                    }
                    else j++;                
                else               
                    if (territory[Walls[j].Start.X, Walls[j].Start.Y - 1].Flooded != territory[Walls[j].Start.X, Walls[j].Start.Y].Flooded)
                    {
                        for (int i = Walls[j].Start.X; i < Walls[j].End.X; i++)
                            territory[i, Walls[j].Start.Y].WallD = false;
                        Walls.RemoveAt(j);
                    }
                    else j++;               
        }

        static void InputPoints(Point[] Points, string[] input, int pointsCount, ref int MaxX, ref int MaxY)
        {
            for (int i = 1; i <= pointsCount; i++)
            {
                int x = Convert.ToInt32(input[i].Split(' ')[0]);
                if (x > MaxX)
                    MaxX = x;
                int y = Convert.ToInt32(input[i].Split(' ')[1]);
                if (y > MaxY)
                    MaxY = y;
                Points[i - 1] = new Point(x, y);
            }
        }

        static void InputWalls(List<Wall> Walls, Point[] Points, string[] input, int wallsCount)
        {
            for (int i = 0; i < wallsCount; i++)
            {
                int pointStart = Convert.ToInt32(input[Points.Length + 2 + i].Split(' ')[0]);
                int pointEnd = Convert.ToInt32(input[Points.Length + 2 + i].Split(' ')[1]);
                Walls.Add(new Wall(Points[pointStart - 1], Points[pointEnd - 1]));
            }
        }

        static void FillBorder(Cell[,] territory, int MaxX, int MaxY)
        {
            for (int i = 0; i < MaxX + 1; i++)
                for (int j = 0; j < MaxY + 1; j++)
                {
                    territory[i, j] = new Cell();
                    if (i == MaxX || i == 0 || j == MaxY || j == 0)
                        territory[i, j].Flooded = true;
                }
        }

        static void Main(string[] args)
        {
            int MaxX = 0;
            int MaxY = 0;
            string[] sep = { Environment.NewLine };
            string[] input = File.ReadAllText("input1.txt").Split(sep, StringSplitOptions.None);

            int pointsCount = Convert.ToInt32(input[0]);
            Point[] Points = new Point[pointsCount];
            InputPoints(Points, input, pointsCount, ref MaxX, ref MaxY);

            Cell[,] territory = new Cell[MaxX + 1, MaxY + 1];
            int terrCount = territory.Length;
            FillBorder(territory, MaxX, MaxY);
            fillCount = 2 * (MaxX + 1) + 2 * (MaxY - 1);

            int wallsCount = Convert.ToInt32(input[pointsCount + 1]);
            List<Wall> Walls = new List<Wall>();
            InputWalls(Walls, Points, input, wallsCount);

            BuildWalls(Walls, territory);
            FillWater(territory, MaxX + 1, MaxY + 1);

            while (terrCount > fillCount)
            {
                BreakWalls(Walls, territory);
                FillWater(territory, MaxX + 1, MaxY + 1);
            }

            Output(Walls);
            Console.ReadLine();
        }
    }
}
