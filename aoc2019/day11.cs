using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Drawing;

namespace aoc2019
{
    public static class day11
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day11.txt").First();
            Dictionary<Point, int> points = dorun(input, 0);
            Console.WriteLine(string.Format("AnswerA: {0}", points.Count));
            points = dorun(input, 1);
            for (int y = points.Min(point => point.Key.Y); y <= points.Max(point => point.Key.Y); y++)
            {
                string line = "";
                for (int x = points.Min(point => point.Key.X); x <= points.Max(point => point.Key.X); x++)
                {
                    Point newpoint = new Point(x, y);
                    char c = ' ';
                    if (points.ContainsKey(newpoint))
                    {
                        int colo = points[newpoint];
                        if (colo == 1)
                            c = '#';
                    }
                    line += c;
                }
                Console.WriteLine(line);
            }
        }

        public static Dictionary<Point, int> dorun(string input, int start)
        {
            Dictionary<Point, int> points = new Dictionary<Point, int>();
            Dictionary<long, long> items = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            Queue inputq = new Queue();
            Queue outputq = new Queue();

            long pointer = 0;
            long relativebase = 0;
            long retval = 0;
            int direct = 1;
            Point curpoint = new Point(0, 0);
            points.Add(curpoint, start);
            while (retval != int.MinValue)
            {
                inputq.Enqueue(points[curpoint]);
                retval = run(items, inputq, outputq, ref pointer, ref relativebase);

                if (outputq.Count == 2)
                {
                    int color = Convert.ToInt32(outputq.Dequeue());
                    int pos = Convert.ToInt32(outputq.Dequeue());
                    points[curpoint] = color;

                    if (pos == 0)
                    {
                        if (direct == 4)
                            direct = 1;
                        else
                            direct += 1;
                    }
                    else
                    {
                        if (direct == 1)
                            direct = 4;
                        else
                            direct -= 1;
                    }
                    switch (direct)
                    {
                        case 1:
                            curpoint = new Point(curpoint.X, curpoint.Y - 1);
                            break;
                        case 2:
                            curpoint = new Point(curpoint.X - 1, curpoint.Y);
                            break;
                        case 3:
                            curpoint = new Point(curpoint.X, curpoint.Y + 1);
                            break;
                        case 4:
                            curpoint = new Point(curpoint.X + 1, curpoint.Y);
                            break;
                    }
                    if (!points.ContainsKey(curpoint))
                    {
                        points.Add(curpoint, 0);
                    }
                }
            }
            return points;
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
