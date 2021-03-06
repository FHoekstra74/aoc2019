﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day23
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day23.txt").First();
            Dictionary<long, long> intcode = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            int nrcomputers = 50;
            Queue<long>[] inputq = new Queue<long>[nrcomputers];
            Queue<long>[] outputq = new Queue<long>[nrcomputers];
            Dictionary<long, long>[] computers = new Dictionary<long, long>[nrcomputers];
            long[] pointer = new long[nrcomputers];
            long[] pointer2 = new long[nrcomputers];

            for (int i = 0; i < nrcomputers; i++)
            {
                computers[i] = new Dictionary<long, long>(intcode);
                inputq[i] = new Queue<long>();
                outputq[i] = new Queue<long>();
                inputq[i].Enqueue(i);
                pointer[i] = 0;
                pointer2[i] = 0;
                run(computers[i], inputq[i], outputq[i], ref pointer[i], ref pointer2[i]);
            }
            bool stop = false;
            long natx = 0;
            long naty = 0;
            long lastnaty = 0;

            while (!stop)
            {
                for (int i = 0; i < nrcomputers; i++)
                {
                    while (outputq[i].Count > 0)
                    {
                        int a = Convert.ToInt32(outputq[i].Dequeue());
                        if (a == 255)
                        {
                            natx = outputq[i].Dequeue();
                            long test = outputq[i].Dequeue();
                            if (naty == 0)
                                Console.WriteLine("AnswerA:" + test);
                            naty = test;
                        }
                        else
                        {
                            inputq[a].Enqueue(outputq[i].Dequeue());
                            inputq[a].Enqueue(outputq[i].Dequeue());
                        }
                    }
                }
                int idlecount = 0;
                for (int i = 0; i < nrcomputers; i++)
                {
                    if (inputq[i].Count == 0)
                    {
                        inputq[i].Enqueue(-1);
                        idlecount++;
                    }
                }
                if (idlecount == nrcomputers && naty > 0)
                {
                    if (naty == lastnaty)
                    {
                        Console.WriteLine("AnswerB:" + naty);
                        stop = true;
                    }
                    inputq[0].Enqueue(natx);
                    inputq[0].Enqueue(naty);
                    lastnaty = naty;
                }
                for (int i = 0; i < nrcomputers; i++)
                    run(computers[i], inputq[i], outputq[i], ref pointer[i], ref pointer2[i]);
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
                            items[getpointer(items, 1, pointer, mode1, relativebase)] = Convert.ToInt64(inputs.Dequeue());
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
