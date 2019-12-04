using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Drawing;

namespace aoc2019
{
    public class day4
    {
        public static void Go()
        {
            int count1 = 0;
            int count2 = 0;

            for (int i= 158126; i<= 624574; i++)
            {
                if (isvalid(i))
                {
                    count1 += 1;
                    if (isvalid2(i))
                        count2 += 1;
                }
            }
            Console.WriteLine(count1);
            Console.WriteLine(count2);
        }

        private static bool isvalid(int password)
        {
            var chars = password.ToString().Select(x => int.Parse(x.ToString())).ToArray();
            bool twosame = false;
            for (int i = 1; i < 6; i++)
            {
                if (chars[i - 1] == chars[i]) twosame = true;
                if (chars[i - 1] > chars[i]) return false;
            }
            return twosame;
        }

        private static bool isvalid2(int password)
        {
            var chars = password.ToString().Select(x => int.Parse(x.ToString())).ToArray();
            var dictionary = new Dictionary<int, int>();
            for (int i = 0; i < 6; i++)
            {
                if (dictionary.ContainsKey(chars[i]))
                    dictionary[chars[i]] = dictionary[chars[i]] + 1;
                else
                    dictionary.Add(chars[i], 1);
            }
            return dictionary.Any(ch => ch.Value == 2);
        }
    }
}
