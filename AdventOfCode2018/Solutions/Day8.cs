using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Day8 : AoCDay
    {
        public override void startA()
        {
            var input = readInput<List<int>>();
            Console.WriteLine($"Solution for Day8.1 is {SumMetadata(ref input, 0)}");
        }

        private int SumMetadata(ref List<int> input, int iter)
        {
            int numChilds = input[iter];
            int result = 0;
            for(int iChild = 0; iChild < numChilds; ++iChild)
            {
                result += SumMetadata(ref input, iter + 2);
            }

            for(int iMeta = 0; iMeta < input[iter+1]; ++iMeta)
            {
                result += input[iMeta + iter + 2];
            }
            input.RemoveRange(iter, input[iter + 1] + 2);
            return result;
        }

        public override void startB()
        {
            var input = readInput<List<int>>();
            Tree tree = BuildMetadataTree(ref input, 0);

            Console.WriteLine($"Solution for Day8.2 is {GetValue(ref tree)}");
        }

        public int GetValue(ref Tree tree)
        {
            int add = 0;
            if (tree.Children.Count == 0)
            {
                add = tree.Metadata.Sum();
                return add;
            }

            foreach(var meta in tree.Metadata)
            {
                if(meta-1 >= 0 && meta-1 < tree.Children.Count)
                {
                    Tree child = tree.Children[meta - 1];
                    add += GetValue(ref child);
                }
            }
            return add;            
        }

        private Tree BuildMetadataTree(ref List<int> input, int iter)
        {
            Tree currTree = new Tree();
            currTree.MetadataEntries = input[iter + 1];

            int numChilds = input[iter];
            for (int iChild = 0; iChild < numChilds; ++iChild)
            {
                currTree.Children.Add(BuildMetadataTree(ref input, iter + 2));
            }

            for (int iMeta = 0; iMeta < input[iter + 1]; ++iMeta)
            {
                currTree.Metadata.Add(input[iMeta + iter + 2]);
            }

            input.RemoveRange(iter, input[iter + 1] + 2);
            return currTree;
        }        

        internal class Tree
        {
            public int MetadataEntries;
            public List<int> Metadata;
            public List<Tree> Children;

            public Tree()
            {
                Metadata = new List<int>();
                Children = new List<Tree>();
            }
        }

        protected override T readInput<T>()
        {
            var input = File.ReadAllLines("../../Input/Day8.txt")[0].Split(' ').Select(int.Parse).ToList();
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
