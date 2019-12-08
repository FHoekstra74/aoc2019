using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day8
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day8.txt").First();
            int width = 25;
            int height = 6;
            string[] layers = Enumerable.Range(0, input.Length / (width * height)).Select(i => input.Substring(i * width * height, width * height)).ToArray();
            string thelayer = layers.Where(s => s.Count(f => f == '0') == layers.Min(s => s.Count(f => f == '0'))).First();
            Console.WriteLine(string.Format("AnswerA {0}", thelayer.Count(f => f == '1') * thelayer.Count(f => f == '2')));
            Console.WriteLine("AnswerB:");
            for (int i = 0; i < height; i++)
            {
                string line = "";
                for (int j = 0; j < width; j++)
                {
                    int pos = (width * i) + j;
                    foreach (string layer in layers)
                    {
                        if (layer[pos] != '2')
                        {
                            if (layer[pos] == '0')
                                line += ' ';
                            else
                                line += 'X';
                            break;
                        }
                    }
                }
                Console.WriteLine(line);
            }
        }
    }
}
