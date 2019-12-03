using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Drawing;

namespace aoc2019
{
    public class day3
    {
        public static void Go()
        {
            List<string> input = File.ReadLines(@"c:\github\aoc2019\input\day3.txt").ToList<string>();
            List<Point> line1 = Path(input[0]);
            List<Point> line2 = Path(input[1]);

            int closest = int.MaxValue;
            int feweststeps = int.MaxValue;
            foreach (Point p in line1.Intersect(line2))
            {
                if (!p.Equals(new Point(1, 1)))
                {
                    int dist = CalculateManhattanDistance(1, p.X, 1, p.Y);
                    if (dist < closest)
                        closest = dist;

                    int steps = line1.IndexOf(p) + line2.IndexOf(p);
                    if (steps < feweststeps)
                        feweststeps = steps;
                }
            }

            Console.WriteLine(string.Format("AnswerA: {0}", closest));
            Console.WriteLine(string.Format("AnswerB: {0}", feweststeps));
        }

        private static List<Point> Path(string directions)
        {
            List<Point> p = new List<Point>();
            int x = 1;
            int y = 1;
            p.Add(new Point(x, y));
            foreach (string help in directions.Split(','))
            {
                string direction = help.Substring(0, 1);
                int length = Convert.ToInt16(help.Substring(1));
                for (int i = 1; i < length + 1; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            y += 1;
                            break;
                        case "D":
                            y -= 1;
                            break;
                        case "L":
                            x -= 1;
                            break;
                        case "R":
                            x += 1;
                            break;
                        default:
                            break;
                    }
                    p.Add(new Point(x, y));
                }
            }
            return p;
        }

        public static int CalculateManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
