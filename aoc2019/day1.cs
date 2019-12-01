using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class day1
    {
        public static void Go()
        {
            var values = File.ReadAllLines(@"c:\github\aoc2019\input\day1.txt").Select(line => int.Parse(line));
            int resultA = 0;
            int resultB = 0;
            foreach (int i in values)
            {
                resultA += Fuel(i);
                resultB += TotalFuel(i);
            }
            Console.WriteLine(resultA.ToString());
            Console.WriteLine(resultB.ToString());
        }

        private static int TotalFuel(int mass)
        {
            int total = 0;
            int val = Fuel(mass);
            while (val>0)
            {
                total += val;
                val = Fuel(val);
            }
            return total;
        }
        private static int Fuel(int mass)
        {
            return mass / 3 - 2;
        }
    }
}
