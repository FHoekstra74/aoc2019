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
            string input = File.ReadLines(@"c:\github\aoc2019\input\day7.txt").First();
            List<int> items = input.Split(',').Select(Int32.Parse).ToList();
            int highestA = int.MinValue;
            int highestB = int.MinValue;
            for (int j = 0; j < 100000; j++)
            {
                string s = j.ToString().PadLeft(5,'0');
                if (isvalid(s,new char[] {'0','1','2','3','4'}))
                {
                    char[] inputs = s.ToCharArray();
                    int prev = 0;
                    foreach (char i in inputs)
                    {
                        Queue inputst = new Queue();
                        Queue tmp = new Queue();
                        inputst.Enqueue(Convert.ToInt16(i - '0'));
                        inputst.Enqueue(prev);
                        int pointer = 0;
                        prev = run(new List<int>(items), inputst, tmp, ref pointer);
                    }
                    if (prev > highestA)
                        highestA = prev;
                }
                if (isvalid(s, new char[] { '5', '6', '7', '8', '9' }))
                {
                    char[] inputs = s.ToCharArray();
                    List<int>[] ic = new List<int>[5];
                    Queue[] inputq = new Queue[5];
                    int[] pointers = new int[] { 0, 0, 0, 0, 0 };
                    for (int k = 0; k < 5; k++)
                    {
                        ic[k] = new List<int>(items);
                        inputq[k] = new Queue();
                        inputq[k].Enqueue(Convert.ToInt16(inputs[k] - '0'));
                    }
                    inputq[0].Enqueue(0);
                    int lastoutput = 0;
                    while (lastoutput != int.MinValue)
                    {
                        run(ic[0], inputq[0], inputq[1], ref pointers[0]);
                        run(ic[1], inputq[1], inputq[2], ref pointers[1]);
                        run(ic[2], inputq[2], inputq[3], ref pointers[2]);
                        run(ic[3], inputq[3], inputq[4], ref pointers[3]);
                        lastoutput = run(ic[4], inputq[4], inputq[0], ref pointers[4]);
                    }
                    int res = Convert.ToInt32(inputq[0].Dequeue());
                    if (res > highestB)
                        highestB = res;
                }
            }
            Console.WriteLine(string.Format("AnswerA: {0}", highestA));
            Console.WriteLine(string.Format("AnswerB: {0}", highestB));
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
            int  param1, param2, steps;
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
