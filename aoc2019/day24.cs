using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    class day24
    {
        public static void Go()
        {
            var input = File.ReadLines(@"c:\github\aoc2019\input\day24.txt");

            int x;
            int y = 0;
            List<(int x, int y)> mapa = new List<(int x, int y)>();
            List<(int x, int y, int z)> mapb = new List<(int x, int y, int z)>();
            foreach (string s in input)
            {
                x = 0;
                foreach (char c in s)
                {
                    if (c == '#')
                    {
                        mapb.Add((x, y, 0));
                        mapa.Add((x, y));
                    }
                    x += 1;
                }
                y += 1;
            }

            List<int> bio = new List<int>();
            bio.Add(biodiversity(mapa));
            bool stop = false;
            while (!stop)
            {
                List<(int x, int y)> next = new List<(int x, int y)>();
                for (int xa = 0; xa < 5; xa++)
                {
                    for (int ya = 0; ya < 5; ya++)
                    {
                        bool add = false;
                        int near = 0;
                        if (xa > 0 && mapa.Contains((xa - 1, ya))) 
                            near++;
                        if (xa < 4 && mapa.Contains((xa + 1, ya)))
                            near++;
                        if (ya > 0 && mapa.Contains((xa, ya - 1)))
                            near++;
                        if (ya < 4 && mapa.Contains((xa, ya + 1)))
                            near++;

                        if (near == 1)
                            add = true;
                        else if (!mapa.Contains((xa, ya)) && near == 2)
                            add = true;
                        if (add)
                            next.Add((xa, ya));
                    }
                }
                int bioc = biodiversity(next);
                if (bio.Contains(bioc))
                {
                    Console.WriteLine("AnswerA: " + bioc);
                    stop = true;
                }
                else
                    bio.Add(bioc);
                mapa = next;
            }

            stop = false;
            int minz = 0;
            int maxz = 0;
            for (int n = 0; n < 200; n++)
            {
                List<(int x, int y, int z)> next = new List<(int x, int y, int z)>();
                for (int z = minz - 1; z <= maxz + 1; z++)
                {
                    for (int xb = 0; xb < 5; xb++)
                    {
                        for (int yb = 0; yb < 5; yb++)
                        {
                            if (yb == 2 && xb == 2)
                                continue;
                            int near = 0;
                            if (yb == 2 && xb == 1)
                                for (int k = 0; k < 5; k++)
                                    if (mapb.Contains((k, 0, z - 1)))
                                        near++;
                            if (yb == 2 && xb == 3)
                                for (int k = 0; k < 5; k++)
                                    if (mapb.Contains((k, 4, z - 1)))
                                        near++;
                            if (yb == 1 && xb == 2)
                                for (int k = 0; k < 5; k++)
                                    if (mapb.Contains((0, k, z - 1)))
                                        near++;
                            if (yb == 3 && xb == 2)
                                for (int k = 0; k < 5; k++)
                                    if (mapb.Contains((4, k, z - 1)))
                                        near++;
                            if (yb > 0 && mapb.Contains((yb - 1, xb, z)))
                                near++;
                            if (yb < 4 && mapb.Contains((yb + 1, xb, z)))
                                near++;
                            if (xb > 0 && mapb.Contains((yb, xb - 1, z)))
                                near++;
                            if (xb < 4 && mapb.Contains((yb, xb + 1, z)))
                                near++;
                            if (yb == 0 && mapb.Contains((1, 2, z + 1)))
                                near++;
                            if (yb == 4 && mapb.Contains((3, 2, z + 1)))
                                near++;
                            if (xb == 0 && mapb.Contains((2, 1, z + 1)))
                                near++;
                            if (xb == 4 && mapb.Contains((2, 3, z + 1)))
                                near++;

                            bool add = false;
                            if (near == 1)
                                add = true;
                            else if (!mapb.Contains((yb, xb, z)) && near == 2)
                                add = true;
                            if (add)
                            {
                                next.Add((yb, xb, z));
                                if (z > maxz)
                                    maxz = z;
                                if (z < minz)
                                    minz = z;
                            }
                        }
                    }
                }
                mapb = next;
            }
            Console.WriteLine("ResultB: " + mapb.Count);
        }

        public static int biodiversity(List<(int x,int y)> map)
        {
            int count = 0;
            int result = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (map.Contains((x, y)))
                        result += Convert.ToInt32(Math.Pow(2, count));
                    count++;
                }
            }
            return result;
        }

        public static void print(List<(int x, int y)> map)
        {
            for (int y = 0; y < 5; y++)
            {
                string line = "";
                for (int x = 0; x < 5; x++)
                {
                    if (map.Contains((x, y)))
                        line += "#";
                    else
                        line += ".";
                }
                Console.WriteLine(line);
            }
        }
    }
}
