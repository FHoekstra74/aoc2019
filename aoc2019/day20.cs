using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day20
    {
        public static void Go()
        {
            var input = File.ReadLines(@"c:\github\aoc2019\input\day20.txt");
            StreamWriter w = new StreamWriter(@"c:\github\aoc2019\python\day20A.py");
            StreamWriter w2 = new StreamWriter(@"c:\github\aoc2019\python\day20B.py");

            w.WriteLine("import networkx as nx");
            w.WriteLine("maze = nx.Graph();");
            w2.WriteLine("import networkx as nx");
            w2.WriteLine("maze = nx.Graph();");

            int x = 0;
            int y = 0;

            Dictionary<Point, string> points = new Dictionary<Point, string>();
            foreach (string s in input)
            {
                x = 0;
                foreach (char c in s)
                {
                    if (c != (int)'#' && c != ' ')
                    {
                        points.Add(new Point(x, y), c.ToString());
                    }
                    x += 1;
                }
                y += 1;
            }

            List<Point> lp = new List<Point>();
            foreach (Point p in points.Keys)
                lp.Add(p);

            Dictionary<string, Point[]> specials = new Dictionary<string, Point[]>();
            foreach (Point p in lp)
            {
                if (points.ContainsKey(p) && char.IsLetter(points[p][0]))
                {
                    List<Point[]> chk = new List<Point[]>();
                    chk.Add(new Point[2] { new Point(p.X + 1, p.Y), new Point(p.X + 2, p.Y) });
                    chk.Add(new Point[2] { new Point(p.X - 1, p.Y), new Point(p.X - 2, p.Y) });
                    chk.Add(new Point[2] { new Point(p.X, p.Y + 1), new Point(p.X, p.Y + 2) });
                    chk.Add(new Point[2] { new Point(p.X, p.Y - 1), new Point(p.X, p.Y - 2) });

                    foreach (Point[] ck in chk)
                    {
                        Point check1 = ck[0];
                        Point check2 = ck[1];
                        if (points.ContainsKey(check1) && points.ContainsKey(check2) && char.IsLetter(points[check1][0]) && points[check2][0] == '.')
                        {
                            string s = points[p] + points[check1];
                            s = String.Concat(s.OrderBy(c => c));
                            points[check2] = s;
                            if (specials.ContainsKey(s))
                                specials[points[check2]][1] = check2;
                            else
                            {
                                Point[] vals = new Point[2];
                                vals[0] = check2;
                                specials.Add(points[check2], vals);
                            }
                            points.Remove(p);
                            points.Remove(check1);
                        }
                    }
                }
            }

            w2.WriteLine("for i in range(45):");
            foreach (Point p in points.Keys)
            {
                Point[] tocheck = new Point[] { new Point(p.X, p.Y - 1), new Point(p.X, p.Y + 1), new Point(p.X - 1, p.Y), new Point(p.X + 1, p.Y) };
                foreach (Point p2 in tocheck)
                {
                    if (points.ContainsKey(p2))
                    {
                        w.WriteLine(string.Format("maze.add_edge(({0}, {1}), ({2}, {3}))", p.X, p.Y, p2.X, p2.Y));
                        w2.WriteLine(string.Format("\tmaze.add_edge(({0}, {1}, {4}), ({2}, {3}, {4}))", p.X, p.Y, p2.X, p2.Y, "i"));
                    }
                }
            }

            int xminouter = points.Keys.Min(point => point.X);
            int xmaxouter = points.Keys.Max(point => point.X);
            int yminouter = points.Keys.Min(point => point.Y);
            int ymaxouter = points.Keys.Max(point => point.Y);

            w2.WriteLine("for i in range(44):");
            foreach (string s in specials.Keys)
            {
                Point[] ps = specials[s];
                if (ps[1] != new Point(0, 0))
                {
                    w.WriteLine(string.Format("maze.add_edge(({0}, {1}), ({2}, {3}))", ps[0].X, ps[0].Y, ps[1].X, ps[1].Y));
                    if (ps[0].X == xminouter || ps[0].X == xmaxouter || ps[0].Y == yminouter || ps[0].Y == ymaxouter)
                        w2.WriteLine(string.Format("\tmaze.add_edge(({0}, {1}, {4}), ({2}, {3}, {5})) # {6}", ps[0].X, ps[0].Y, ps[1].X, ps[1].Y, "i+1", "i", s));
                    else
                        w2.WriteLine(string.Format("\tmaze.add_edge(({0}, {1}, {4}), ({2}, {3}, {5})) # {6}", ps[1].X, ps[1].Y, ps[0].X, ps[0].Y, "i+1", "i", s));
                }
            }

            Point aa = specials["AA"][0];
            Point zz = specials["ZZ"][0];
            w.WriteLine(string.Format("res = nx.shortest_path_length(maze,({0},{1}),({2},{3}))", aa.X, aa.Y, zz.X, zz.Y));
            w.WriteLine("print(res);");
            w.Close();
            w2.WriteLine(string.Format("res = nx.shortest_path_length(maze,({0},{1},0),({2},{3},0))", aa.X, aa.Y, zz.X, zz.Y));
            w2.WriteLine("print(res);");
            w2.Close();
        }
    }
}
