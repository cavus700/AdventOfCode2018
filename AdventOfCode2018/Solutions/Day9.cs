using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Day9 : AoCDay
    {
        public override void startA()
        {
            Console.WriteLine($"Solution for Day9.1 is {PlayMarbles(458, 71307)}");
        }

        public override void startB()
        {
            Console.WriteLine($"Solution for Day9.2 is {PlayMarbles(458, 71307 * 100)}");
        }

        private ulong PlayMarbles(int numPlayers, int maxMarbles)
        {
            ulong[] scores = Enumerable.Repeat(0uL, numPlayers).ToArray();
            int currPlayer = 0;
            int currMarbleIdx = 1;

            List<int> marbles = new List<int>();
            marbles.Add(0);
            marbles.Add(2);
            marbles.Add(1);

            for (int marbleCounter = 3; marbleCounter <= maxMarbles; ++marbleCounter)
            {
                if (marbleCounter % 23 == 0)
                {
                    int rmIdx = ((currMarbleIdx + marbles.Count) - 7) % marbles.Count;
                    scores[currPlayer] += Convert.ToUInt64(marbleCounter);
                    scores[currPlayer] += Convert.ToUInt64(marbles[rmIdx]);
                    marbles.RemoveAt(rmIdx);
                    currMarbleIdx = rmIdx % marbles.Count;
                }
                else
                {
                    currMarbleIdx = (currMarbleIdx + 2) % marbles.Count;
                    marbles.Insert(currMarbleIdx, marbleCounter);
                }
                currPlayer = (currPlayer + 1) % numPlayers;
            }

            return scores.Max();
        }

        protected override T readInput<T>()
        {
            var input = File.ReadAllLines("../../Input/Day9.txt");
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
