using AdventOfCode2018;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Day6 : AoCDay
    {
        private List<int>[,] grid;
        private State[,] gridB;

        private enum State { InDist, OutOfDist, None, Coordinate}

        public override void startA()
        {
            var coordinates = readInput<Tuple<int, int>[]>();

            int maxX = coordinates.Max(c => c.Item2)+1;
            int maxY = coordinates.Max(c => c.Item1)+1;

            grid = new List<int>[maxY, maxX];

            initializeGridA(coordinates.Length);

            for (int iY = 0; iY < grid.GetLength(0); ++iY)
            {
                for (int iX = 0; iX < grid.GetLength(1); ++iX)
                {
                    //Compute manhattan to each coordinate
                    grid[iY, iX] = grid[iY, iX].Select(c => ComputeDist(new Tuple<int, int>(iY, iX), coordinates[c])).ToList();
                    int minDist = grid[iY, iX].Min();
                    //Determine all coordinates with shortest manhatten
                    grid[iY, iX] = grid[iY, iX].Select((c,i) => c == minDist ? i : -1).Where(c => c > -1).ToList();
                    //If more than one coordinate left clear them all
                    if (grid[iY, iX].Count > 1)
                        grid[iY, iX].Clear();
                }
            }

            //This is the final grid where we have to count the area and eliminate infinite areas
            int[,] extendedGrid = new int[maxY, maxX];
            for (int iY = 0; iY < grid.GetLength(0); ++iY)
            {
                for (int iX = 0; iX < grid.GetLength(1); ++iX)
                {
                    if (grid[iY, iX].Count == 1)
                        extendedGrid[iY, iX] = grid[iY, iX][0];
                    else
                        extendedGrid[iY, iX] = -1;
                }
            }

            //Print(ref extendedGrid);
            
            //Look at the corners to determine infinite areas
            List<int> infiniteCoord = new List<int>();
            for (int iY = 0; iY < extendedGrid.GetLength(0); ++iY)
            {
                infiniteCoord.Add(extendedGrid[iY, 0]);
                infiniteCoord.Add(extendedGrid[iY, extendedGrid.GetLength(1) - 1]);
            }
            for (int iX = 0; iX < extendedGrid.GetLength(1); ++iX)
            {
                infiniteCoord.Add(extendedGrid[0, iX]);
                infiniteCoord.Add(extendedGrid[extendedGrid.GetLength(0) - 1, iX]);
            }
            infiniteCoord = infiniteCoord.Distinct().ToList();

            int result = coordinates.Where((c, i) => !infiniteCoord.Contains(i))
                .Select(c => CountArea(ref extendedGrid, coordinates.ToList().IndexOf(c)))
                .Max();

            Console.WriteLine($"Solution for Day6.1 is {result}");
        }

        private void initializeGridA(int numCoordinates)
        {
            for (int iY = 0; iY < grid.GetLength(0); ++iY)
            {
                for (int iX = 0; iX < grid.GetLength(1); ++iX)
                {
                    grid[iY, iX] = new List<int>();
                    for (int iCor = 0; iCor < numCoordinates; ++iCor)
                    {
                        grid[iY, iX].Add(iCor);
                    }
                }
            }
        }
                
        private int ComputeDist(Tuple<int, int> cord1, Tuple<int, int> cord2)
        {
            return Math.Abs(cord1.Item1 - cord2.Item1) + Math.Abs(cord1.Item2 - cord2.Item2);
        }

        public static int CountArea(ref int[,] grid, int area)
        {
            int result = 0;
            for (int iY = 0; iY < grid.GetLength(0); ++iY)
            {
                for (int iX = 0; iX < grid.GetLength(1); ++iX)
                {
                    if (grid[iY, iX] == area)
                        ++result;            
                }
            }

            return result;
        }

        private void Print(ref int[,] grid)
        {
            for (int iX = 0; iX < grid.GetLength(1); ++iX)
            {
                for (int iY = 0; iY < grid.GetLength(0); ++iY)
                {
                    Console.Write(grid[iY, iX] + " ");
                }
                Console.WriteLine();
            }
        }

        //You have to run this in Release mode. In Debug you will get an Stackoverflow Exception due to the recursion
        public override void startB()
        {
            var coordinates = readInput<Tuple<int, int>[]>();

            int maxX = coordinates.Max(c => c.Item1) + 1;
            int maxY = coordinates.Max(c => c.Item2) + 1;

            gridB = new State[maxX, maxY];

            initializeGridB(ref coordinates);

            for (int iY = 0; iY < gridB.GetLength(1); ++iY)
            {
                for (int iX = 0; iX < gridB.GetLength(0); ++iX)
                {
                    if (gridB[iX, iY] == State.Coordinate)
                        continue;

                    //Compute manhattan to each coordinate
                    gridB[iX, iY] = coordinates.Select(c => ComputeDist(new Tuple<int, int>(iX, iY), c)).Sum() < 10000 ? State.InDist : State.OutOfDist;                                   
                }
            }

            //PrintB();

            List<int> results = new List<int>();

            for (int iY = 0; iY < gridB.GetLength(1); ++iY)
            {
                for (int iX = 0; iX < gridB.GetLength(0); ++iX)
                {
                    results.Add(CountAreaB(iX, iY));
                }
            }

            Console.WriteLine($"Solution for Day6.2 is {results.Max()}");
        }

        private void initializeGridB(ref Tuple<int, int>[] coordinates)
        {
            for (int iY = 0; iY < gridB.GetLength(1); ++iY)
            {
                for (int iX = 0; iX < gridB.GetLength(0); ++iX)
                {
                    gridB[iX, iY] = State.None;      
                }
            }
            foreach (var cor in coordinates)
            {
                gridB[cor.Item1, cor.Item2] = State.Coordinate;
            }
        }

        private int CountAreaB(int x, int y)
        {
            if (x < 0 || x >= gridB.GetLength(0))
                return 0;
            if (y < 0 || y >= gridB.GetLength(1))
                return 0;
            if (gridB[x, y] == State.OutOfDist || gridB[x, y] == State.None)
                return 0;

            gridB[x, y] = State.None;
            return 1 + CountAreaB(x, y + 1) + CountAreaB(x, y - 1) + CountAreaB(x + 1, y) + CountAreaB(x - 1, y);
        }

        private void PrintB()
        {
            for (int iY = 0; iY < gridB.GetLength(1); ++iY)
            {
                for (int iX = 0; iX < gridB.GetLength(0); ++iX)
                {
                    if (gridB[iX, iY] == State.OutOfDist)
                        Console.Write(". ");
                    if (gridB[iX, iY] == State.InDist)
                        Console.Write("# ");
                    if (gridB[iX, iY] == State.Coordinate)
                        Console.Write("C ");
                }
                Console.WriteLine();
            }
        }
        protected override T readInput<T>()
        {
            var input = File.ReadAllLines("../../Input/Day6.txt")
                .Select(s => new Tuple<int, int>(int.Parse(s.Split(',')[0]), int.Parse(s.Split(',')[1])))
                .ToArray();
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
