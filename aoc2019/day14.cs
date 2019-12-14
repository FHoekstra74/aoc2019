using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace aoc2019
{
    public class day14
    {
        public static void Go()
        {
            var input = File.ReadLines(@"c:\github\aoc2019\input\day14.txt");
            List<reactrion> reactions = new List<reactrion>();
            //Hm, maybe time to learn some regex magic? for now oldschool splits:
            foreach (string s in input)
            {
                string[] test = s.Split('=');
                reactrion re = new reactrion();
                re.result = test[1].Replace('>', ' ').Trim().Split(' ')[1].Trim();
                re.resultcoint = Convert.ToInt32(test[1].Replace('>', ' ').Trim().Split(' ')[0].Trim());
                foreach (string i in test[0].Trim().Split(','))
                {
                    string[] j = i.Trim().Split(' ');
                    re.ingredients.Add(new checmical(j[1].Trim(), Convert.ToInt32(j[0].Trim())));
                }
                reactions.Add(re);
            }

            Console.WriteLine(string.Format("AnswerA: {0}", neededore(1, reactions)));
            int low = 0;
            int high = 1;
            long target = 1000000000000;
            while (neededore(high, reactions) < target)
            {
                low = high;
                high *= 2;
            }
            int mid;
            while (Math.Abs(high-low)>1)
            {
                mid = (high + low) / 2;
                long needed = neededore(mid, reactions);
                if (needed > target)
                    high = mid;
                else
                    low = mid;
            }
            Console.WriteLine(string.Format("AnswerB: {0}", low));
        }

        private static long neededore(int fuel, List<reactrion> reactions)
        {
            List<checmical> needed = new List<checmical>();
            Dictionary<string, long> leftover = new Dictionary<string, long>();

            needed.Add(new checmical("FUEL", fuel));
            long ore = 0;

            while (needed.Count > 0)
            {
                checmical need = needed[0];
                if (leftover.ContainsKey(need.name))
                {
                    if (leftover[need.name] > need.quantity)
                    {
                        leftover[need.name] -= need.quantity;
                        need.quantity = 0;
                    }
                    else
                    {
                        need.quantity -= leftover[need.name];
                        leftover.Remove(need.name);
                    }
                }
                if (need.quantity > 0)
                {
                    var react = reactions.Find(re => re.result == need.name);
                    long count = (need.quantity + (react.resultcoint - 1)) / react.resultcoint;
                    need.quantity -= (react.resultcoint * count);
                    if (need.quantity < 0)
                    {
                        if (leftover.ContainsKey(need.name))
                            leftover[need.name] += (need.quantity * -1);
                        else
                            leftover.Add(need.name, need.quantity * -1);
                    }
                    foreach (checmical ingr in react.ingredients)
                    {
                        if (ingr.name.Equals("ORE"))
                            ore += (ingr.quantity * count);
                        else
                            needed.Add(new checmical(ingr.name, ingr.quantity * count));
                    }
                }
                needed.RemoveAt(0);
            }
            return ore;
        }

        public class checmical
        {
            public string name;
            public long quantity;
            public checmical(string nam, long quant)
            {
                name = nam;
                quantity = quant;
            }
        }

        public class reactrion
        {
            public string result;
            public int resultcoint;
            public List<checmical> ingredients;
            public reactrion()
            {
                ingredients = new List<checmical>();
            }
        }
    }
}
