using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;

namespace aoc2019
{
    public class day9
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day9.txt").First();
            Dictionary<int, long> items = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s);
            Queue inputq = new Queue();
            Queue outputq = new Queue();
            inputq.Enqueue(1);
            Console.WriteLine("PartA");
            run(new Dictionary<int, long>(items), inputq, outputq);
            while (outputq.Count > 0)
                Console.WriteLine(outputq.Dequeue());
            inputq.Enqueue(2);
            Console.WriteLine("PartB");
            run(items, inputq, outputq);
            while (outputq.Count > 0)
                Console.WriteLine(outputq.Dequeue());
        }

        private static int getpointer(Dictionary<int, long> items, int paramnum, int pointer, int mode, int relativebase)
        {
            int p;
            if (mode == 0)
                p = Convert.ToInt32(items[pointer + paramnum]);
            else if (mode == 1)
                p = pointer + paramnum;
            else
                p = Convert.ToInt32(items[pointer + paramnum] + relativebase);
            if (!items.ContainsKey(p))
                items.Add(p, 0);
            return p;
        }

        private static int run(Dictionary<int, long> items, Queue inputs, Queue results)
        {
            long param1, param2;
            int steps, relativebase, pointer;
            param1 = param2 = steps = relativebase = pointer = 0;
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
                        //   Console.WriteLine(string.Format("FINISHED IN {0} STEPS", steps));
                        return int.MinValue;
                    case 1:
                        items[getpointer(items, 3, pointer, mode3, relativebase)] = param1 + param2;
                        break;
                    case 2:
                        items[getpointer(items, 3, pointer, mode3, relativebase)] = param1 * param2;
                        break;
                    case 3:
                        items[getpointer(items, 1, pointer, mode1, relativebase)] = Convert.ToInt32(inputs.Dequeue());
                        break;
                    case 4:
                        //  Console.WriteLine(param1);
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
                if (new[] { 3, 9 }.Contains(opcode))
                    pointer += 2;
            }
        }
    }
}