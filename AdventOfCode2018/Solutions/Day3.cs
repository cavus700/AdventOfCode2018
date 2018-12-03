using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Day3 : AoCDay
    {
        public override void startA()
        {
            var rectangles = readInput<Rectangle[]>();            

            Console.WriteLine($"Solution for Day 3.1 is {ComputeTiles(ref rectangles).Where(i => i.Value > 1).Count()}");
        }

        public override void startB()
        {
            var rectangles = readInput<Rectangle[]>();
            var tiles = ComputeTiles(ref rectangles);

            bool isOverlapped = false;
            string key;

            foreach (var rec in rectangles)
            {
                for (int height = rec.Top; height < rec.Top + rec.Height; ++height)
                {
                    for (int width = rec.Left; width < rec.Left + rec.Width; ++width)
                    {
                        key = $"{height}x{width}";
                        if(tiles[key] > 1)
                        {
                            isOverlapped = true;
                            break;
                        }
                    }
                    if (isOverlapped)
                        break;
                }

                if (isOverlapped)
                    isOverlapped = false;
                else
                {
                    Console.WriteLine($"Solution for Day 3.2 is {rec.Id}");
                    return;
                }
            }
        }

        private Dictionary<string, int> ComputeTiles(ref Rectangle[] rectangles)
        {
            Dictionary<string, int> tiles = new Dictionary<string, int>();
            string key;

            foreach (var rec in rectangles)
            {
                for (int height = rec.Top; height < rec.Top + rec.Height; ++height)
                {
                    for (int width = rec.Left; width < rec.Left + rec.Width; ++width)
                    {
                        key = $"{height}x{width}";
                        if (!tiles.ContainsKey(key))
                        {
                            tiles[key] = 0;
                        }
                        ++tiles[key];
                    }
                }
            }

            return tiles;
        }

        protected override T readInput<T>()
        {
            Rectangle[] input = File.ReadAllLines("../../Input/Day3.txt").Select(x => new Rectangle(x)).ToArray();
            return (T)Convert.ChangeType(input, typeof(T));
        }

        private class Rectangle
        {
            public int Id;
            public int Left;
            public int Top;
            public int Width;
            public int Height;

            //Example #25 @ 121,842: 15x22
            public Rectangle(string s) {
                var parts = s.Split(' ');
                Id = int.Parse(parts[0].Substring(1));

                var coordinates = parts[2].Split(',');
                Left = int.Parse(coordinates[0]);
                Top = int.Parse(coordinates[1].Substring(0, coordinates[1].Length-1));

                var dimension = parts[3].Split('x');
                Width = int.Parse(dimension[0]);
                Height = int.Parse(dimension[1]);

            }
        }
    }
}
