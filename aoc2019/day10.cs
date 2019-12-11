using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;

namespace aoc2019
{
    public static class day10
    {
        class Asteroid
        {
            public Asteroid(int Xcoord, int Ycoord)
            {
                coord = new Point(Xcoord, Ycoord);
                angles = new List<double>();
            }

            public Point coord { get; set; }
            public double angle { get; set; }
            public List<double> angles { get; set; } 
        }

        public static void Go()
        {
            List<Asteroid> asteroids = new List<Asteroid>();
            List<string> input = File.ReadLines(@"c:\github\aoc2019\input\day10.txt").ToList<string>();
            int y = 0;
            foreach (string s in input)
            {
                int x = 0;
                foreach (char c in s)
                {
                    if (c == '#')
                        asteroids.Add(new Asteroid(x, y));
                    x++;
                }
                y++;
            }
            foreach (Asteroid a in asteroids)
            {
                List<double> angles = new List<double>();
                foreach (Asteroid b in asteroids)
                {
                    double angle = aocangle(a.coord, b.coord);
                    if (!a.angles.Contains(angle))
                        a.angles.Add(angle);
                }
            }
            int maxa = asteroids.Max(a => a.angles.Count);
            Asteroid ResultA = asteroids.First(a => a.angles.Count == maxa);

            Console.WriteLine(string.Format("AnswerA: {0}", maxa));
            List<Asteroid> cansee = new List<Asteroid>();
            foreach (Asteroid to in asteroids)
            {
                if (!(to.coord.X == ResultA.coord.X && to.coord.Y == ResultA.coord.Y))
                {
                    bool block = false;
                    foreach (Asteroid check in asteroids)
                    {
                        if (!(check.coord.X == to.coord.X && check.coord.Y == to.coord.Y) && !(check.coord.X == ResultA.coord.X && check.coord.Y == ResultA.coord.Y))
                        {
                            block = sameline(ResultA.coord, to.coord, check.coord);
                            if (block == true)
                                break;
                        }
                    }
                    if (!block)
                        cansee.Add(to);
                }
            }

            foreach (Asteroid item in cansee)
                item.angle = aocangle(item.coord, ResultA.coord);

            Asteroid result = cansee.OrderBy(f => f.angle).ToList()[199];
            int resultB = (result.coord.X * 100) + result.coord.Y;
            Console.WriteLine(string.Format("ResultB: {0}", resultB));
        }

        static double aocangle(Point point1, Point point2)
        {
            int Xdiff = point1.X - point2.X;
            int ydiff = point1.Y - point2.Y;
            double result = 0;

            if (ydiff < 0 && Xdiff >= 0)
                result = Math.Atan2(Math.Abs(Xdiff), Math.Abs(ydiff)) * 180d / Math.PI;
            else if (ydiff >= 0 && Xdiff > 0)
            { 
                result = Math.Atan2(Math.Abs(ydiff), Math.Abs(Xdiff)) * 180d / Math.PI;
                result += 90;
            }
            else if (ydiff > 0 && Xdiff <= 0)
            {
                result = Math.Atan2(Math.Abs(Xdiff), Math.Abs(ydiff)) * 180d / Math.PI;
                result += 180;
            }
            else if (ydiff <= 0 && Xdiff <= 0)
            {
                result = Math.Atan2(Math.Abs(ydiff), Math.Abs(Xdiff)) * 180d / Math.PI;
                result += 270;
            }
            return result;
        }

        static bool sameline(Point start, Point end, Point pt)
        {
            double dist1 = Math.Sqrt(Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2));
            double dist2 = Math.Sqrt(Math.Pow((start.X - pt.X), 2) + Math.Pow((start.Y - pt.Y), 2));
            if (dist2 > dist1)
                return false;

            return (aocangle(pt, start) == aocangle(end, start));
        }
    }
}
