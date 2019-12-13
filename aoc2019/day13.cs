using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day13
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day13.txt").First();

            Dictionary<long, long> items = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            Queue inputq = new Queue();
            Queue outputq = new Queue();
            long pointer = 0, relativebase = 0, retval = 0, res = 0;
            int x = 0, y = 0, tile = 0, balx = 0, curx = 0, score = 0, stepcount = 0;
            items[0] = 2;
            Dictionary<Point, int> points = new Dictionary<Point, int>();
            Dictionary<Point, int> currentpoints = new Dictionary<Point, int>();

            while (retval > int.MinValue)
            {
                retval = run(items, inputq, outputq, ref pointer, ref relativebase);
                stepcount++;
                while (outputq.Count > 1)
                {
                    x = Convert.ToInt32(outputq.Dequeue());
                    y = Convert.ToInt32(outputq.Dequeue());
                    tile = Convert.ToInt32(outputq.Dequeue());
                    if (!points.ContainsKey(new Point(x, y)))
                        points.Add(new Point(x, y), tile);
                    else
                        points[new Point(x, y)] = tile;
                    if (x == -1 && y == 0)
                        score = tile;
                    if (tile == 2 && stepcount == 1)
                        res += 1;
                    else if (tile == 3)
                        curx = x;
                    else if (tile == 4)
                        balx = x;
                }
                if (balx > curx)
                    inputq.Enqueue(1);
                else if (balx < curx)
                    inputq.Enqueue(-1);
                else
                    inputq.Enqueue(0);
                print(points, currentpoints);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(string.Format("ResultA: {0}", res));
            Console.WriteLine(string.Format("ResultB: {0}", score));
        }

        public static void print(Dictionary<Point, int> points, Dictionary<Point, int> currentpoints)
        {
            for (int y = points.Min(point => point.Key.Y); y <= points.Max(point => point.Key.Y); y++)
            {
                for (int x = points.Min(point => point.Key.X); x <= points.Max(point => point.Key.X); x++)
                {
                    Point newpoint = new Point(x, y);
                    char c = ' ';
                    if (points.ContainsKey(newpoint))
                    {
                        int value = points[newpoint];
                        if (value == 1)
                            c = '#';
                        else if (value == 2)
                            c = 'X';
                        else if (value == 3)
                            c = '-';
                        else if (value == 4)
                            c = 'B';
                        if (x >= 0)
                        {
                            if (!currentpoints.ContainsKey(new Point(x, y)))
                                currentpoints.Add(new Point(x, y), 'z');
                            if (value != currentpoints[new Point(x, y)])
                            {
                                currentpoints[new Point(x, y)] = value;
                                Console.SetCursorPosition(x, y + 1);
                                Console.Write(c);
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(10, 0);
                            Console.Write(string.Format("Points: {0}", value));
                        }
                    }
                }
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
