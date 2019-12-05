using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;

namespace aoc2019
{
    public class day5
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day5.txt").First();
            List<int> items = input.Split(',').Select(Int32.Parse).ToList();
            Console.WriteLine("PartA:");
            Stack inputs = new Stack();
            inputs.Push(1);
            run(new List<int>(items), inputs);
            inputs.Push(5);
            Console.WriteLine("PartB:");
            run(items, inputs);
        }

        private static int getparam(List<int> items, int paramnum, int pointer, bool position)
        {
            if (position)
                return items[items[pointer + paramnum]];
            else
                return items[pointer + paramnum];
        }

        private static int run(List<int> items, Stack inputs)
        {
            int pointer, param1, param2, steps;
            pointer = param1 = param2 = steps = 0;
            while (true)
            {
                steps += 1;
                string opcodes = items[pointer].ToString().PadLeft(5,'0');
                int opcode = int.Parse(opcodes.Substring(3, 2));

                if (new[] { 1, 2, 4, 5, 6, 7, 8 }.Contains(opcode))
                    param1 = getparam(items, 1, pointer, (opcodes[2] - 48) == 0);
                if (new[] { 1, 2, 5, 6, 7, 8 }.Contains(opcode))
                    param2 = getparam(items, 2, pointer, (opcodes[1] - 48) == 0);

                switch (opcode)
                {
                    case 99:
                        Console.WriteLine(string.Format("FINISHED IN {0} STEPS",steps));
                        return 0;
                    case 1:
                        items[items[pointer + 3]] = param1 + param2;
                        break;
                    case 2:
                        items[items[pointer + 3]] = param1 * param2;
                        break;
                    case 3:
                        items[items[pointer + 1]] = Convert.ToInt32(inputs.Pop());
                        break;
                    case 4:
                        Console.WriteLine(param1);
                        break;
                    case 5:
                        if (param1 != 0)
                            pointer = param2;
                        else
                            pointer += 3;
                        break;
                    case 6:
                        if (param1 == 0)
                            pointer = param2;
                        else
                            pointer += 3;
                        break;
                    case 7:
                        if (param1 < param2)
                            items[items[pointer + 3]] = 1;
                        else
                            items[items[pointer + 3]] = 0;
                        break;
                    case 8:
                        if (param1 == param2)
                            items[items[pointer + 3]] = 1;
                        else
                            items[items[pointer + 3]] = 0;
                        break;
                    default:
                        break;
                }
                if (new[] { 1, 2, 7, 8 }.Contains(opcode))
                    pointer += 4;
                if (new[] { 3, 4 }.Contains(opcode))
                    pointer += 2;
            }
        }
    }
}
