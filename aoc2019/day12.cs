using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class day12
    {
        public static void Go()
        {
            var input = File.ReadLines(@"c:\github\aoc2019\input\day12.txt");
            List<Moon> moons = new List<Moon>(); 
            foreach (string s in input)
            {
                string i = s;
                foreach (var c in new string[] { "<", ">", "x", "y", "z", "=" })
                    i = i.Replace(c, string.Empty);
                string[] hulp = i.Split(','); 
                Moon moon = new Moon(Convert.ToInt32(hulp[0].Trim()), Convert.ToInt32(hulp[1].Trim()), Convert.ToInt32(hulp[2].Trim()));
                moons.Add(moon);
            }

            var moonsA = moons.Select(s => new Moon(s.x, s.y, s.z)).ToList();
            for (var i = 0; i < 1000; i++)
            {
                foreach (Moon x in moonsA)
                    foreach (Moon m2 in moonsA)
                        x.updateVelocity(m2);
                foreach (Moon m in moonsA)
                    m.updatePosition();
            }
            Console.WriteLine(string.Format("AnswerA: {0}", moonsA.Sum(m => m.energie())));
            Console.WriteLine(string.Format("AnserB: {0}", LowestCommonMultiplier(new long[] { findrepeat(moons, "X"), findrepeat(moons, "Y"), findrepeat(moons, "Z") })));
        }

        private static long findrepeat(List<Moon> input, string dimension)
        {
            var moons = input.Select(s => new Moon(s.x, s.y, s.z)).ToList();
            long loops = 1;
            while (true)
            {
                foreach (Moon x in moons)
                    foreach (Moon m2 in moons)
                        x.updateVelocity(m2);
                foreach (Moon m in moons)
                    m.updatePosition();
                bool test = true;
                foreach (Moon m in moons)
                    if (!m.initialpos(dimension))
                        test = false;
                if (test)
                    return loops;
                loops++;
            } 
        }

        public class Moon
        {
            public int x;
            public int velx;
            public int y;
            public int vely;
            public int z;
            public int velz;
            public int xorg;
            public int yorg;
            public int zorg;

            public Moon(int X, int Y, int Z)
            {
                x = X;
                y = Y;
                z = Z;
                xorg = X;
                yorg = Y;
                zorg = Z;
            }

            public void print()
            {
                Console.WriteLine ("Pos {0},{1},{2} Vel {3},{4},{5}", x, y, z, velx, vely, velz);
            }

            public bool initialpos(string pos)
            {
                if (pos == "X")
                    return (x == xorg && velx == 0);
                else if (pos == "Y")
                    return (y == yorg && vely == 0);
                else
                    return (z == zorg && velz == 0);
            }

            public int energie () 
            {
                return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) * (Math.Abs(velx) + Math.Abs(vely) + Math.Abs(velz));
            }

            public void updateVelocity(Moon m)
            {
                if (x > m.x)
                    velx -= 1;
                else if (x < m.x)
                    velx += 1;
                if (y > m.y)
                    vely -= 1;
                else if (y < m.y)
                    vely += 1;
                if (z > m.z)
                    velz -= 1;
                else if (z < m.z)
                    velz += 1;
            }

            public void updatePosition()
            {
                x += velx;
                y += vely;
                z += velz;
            }
        }

        //Thanks internet for GreatestCommonDenominator and LowestCommonMultiplier!
        private static long GreatestCommonDenominator(long a, long b)
        {
            return b == 0 ? a : GreatestCommonDenominator(b, a % b);
        }

        private static long LowestCommonMultiplier(long[] values)
        {
            switch (values.Length)
            {
                case 0:
                    return 0;
                case 1:
                    return values[0];
                case 2:
                    return values[0] / GreatestCommonDenominator(values[0], values[1]) * values[1];
                default:
                    return LowestCommonMultiplier(new[] { values[0], LowestCommonMultiplier(values.Skip(1).ToArray()) });
            }
        }
    }
}
