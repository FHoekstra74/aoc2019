﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day19
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day19.txt").First();
            Dictionary<long, long> intcode = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            int res = 0;

            for (int xa = 0; xa < 50; xa++)
                for (int ya = 0; ya < 50; ya++)
                    if (runIntcode(intcode, xa, ya) == 1)
                        res += 1;

            Console.WriteLine(string.Format("AnswerA: {0}", res));

            int x = 0;
            int y = 100;
            while (true)
            {
                if (runIntcode(intcode, x, y) == 1)
                {
                    if (runIntcode(intcode, x + 99, y - 99) == 1)
                    {
                        Console.WriteLine(string.Format("AnswerB: {0}", (x * 10000) + (y - 99)));
                        break;
                    }
                    y++;
                }
                else
                    x++;
            }
        }

        private static long runIntcode(Dictionary<long, long> program, int x, int y)
        {
            Queue<long> inputq = new Queue<long>();
            Queue<long> outputq = new Queue<long>();
            long pointer = 0;
            long pointer2 = 0;
            inputq.Enqueue(x);
            inputq.Enqueue(y);
            run(new Dictionary<long, long>(program), inputq, outputq, ref pointer, ref pointer2);
            return outputq.Dequeue();
        }

        private static void printmap(Dictionary<Point, int> map)
        {
            for (int y = map.Keys.Min(point => point.Y); y <= map.Keys.Max(point => point.Y); y++)
            {
                string line = y.ToString();
                line.PadLeft(4, ' ');

                for (int x = map.Keys.Min(point => point.X); x <= map.Keys.Max(point => point.X); x++)
                {
                    Point newpoint = new Point(x, y);
                    char c = ' ';
                    if (map.ContainsKey(newpoint))
                    {
                        if (Convert.ToInt32(map[newpoint]) == 1)
                            c = '#';
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

        private static long run(Dictionary<long, long> items, Queue<long> inputs, Queue<long> results, ref long pointer, ref long relativebase)
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
