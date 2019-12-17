using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace aoc2019
{
    public class day16
    {
        public static void Go()
        {
            string input = "59754835304279095723667830764559994207668723615273907123832849523285892960990393495763064170399328763959561728553125232713663009161639789035331160605704223863754174835946381029543455581717775283582638013183215312822018348826709095340993876483418084566769957325454646682224309983510781204738662326823284208246064957584474684120465225052336374823382738788573365821572559301715471129142028462682986045997614184200503304763967364026464055684787169501819241361777789595715281841253470186857857671012867285957360755646446993278909888646724963166642032217322712337954157163771552371824741783496515778370667935574438315692768492954716331430001072240959235708";

            int[] inputa = input.ToCharArray().Select(t => Convert.ToInt32(t)-48).ToArray();
            int[] pattern = new int[] { 0, 1, 0, -1 };

            int[] A = solvea(inputa);
            Console.WriteLine(string.Join("", A.Take(8).Select(b => b.ToString())));

            var offset = int.Parse(input.Substring(0, 7));

            StringBuilder repeated = new StringBuilder();
            for (int i = 0; i < 10000; i++)
                repeated.Append(input);

            var bigstring = repeated.ToString().Select(c => int.Parse(c.ToString())).ToArray();

            var output = new int[bigstring.Length - offset];

            Array.Copy(bigstring, offset, output, 0, output.Length);

            for (int k = 0; k<100; k++)
            {
                for (var i = output.Length - 2; i > -1; i--)
                {
                    output[i] += output[i + 1];
                    output[i] %= 10; 
                }
            }

            Console.WriteLine(string.Join("", output.Take(8).Select(b => b.ToString())));
        }

        public static int[] solvea(int[] inputa)
        {
            int[] pattern = new int[] { 0, 1, 0, -1 };


            int step = 1;
            List<int> multiplierstouse;
            Dictionary<int, List<int>> cache = new Dictionary<int, List<int>>();

            while (step < 101)
            {
                int[] newval = new int[inputa.Length];
                for (int j = 1; j <= inputa.Length; j++)
                {
                    int pos = 1;
                    int res = 0;

                    if (!cache.ContainsKey(j))
                    {
                        cache.Add(j, multiplier(pattern, inputa.Length, j));
                    }
                    multiplierstouse = cache[j];

                    foreach (int i in inputa)
                    {
                        int mp = multiplierstouse[pos - 1];
                        res += (mp * i);
                        pos++;
                    }
                    newval[j-1] = Math.Abs(res % 10);
                }
                inputa = newval;
                step++;
            }
            return inputa;
        }

        public static List<int> multiplier(int[] basePattern, int lengthOfList, int numbersToUse)
        {
            var result = new List<int>();

            while (result.Count <= lengthOfList)
            {
                foreach (int item in basePattern)
                {
                    for (int i = 0; i < numbersToUse; i++)
                    {
                        result.Add(item);
                    }
                }
            }

            result.RemoveAt(0);
            return result;
        }
    }
}