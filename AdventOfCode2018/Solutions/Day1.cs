using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    public class Day1 : AoCDay
    {
        public override void startA()
        {
            int[] input = readInput<int[]>();
            Console.WriteLine($"Solution for Day1.1 is {input.Sum()}");
        }

        public override void startB()
        {
            int[] input = readInput<int[]>();
            int currentFrquency = 0;
            Dictionary<int, bool> frequencies = new Dictionary<int, bool>();
            frequencies[0] = true;

            bool found = false;
            while (!found)
            {
                foreach(int value in input){
                    currentFrquency += value;

                    if (frequencies.ContainsKey(currentFrquency))
                    {
                        found = true;
                        break;
                    }
                    else {
                        frequencies[currentFrquency] = true;
                    }
                }
            }

            Console.WriteLine($"Solution for Day1.2 is {currentFrquency}");
        }

        protected override T readInput<T>()
        {
            int[] input = File.ReadAllLines("../../Input/Day1.txt").Select(int.Parse).ToArray();
            return (T)Convert.ChangeType(input, typeof(T));
        }
        
    }
}
