using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day15
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day15.txt").First();

            Dictionary<long, long> items = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            Queue inputq = new Queue();
            Queue outputq = new Queue();
            long pointer = 0;
            long pointer2 = 0;

            Dictionary<Point, int> map = new Dictionary<Point, int>();
            map.Add(new Point(0, 0), 1);
            int x = 0;
            int y = 0;
            int stepcount = 0;

            handlepoint( x,  y, map, items, ref pointer, ref pointer2, ref stepcount);
       //     printmap(map);

            Point oxygen = map.FirstOrDefault(x => x.Value == 2).Key;
            List<Point> testB = new List<Point>();
            testB.Add(new Point(oxygen.X, oxygen.Y));
            map[new Point(oxygen.X, oxygen.Y)] = 3;

            bool stop = false;
            int AnswerB = 0;
            while (!stop)
            {
                List<Point> temp = new List<Point>();
                foreach (Point p in testB)
                {
                    Point[] tocheck = new Point[] { new Point(p.X, p.Y - 1), new Point(p.X, p.Y + 1), new Point(p.X - 1, p.Y), new Point(p.X + 1, p.Y) };
                    foreach(Point p2 in tocheck)
                    {
                        if (map.ContainsKey(p2))
                            if (map[p2] == 1)
                            {
                                map[p2] = 3;
                                temp.Add(p2);
                            }
                    }
                }
                stop = temp.Count == 0;
                foreach(Point p in temp)
                    testB.Add(new Point(p.X, p.Y));
                AnswerB++;
            }
            Console.WriteLine(string.Format("AnswerB: {0}", AnswerB-1));
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
                        int colo = map[newpoint];
                        if (colo == 0)
                            c = '#';
                        if (colo == 1)
                            c = '.';
                        if (colo == 2)
                            c = 'X';
                        if (x == 0 && y == 0)
                            c = 'S';
                    }
                    line += c;
                }
                Console.WriteLine(line);
            }
        }

        private static void handlepoint( int x,  int y, Dictionary<Point, int> map, Dictionary<long, long> items, ref long pointer, ref long pointer2, ref int stepcount )
        {
            Queue inputq = new Queue();
            Queue outputq = new Queue();
            stepcount += 1;

            Dictionary<long, long> copy = new Dictionary<long, long>(items);
            long pointerbackup = pointer;
            long pointer2backup = pointer2;
            int xbackup = x;
            int ybackup = y;
            int stepcountbakcup = stepcount;

            Point[] tocheck = new Point[] { new Point(x, y - 1), new Point(x, y + 1), new Point(x - 1, y), new Point(x + 1, y) };
            for (int t = 1; t<5; t++)
            {
                if (!map.ContainsKey(tocheck[t-1]))
                {
                    inputq.Enqueue(t);
                    run(items, inputq, outputq, ref pointer, ref pointer2);
                    int res = Convert.ToInt16(outputq.Dequeue());
                    map.Add(tocheck[t-1], res);

                    if (res == 1)
                        handlepoint(tocheck[t - 1].X, tocheck[t - 1].Y, map, items, ref pointer, ref pointer2, ref stepcount);
                    if (res == 2)
                        Console.WriteLine(string.Format("AnswerA: {0}",stepcount));

                    //Recursion is a bitch.... put state back:
                    items = new Dictionary<long, long>(copy);
                    pointer = pointerbackup;
                    pointer2 = pointer2backup;
                    x = xbackup;
                    y = ybackup;
                    stepcount = stepcountbakcup;
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
