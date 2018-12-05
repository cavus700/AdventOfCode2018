using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Day5 : AoCDay
    {        
        public override void startA()
        {
            string input = readInput<string>();
            input = reducePolymer(input);

            Console.WriteLine($"Solution for Day 5.1 is {input.Length}");
        }

        // Solution has to be < 10708
        public override void startB()
        {
            string input = readInput<string>();
            List<int> solutions = new List<int>();
            for(char iChar = 'a'; iChar <= 'z'; ++iChar)
            {
                string newPolymer = String.Concat(input.Where(c => c != iChar && c != char.ToUpper(iChar)));
                solutions.Add(reducePolymer(newPolymer).Length);
            }

            Console.WriteLine($"Solution for Day 5.2 is {solutions.Min()}");
        }

        public string  reducePolymer(string polymer)
        {
            Regex regex = new Regex(@"(?<char>.)\k<char>", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(polymer);
            bool removedPolymers = true;
            while (matches != null && removedPolymers)
            {
                removedPolymers = false;

                foreach (Match match in matches)
                {
                    string possibleReaction = match.Value;
                    if ((char.IsLower(possibleReaction[0]) && char.IsUpper(possibleReaction[1])) ||
                        (char.IsLower(possibleReaction[1]) && char.IsUpper(possibleReaction[0])))
                    {
                        polymer = polymer.Replace(possibleReaction, "");
                        removedPolymers = true;
                    }
                }
                matches = regex.Matches(polymer);
            }
            return polymer;
        }

        protected override T readInput<T>()
        {
            var input = File.ReadAllLines("../../Input/Day5.txt");
            return (T)Convert.ChangeType(input[0], typeof(T));
        }
    }
}
