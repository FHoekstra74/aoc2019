using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace aoc2019
{

    public enum Direction
    {
        Left,
        Right,
        Up,
        Dowm
    }

    public class day17
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day17.txt").First();

            Dictionary<long, long> items = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            Queue inputq = new Queue();
            Queue outputq = new Queue();
            long pointer = 0;
            long pointer2 = 0;

            run(items, inputq, outputq, ref pointer, ref pointer2);

            int x = 0;
            int y = 0;

            Dictionary<Point, int> points = new Dictionary<Point, int>();
            while (outputq.Count > 0)
            {
                //35 means #, 46 means ., 10 starts a new line of output below the current one
                int o = Convert.ToInt32(outputq.Dequeue());
                if (o == 10)
                {
                    y += 1;
                    x = 0;
                }
                else
                {
                    points.Add(new Point(x, y), o);
                    x++;
                }
            }

            printmap(points);

            int res = 0;
            foreach (Point p in points.Keys)
            {
                if (points[p] == 35)
                {
                    Point[] tocheck = new Point[] { new Point(p.X, p.Y - 1), new Point(p.X, p.Y + 1), new Point(p.X - 1, p.Y), new Point(p.X + 1, p.Y) };

                    bool isx = true;
                    foreach (Point p2 in tocheck)
                    {
                        if (points.ContainsKey(p2))
                        {
                            if (points[p2] != 35)
                                isx = false;
                        }
                        else
                            isx = false;
                    }

                    if (isx)
                        res += (p.X * p.Y);
                }
            }
            Console.WriteLine(string.Format("AnswerA:{0}", res));

            string route = "";
            Point pos = points.FirstOrDefault(x => x.Value == (int)'^').Key;
            Direction direction = Direction.Up;
            int stepcount = 0;
            bool stop = false;
            while (!stop)
            {
                Point tocheck = new Point(-1, -1);
                if (direction == Direction.Up)
                    tocheck = new Point(pos.X, pos.Y - 1);
                if (direction == Direction.Dowm)
                    tocheck = new Point(pos.X, pos.Y + 1);
                if (direction == Direction.Right)
                    tocheck = new Point(pos.X + 1, pos.Y);
                if (direction == Direction.Left)
                    tocheck = new Point(pos.X - 1, pos.Y);

                if (points.ContainsKey(tocheck) && points[tocheck] == (int)'#')
                {
                    stepcount++;
                    pos = tocheck;
                }
                else
                {
                    if (stepcount > 0)
                    {
                        route += ",";
                        route += stepcount.ToString();
                    }
                    stepcount = 0;

                    Point checkr = new Point(0, 0);
                    Point checkl = new Point(0, 0);
                    if (direction == Direction.Up)
                    {
                        checkr = new Point(pos.X + 1, pos.Y);
                        checkl = new Point(pos.X - 1, pos.Y);
                    }
                    if (direction == Direction.Dowm)
                    {
                        checkr = new Point(pos.X - 1, pos.Y);
                        checkl = new Point(pos.X + 1, pos.Y);
                    }
                    if (direction == Direction.Right)
                    {
                        checkr = new Point(pos.X, pos.Y + 1);
                        checkl = new Point(pos.X, pos.Y - 1);
                    }
                    if (direction == Direction.Left)
                    {
                        checkr = new Point(pos.X, pos.Y - 1);
                        checkl = new Point(pos.X, pos.Y + 1);
                    }

                    if (points[checkr] == (int)'#')
                    {
                        route += ",R";
                        if (direction == Direction.Up)
                            direction = Direction.Right;
                        else if (direction == Direction.Right)
                            direction = Direction.Dowm;
                        else if (direction == Direction.Dowm)
                            direction = Direction.Left;
                        else if (direction == Direction.Left)
                            direction = Direction.Up;
                    }
                    else if (points[checkl] == (int)'#')
                    {
                        route += ",L";
                        if (direction == Direction.Up)
                            direction = Direction.Left;
                        else if (direction == Direction.Left)
                            direction = Direction.Dowm;
                        else if (direction == Direction.Dowm)
                            direction = Direction.Right;
                        else if (direction == Direction.Right)
                            direction = Direction.Up;
                    }
                    else
                        stop = true;
                }
            }
            route = route.Substring(1);
            Console.WriteLine(route);
            route += ",";

            string[] part = new string[3];
 
            part[0] = findpart(route, route.IndexOfAny(new[] { 'R', 'L' }));
            route = route.Replace(part[0], "A,");

            part[1] = findpart(route, route.IndexOfAny(new[] { 'R', 'L' }));
            route = route.Replace(part[1], "B,");

            part[2] = findpart(route, route.IndexOfAny(new[] { 'R', 'L' }));
            route = route.Replace(part[2], "C,");

            route = route.TrimEnd(',');

            Console.WriteLine();
            Console.WriteLine(route);
            Console.WriteLine("A: " + part[0].TrimEnd(','));
            Console.WriteLine("B: " + part[1].TrimEnd(','));
            Console.WriteLine("C: " + part[2].TrimEnd(','));

            items = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            items[0] = 2;
            inputq = new Queue();
            outputq = new Queue();
            pointer = 0;
            pointer2 = 0;

            foreach (var c in route)
                inputq.Enqueue(c);
            inputq.Enqueue(10);
            for (int i =0; i<3; i++)
            {
                foreach (var c in part[i].TrimEnd(','))
                    inputq.Enqueue(c);
                inputq.Enqueue(10);
            }

            inputq.Enqueue((int)'n');
            inputq.Enqueue(10);

            long b = run(items, inputq, outputq, ref pointer, ref pointer2);
            foreach (long i in outputq)
                b = i;
            Console.WriteLine(string.Format("AnswerB {0}", b));
        }

        private static string findpart(string directions, int start)
        {
            for (int i = directions.Length - 1; i > start; i--)
            {
                string substr = directions[start..i];
                string help = substr.Substring(substr.Length - 2,1);
                if (substr.Length < 22 &&
                    substr.Last() == ',' &&
                    help.IndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}) != -1 &&
                    substr.IndexOfAny(new[] { 'A', 'B', 'C' }) == -1) 
                    {
                        return substr;
                }
            }
            return "";
        }

        private static void printmap(Dictionary<Point, int> map)
        {
            for (int y = map.Keys.Min(point => point.Y); y <= map.Keys.Max(point => point.Y); y++)
            {
                string line = "";
                for (int x = map.Keys.Min(point => point.X); x <= map.Keys.Max(point => point.X); x++)
                {
                    Point newpoint = new Point(x, y);
                    char c = ' ';
                    if (map.ContainsKey(newpoint))
                    {
                        c = (char)map[newpoint];
                    }
                    line += c;
                }
                Console.WriteLine(line);
            }
        }

        private static long getpointer(Dictionary<long, long> items, int paramnum, long pointer, int mode, long relativebase)
        {
            long p;
            if (mode == 0)
                p = Convert.ToInt64(items[pointer + paramnum]);
            else if (mode == 1)
                p = pointer + paramnum;
            else
                p = Convert.ToInt64(items[pointer + paramnum] + relativebase);
            if (!items.ContainsKey(p))
                items.Add(p, 0);
            return p;
        }

        private static long run(Dictionary<long, long> items, Queue inputs, Queue results, ref long pointer, ref long relativebase)
        {
            long param1, param2;
            int steps;
            param1 = param2 = steps = 0;
            while (true)
            {
                steps += 1;
                string opcodes = items[pointer].ToString().PadLeft(5, '0');
                int opcode = int.Parse(opcodes.Substring(3, 2));
                int mode3 = opcodes[0] - 48;
                int mode2 = opcodes[1] - 48;
                int mode1 = opcodes[2] - 48;
                if (new[] { 1, 2, 4, 5, 6, 7, 8, 9 }.Contains(opcode))
                    param1 = items[getpointer(items, 1, pointer, mode1, relativebase)];
                if (new[] { 1, 2, 5, 6, 7, 8 }.Contains(opcode))
                    param2 = items[getpointer(items, 2, pointer, mode2, relativebase)];
                switch (opcode)
                {
                    case 99:
                        return int.MinValue;
                    case 1:
                        items[getpointer(items, 3, pointer, mode3, relativebase)] = param1 + param2;
                        break;
                    case 2:
                        items[getpointer(items, 3, pointer, mode3, relativebase)] = param1 * param2;
                        break;
                    case 3:
                        if (inputs.Count == 0)
                            return (int.MinValue + 1);
                        else
                            items[getpointer(items, 1, pointer, mode1, relativebase)] = Convert.ToInt32(inputs.Dequeue());
                        break;
                    case 4:
                        results.Enqueue(param1);
                        pointer += 2;
                        break;
                    case 5:
                        if (param1 != 0)
                            pointer = Convert.ToInt32(param2);
                        else
                            pointer += 3;
                        break;
                    case 6:
                        if (param1 == 0)
                            pointer = Convert.ToInt32(param2);
                        else
                            pointer += 3;
                        break;
                    case 7:
                        if (param1 < param2)
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 1;
                        else
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 0;
                        break;
                    case 8:
                        if (param1 == param2)
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 1;
                        else
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 0;
                        break;
                    case 9:
                        relativebase += Convert.ToInt32(param1);
                        break;
                    default:
                        break;
                }
                if (new[] { 1, 2, 7, 8 }.Contains(opcode))
                    pointer += 4;
                if (new[] { 9, 3 }.Contains(opcode))
                    pointer += 2;
            }
        }
    }
}
