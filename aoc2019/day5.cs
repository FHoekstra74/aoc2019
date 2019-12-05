using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class day5
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day5.txt").First();
            List<int> items = input.Split(',').Select(Int32.Parse).ToList();
            Console.WriteLine("PartA:");
            run(items, 1);
            Console.WriteLine("PartB:");
            items = input.Split(',').Select(Int32.Parse).ToList();
            run(items, 5);
        }

        private static int getparam(List<int> items, int paramnum, int pointer, bool position)
        {
            if (position)
                return items[items[pointer + paramnum]];
            else
                return items[pointer + paramnum];
        }

        private static int run(List<int> items, int machineocode)
        {
            int pointer = 0;
            List<int> param1needed = new List<int> { 1, 2, 4, 5, 6, 7, 8 };
            List<int> param2needed = new List<int> { 1, 2, 5, 6, 7, 8 };

            while (true)
            {
                string opcodes = items[pointer].ToString();

                while (opcodes.Length < 5)
                    opcodes = "0" + opcodes;

                int mode1 = int.Parse(opcodes.Substring(2, 1));
                int mode2 = int.Parse(opcodes.Substring(1, 1));
                int mode3 = int.Parse(opcodes.Substring(0, 1));
                int opcode = int.Parse(opcodes.Substring(3, 2));

                int param1=0;
                int param2=0;

                if (param1needed.Contains(opcode))
                    param1 = getparam(items, 1, pointer, mode1 == 0);
                if (param2needed.Contains(opcode))
                    param2 = getparam(items, 2, pointer, mode2 == 0);

                switch (opcode)
                {
                    case 99:
                        Console.WriteLine("STOP");
                        return 0;
                    case 1:
                        items[items[pointer + 3]] = param1 + param2;
                        pointer += 4;
                        break;
                    case 2:
                        items[items[pointer + 3]] = param1 * param2;
                        pointer += 4;
                        break;
                    case 3:
                        items[items[pointer + 1]] = machineocode;
                        pointer += 2;
                        break;
                    case 4:
                        Console.WriteLine(param1);
                        pointer += 2;
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
                        pointer += 4;
                        break;
                    case 8:
                        if (param1 == param2)
                            items[items[pointer + 3]] = 1;
                        else
                            items[items[pointer + 3]] = 0;
                        pointer += 4;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
