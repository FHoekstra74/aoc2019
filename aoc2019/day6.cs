using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public static class day6
    {
        public static void Go()
        {
            Dictionary<string, string> mylist = File.ReadLines(@"c:\github\aoc2019\input\day6.txt").ToDictionary(i => i.Split(')')[1], i => i.Split(')')[0]);
            Console.WriteLine(string.Format("AnswerA: {0}", mylist.Keys.ToList().Select(i => path(i, mylist).Count).Sum()));
            List<string> pathofSAN = path("SAN", mylist);
            List<string> pathofYOU = path("YOU", mylist);
            Console.WriteLine(string.Format("AnswerB: {0}", pathofYOU.IndexOf(pathofSAN.Intersect(pathofYOU).First()) + pathofSAN.IndexOf(pathofSAN.Intersect(pathofYOU).First())));
        }

        private static List<string> path(string start, Dictionary<string, string> mylist)
        {
            List<string> result = new List<string>();
            string pos = start;
            while (pos != "COM")
            {
                pos = mylist[pos];
                result.Add(pos);
            }
            return result;
        }
    }
}
