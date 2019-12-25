using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace aoc2019
{
    public class day22
    {
        public static void Go()
        {
            var instrunctions = File.ReadLines(@"c:\github\aoc2019\input\day22.txt");

            long nrcards = 10007;
            long[] cards = new long[nrcards];
            for (long i = 0; i < cards.Length; i++)
                cards[i] = i;

            foreach (string s in instrunctions)
            {
                if (s.Equals("deal into new stack"))
                    Array.Reverse(cards);
                if (s.StartsWith("cut"))
                {
                    int n = Convert.ToInt32(s.Substring(3).Trim());
                    long[] help = new long[Math.Abs(n)];

                    if (n > 0)
                    {
                        for (long i = 0; i < n; i++)
                            help[i] = cards[i];
                        for (long i = n; i < cards.Length; i++)
                            cards[i - n] = cards[i];
                        for (long i = 0; i < n; i++)
                            cards[cards.Length - n + i] = help[i];
                    }
                    else
                    {
                        for (long i = cards.Length + n; i < cards.Length; i++)
                            help[i - cards.Length - n] = cards[i];
                        for (long i = cards.Length - 1; i >= Math.Abs(n); i--)
                            cards[i] = cards[i + n];
                        for (long i = 0; i < Math.Abs(n); i++)
                            cards[i] = help[i];
                    }
                }
                if (s.StartsWith("deal with increment"))
                {
                    int n = Convert.ToInt32(s.Substring(19).Trim());
                    long[] copy = (long[])cards.Clone();
                    for (long i = 0; i < cards.Length; i++)
                    {
                        long newpos = (i * n);
                        while (newpos > cards.Length)
                            newpos = newpos - cards.Length;
                        cards[newpos] = copy[i];
                    }
                }
            }

            for (long i = 0; i < cards.Length; i++)
                if (cards[i] == 2019)
                    Console.WriteLine("AnswerA:" + i);

            //For partB I had some help from Reddit r/adventofcode posts! My math is a bit rusty....
            nrcards = 119315717514047;
            BigInteger bignrcards = new BigInteger(nrcards);
            BigInteger offset = new BigInteger(0);
            BigInteger increment = new BigInteger(1);

            foreach (string s in instrunctions)
            {
                if (s.Equals("deal into new stack"))
                {
                    increment *= -1;
                    offset += increment;
                }
                if (s.StartsWith("cut"))
                {
                    int n = Convert.ToInt32(s.Substring(3).Trim());
                    offset += n * increment;
                }
                if (s.StartsWith("deal with increment"))
                {
                    int n = Convert.ToInt32(s.Substring(19).Trim());
                    increment *= BigInteger.ModPow(n, bignrcards - 2, bignrcards);
                }

                increment = myMod(increment, bignrcards);
                offset = myMod(offset, bignrcards);
            }

            BigInteger iterations = new BigInteger(101741582076661);
            BigInteger incr = BigInteger.ModPow(increment, iterations, bignrcards);
            offset = myMod( (offset * (1 - incr) * BigInteger.ModPow((1 - increment) % bignrcards, bignrcards - 2, bignrcards)) ,bignrcards);
            Console.WriteLine("AnswerB:" + (myMod(  (offset + 2020 * incr) , bignrcards)).ToString());
        }

        private static BigInteger myMod(BigInteger x, BigInteger m)
        {
            //Own mod function, c# turns mad in case of an out of the box mod of two bigintegers!
            return (x % m + m) % m;
        }
    }
}
