using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;

namespace aoc2019
{
    public class day7
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day7a.txt").First();
            List<int> items = input.Split(',').Select(Int32.Parse).ToList();
            int highestA = int.MinValue;
            int highestB = int.MinValue;
            string codeA = string.Empty;
            string codeB = string.Empty;

            for (int j = 0; j < 100000; j++)
            {
                string s = j.ToString().PadLeft(5, '0');
                if (isvalid(s, new char[] { '0', '1', '2', '3', '4' }))
                {
                    int prev = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        Queue inputst = new Queue();
                        Queue tmp = new Queue();
                        inputst.Enqueue(Convert.ToInt16(s[i] - '0'));
                        inputst.Enqueue(prev);
                        int pointer = 0;

                        prev = run(new List<int>(items), inputst, tmp, ref pointer);
                    }
                    if (prev > highestA)
                    {
                        codeA = s;
                        highestA = prev;
                    }
                }
                if (isvalid(s, new char[] { '5', '6', '7', '8', '9' }))
                {
                    List<int>[] ic = new List<int>[5];
                    Queue[] inputq = new Queue[5];
                    int[] pointers = new int[] { 0, 0, 0, 0, 0 };
                    for (int i = 0; i < 5; i++)
                    {
                        ic[i] = new List<int>(items);
                        inputq[i] = new Queue();
                        inputq[i].Enqueue(Convert.ToInt16(s[i] - '0'));
                    }
                    inputq[0].Enqueue(0);
                    int lastoutput = 0;
                    while (lastoutput != int.MinValue)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (i < 4)
                                lastoutput = run(ic[i], inputq[i], inputq[i + 1], ref pointers[i]);
                            else
                                lastoutput = run(ic[i], inputq[i], inputq[0], ref pointers[i]);
                        }
                    }
                    int res = Convert.ToInt32(inputq[0].Dequeue());
                    if (res > highestB)
                    {
                        highestB = res;
                        codeB = s;
                    }
                }
            }
            Console.WriteLine(string.Format("AnswerA: {0} (phase setting sequence: {1})", highestA.ToString().PadLeft(9, ' '), codeA));
            Console.WriteLine(string.Format("AnswerB: {0} (phase setting sequence: {1})", highestB.ToString().PadLeft(9, ' '), codeB));

            if (1 == 1)
            {
                Console.WriteLine("");
                Console.WriteLine("Part I:");
                Console.WriteLine(("A-" + codeA[0]).PadLeft(10, ' ') + ("B-" + codeA[1]).PadLeft(14, ' ') + ("C-" + codeA[2]).PadLeft(14, ' ') + ("D-" + codeA[3]).PadLeft(14, ' ') + ("E-" + codeA[4]).PadLeft(14, ' '));
                int prev = 0;
                string o = "";
                for (int i = 0; i < 5; i++)
                {
                    Queue inputst = new Queue();
                    Queue tmp = new Queue();
                    inputst.Enqueue(Convert.ToInt16(codeA[i] - '0'));
                    inputst.Enqueue(prev);
                    int pointer = 0;

                    prev = run(new List<int>(items), inputst, tmp, ref pointer);
                    if (o.Length > 0)
                        o += " -> ";
                    else
                    {
                        o = "0 ->";
                    }
                    if (i == 0)
                    {
                        o += prev.ToString().PadLeft(6, ' ');
                    }
                    else
                    {
                        if (prev != int.MinValue)
                            o += prev.ToString().PadLeft(10, ' ');
                        else
                            o += "STOP".PadLeft(10, ' ');
                    }
                }
                Console.WriteLine(o);
                Console.WriteLine("");

                Console.WriteLine("Part II:");
                Console.WriteLine(("A-" + codeB[0]).PadLeft(10, ' ') + ("B-" + codeB[1]).PadLeft(14, ' ') + ("C-" + codeB[2]).PadLeft(14, ' ') + ("D-" + codeB[3]).PadLeft(14, ' ') + ("E-" + codeB[4]).PadLeft(14, ' '));



                List<int>[] ic = new List<int>[5];
                Queue[] inputq = new Queue[5];
                int[] pointers = new int[] { 0, 0, 0, 0, 0 };
                for (int i = 0; i < 5; i++)
                {
                    ic[i] = new List<int>(items);
                    inputq[i] = new Queue();
                    inputq[i].Enqueue(Convert.ToInt16(codeB[i] - '0'));
                }
                inputq[0].Enqueue(0);
                int lastoutput = 0;
                int loop = 0;
                int x = 0;
                while (lastoutput != int.MinValue)
                {
                    o = "";
                    for (int i = 0; i < 5; i++)
                    {
                        if (i < 4)
                            lastoutput = run(ic[i], inputq[i], inputq[i + 1], ref pointers[i]);
                        else
                            lastoutput = run(ic[i], inputq[i], inputq[0], ref pointers[i]);
                        if (o.Length > 0)
                        {
                            if (lastoutput != int.MinValue)
                                o += " -> ";
                            else
                                o += "    ";
                        }
                        else
                        {
                            if (loop == 0)
                            {
                                o = "0 ->";
                            }
                        }
                        if (x == 0)
                        {
                            o += lastoutput.ToString().PadLeft(6, ' ');
                            x = 1;
                        }
                        else
                        {
                            if (lastoutput != int.MinValue)
                                o += lastoutput.ToString().PadLeft(10, ' ');
                            else
                                o += "STOP".PadLeft(10, ' ');
                        }
                    }
                    loop++;
                    Console.WriteLine(o);
                }
            }
            Console.WriteLine("");

        }

        private static bool isvalid(string input, char[] validvalues)
        {
            List<char> te = new List<char>();
            foreach (char c in input)
            {
                if (validvalues.Contains(c))
                {
                    if (te.Contains(c))
                        return false;
                    else
                        te.Add(c);
                }
                else
                    return (false);
            }
            return true;
        }

        private static int getparam(List<int> items, int paramnum, int pointer, bool position)
        {
            if (position)
                return items[items[pointer + paramnum]];
            else
                return items[pointer + paramnum];
        }
        private static int run(List<int> items, Queue inputs, Queue results, ref int pointer)
        {
            int param1, param2, steps;
            param1 = param2 = steps = 0;
            while (true)
            {
                steps += 1;
                string opcodes = items[pointer].ToString().PadLeft(5, '0');
                int opcode = int.Parse(opcodes.Substring(3, 2));

                if (new[] { 1, 2, 4, 5, 6, 7, 8 }.Contains(opcode))
                    param1 = getparam(items, 1, pointer, (opcodes[2] - 48) == 0);
                if (new[] { 1, 2, 5, 6, 7, 8 }.Contains(opcode))
                    param2 = getparam(items, 2, pointer, (opcodes[1] - 48) == 0);

                switch (opcode)
                {
                    case 99:
                        //Console.WriteLine(string.Format("FINISHED IN {0} STEPS", steps));
                        return int.MinValue;
                    case 1:
                        items[items[pointer + 3]] = param1 + param2;
                        break;
                    case 2:
                        items[items[pointer + 3]] = param1 * param2;
                        break;
                    case 3:
                        items[items[pointer + 1]] = Convert.ToInt32(inputs.Dequeue());
                        break;
                    case 4:
                        //Console.WriteLine(param1);
                        results.Enqueue(param1);
                        pointer += 2;
                        return (param1);
                    case 5:
                        if (param1 != 0)
                            pointer = param2;
                        else
                            pointer += 3;
                        break;
                    case 6:
                        if (param1 == 0)
                            pointer = param2;
                        else
                            pointer += 3;
                        break;
                    case 7:
                        if (param1 < param2)
                            items[items[pointer + 3]] = 1;
                        else
                            items[items[pointer + 3]] = 0;
                        break;
                    case 8:
                        if (param1 == param2)
                            items[items[pointer + 3]] = 1;
                        else
                            items[items[pointer + 3]] = 0;
                        break;
                    default:
                        break;
                }
                if (new[] { 1, 2, 7, 8 }.Contains(opcode))
                    pointer += 4;
                if (new[] { 3, 4 }.Contains(opcode))
                    pointer += 2;
            }
        }
    }
}
