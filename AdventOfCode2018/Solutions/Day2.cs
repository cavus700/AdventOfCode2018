using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.Solutions
{
    public class Day2 : AoCDay
    {
        public override void startA()
        {
            string[] input = readInput<string[]>();
            //string[] input = { "abcdef", "bababc", "abcccd", "abbcde", "aabcdd", "abcdee", "ababab" };
            var sortedStrings = input.Select(x => String.Concat(x.OrderBy(c => c)));
            int doubles = sortedStrings.Where(s => ContainsChars(s, 2)).Count();
            int tripples = sortedStrings.Where(s => ContainsChars(s, 3)).Count();
            
            Console.WriteLine($"Solution for Day2.1 is {doubles * tripples}");
        }

        private bool ContainsChars(string s, int num)
        {
            char c = s[0];
            int count = 1;

            for (int i = 1; i < s.Length; ++i) {
                if(s[i] == c)
                {
                    ++count;
                }else
                {
                    if (count == num)
                        return true;

                    c = s[i];
                    count = 1;
                }
            }
            return count == num;
        }

        public override void startB()
        {
            string[] input = readInput<string[]>();
            int index;

            foreach(string s1 in input)
            {
                foreach (string s2 in input) {
                    if(isMatching(s1,s2,out index))
                    {
                        Console.WriteLine($"Solution for Day2.2 is {s1.Remove(index, 1)}");
                        return;
                    }
                }
            }
        }

        private bool isMatching(string s1, string s2, out int index)
        {
            bool result = false;
            index = -1;

            if (s1.Length != s2.Length)
                return result;

            for(int i = 0; i < s1.Length; ++i)
            {
                if(s1[i] != s2[i])
                {
                    if (index != -1)
                        return false;
                    index = i;
                    result = true;
                }
            }
            return result;
        }

        protected override T readInput<T>()
        {
            string[] input = File.ReadAllLines("../../Input/Day2.txt").ToArray();
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
