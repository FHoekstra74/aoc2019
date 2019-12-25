using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    class day25
    {
        public static void Go()
        {
            string input = File.ReadLines(@"c:\github\aoc2019\input\day25.txt").First();
            Dictionary<long, long> intcode = input.Split(',').Select(Int64.Parse).Select((s, i) => new { s, i }).ToDictionary(x => Convert.ToInt64(x.i), x => x.s);
            Queue<long> inputq = new Queue<long>();
            Queue<long> outputq = new Queue<long>();
            Dictionary<long, long> computer = new Dictionary<long, long>(intcode);
            long pointer = 0;
            long pointer2 = 0;
            bool stop = false;

            List<string> inventory = new List<string>
            {
                "space heater",
                "shell",
                "jam",
                "asterisk",
                "klein bottle",
                "spool of cat6",
                "astronaut ice cream",
                "space law space brochure"
            };
            List<string> goods = inventory.Select(item => (string)item.Clone()).ToList();

            List<string> commands = new List<string>
            {
                "south",
                "east",
                "take space heater",
                "west",
                "west",
                "take shell",
                "east",
                "north",
                "west",
                "north",
                "take jam",
                "east",
                "south",
                "take asterisk",
                "south",
                "take klein bottle",
                "east",
                "take spool of cat6",
                "west",
                "north",
                "north",
                "west",
                "north",
                "take astronaut ice cream",
                "north",
                "east",
                "south",
                "take space law space brochure",
                "north",
                "west",
                "south",
                "south",
                "south",
                "south",
                "west"
            };

            int commandcount = 0;
            while (!stop)
            {
                run(computer, inputq, outputq, ref pointer, ref pointer2);

                string command = "";
                if (commandcount < commands.Count)
                {
                    command = commands[commandcount];
                    commandcount += 1;
                }
                else
                {
                    while (true)
                    {
                        string output = runacommad (computer, inputq, outputq, ref pointer, ref pointer2, "south");
                        if (output.Contains("lighter"))
                        {
                            foreach (string item in goods)
                                runacommad(computer, inputq, outputq, ref pointer, ref pointer2, "drop " + item);
                            goods = new List<string>();
                        }
                        else if (output.Contains("heavier"))
                        {
                            string itemtoadd = "";
                            while (itemtoadd.Length == 0)
                            {
                                Random rnd = new Random();
                                int r = rnd.Next(inventory.Count);
                                if (!goods.Contains(inventory[r]))
                                    itemtoadd = inventory[r];
                            }
                            goods.Add(itemtoadd);
                            command = "take " + itemtoadd;
                            runacommad(computer, inputq, outputq, ref pointer, ref pointer2, command);
                        }
                        else
                        {
                            Console.WriteLine(output);
                            stop = true;
                            break;
                        }
                    }
                }
                foreach (char c in command)
                    inputq.Enqueue((int)c);
                inputq.Enqueue(10);
            }
        }

        private static string runacommad(Dictionary<long, long> items, Queue<long> inputs, Queue<long> results, ref long pointer, ref long relativebase, string command)
        {
            foreach (char c in command)
                inputs.Enqueue((int)c);
            inputs.Enqueue(10);
            run(items, inputs, results, ref pointer, ref relativebase);
            string output = "";
            while (results.Count > 0)
                output += ((char)results.Dequeue()).ToString();
            return output;
        }

        private static long getpointer(Dictionary<long, long> items, int paramnum, long pointer, int mode, long relativebase)
        {
            long p;
            if (mode == 0)
                p = Convert.ToInt64(items[pointer + paramnum]);
            else if (mode == 1)
                p = pointer + paramnum;
            else
                p = Convert.ToInt64(items[pointer + paramnum] + relativebase);
            if (!items.ContainsKey(p))
                items.Add(p, 0);
            return p;
        }

        private static long run(Dictionary<long, long> items, Queue<long> inputs, Queue<long> results, ref long pointer, ref long relativebase)
        {
            long param1, param2;
            int steps;
            param1 = param2 = steps = 0;
            while (true)
            {
                steps += 1;
                string opcodes = items[pointer].ToString().PadLeft(5, '0');
                int opcode = int.Parse(opcodes.Substring(3, 2));
                int mode3 = opcodes[0] - 48;
                int mode2 = opcodes[1] - 48;
                int mode1 = opcodes[2] - 48;
                if (new[] { 1, 2, 4, 5, 6, 7, 8, 9 }.Contains(opcode))
                    param1 = items[getpointer(items, 1, pointer, mode1, relativebase)];
                if (new[] { 1, 2, 5, 6, 7, 8 }.Contains(opcode))
                    param2 = items[getpointer(items, 2, pointer, mode2, relativebase)];
                switch (opcode)
                {
                    case 99:
                        return int.MinValue;
                    case 1:
                        items[getpointer(items, 3, pointer, mode3, relativebase)] = param1 + param2;
                        break;
                    case 2:
                        items[getpointer(items, 3, pointer, mode3, relativebase)] = param1 * param2;
                        break;
                    case 3:
                        if (inputs.Count == 0)
                            return (int.MinValue + 1);
                        else
                            items[getpointer(items, 1, pointer, mode1, relativebase)] = Convert.ToInt64(inputs.Dequeue());
                        break;
                    case 4:
                        results.Enqueue(param1);
                        pointer += 2;
                        break;
                    case 5:
                        if (param1 != 0)
                            pointer = Convert.ToInt32(param2);
                        else
                            pointer += 3;
                        break;
                    case 6:
                        if (param1 == 0)
                            pointer = Convert.ToInt32(param2);
                        else
                            pointer += 3;
                        break;
                    case 7:
                        if (param1 < param2)
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 1;
                        else
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 0;
                        break;
                    case 8:
                        if (param1 == param2)
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 1;
                        else
                            items[getpointer(items, 3, pointer, mode3, relativebase)] = 0;
                        break;
                    case 9:
                        relativebase += Convert.ToInt32(param1);
                        break;
                    default:
                        break;
                }
                if (new[] { 1, 2, 7, 8 }.Contains(opcode))
                    pointer += 4;
                if (new[] { 9, 3 }.Contains(opcode))
                    pointer += 2;
            }
        }
    }
}
