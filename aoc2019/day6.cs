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
            Console.WriteLine(string.Format("AnswerA: {0}", mylist.Keys.ToList().Select(i => path(i, mylist).Count-1).Sum()));
            HashSet<string> pathofSAN = path("SAN", mylist);
            pathofSAN.SymmetricExceptWith(path("YOU", mylist));
            Console.WriteLine(string.Format("AnswerB: {0}", pathofSAN.Count-2 ));
        }

        private static HashSet<string> path(string start, Dictionary<string, string> mylist)
        {
            List<string> result = new List<string>() { start };
            while(result[result.Count-1] != "COM")
                result.Add(mylist[result[result.Count - 1]]);
            return result.ToHashSet();
        }
    }
}