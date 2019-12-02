using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class day2
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day2.txt").First();
            List<int> items = input.Split(',').Select(Int32.Parse).ToList();

            items[1] = 12;
            items[2] = 2;

            Console.WriteLine(run(items).ToString());

            bool stop = false;
            int noun = 1;
            int verb = 1;

            while (!stop)
            {
                items = input.Split(',').Select(Int32.Parse).ToList();
                items[1] = noun;
                items[2] = verb;

                if (run(items) == 19690720)
                    stop = true;
                else
                {
                    if (noun == 99)
                    {
                        verb += 1;
                        noun = 1;
                    }
                    else
                        noun += 1;
                }
            }
            Console.WriteLine(noun*100 + verb);
        }

        private static int run(List<int> items)
        {
            int pointer = 0;
            while (true)
            {
                int opcode = items[pointer];
                switch (opcode)
                {
                    case 99:
                        return items[0];
                    case 1:
                        items[items[pointer + 3]] = items[items[pointer + 1]] + items[items[pointer + 2]];
                        pointer += 4;
                        break;
                    case 2:
                        items[items[pointer + 3]] = items[items[pointer + 1]] * items[items[pointer + 2]];
                        pointer += 4;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
