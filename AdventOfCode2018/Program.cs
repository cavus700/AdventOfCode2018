using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Which day do you want to execute?");
            int day;
            while (!int.TryParse(Console.ReadLine(), out day) || (day < 1 || day > 25)) {
                Console.WriteLine("Please choose a day from 1-25...");
            }

            Console.WriteLine();

            AoCDay aocDay = GetAoCDay(day);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            aocDay.startA();
            watch.Stop();
            Console.WriteLine($"Day{day}.1 took {watch.ElapsedMilliseconds} msecs to execute");

            Console.WriteLine();

            watch = System.Diagnostics.Stopwatch.StartNew();
            aocDay.startB();
            watch.Stop();
            Console.WriteLine($"Day{day}.2 took {watch.ElapsedMilliseconds} msecs to execute");

            Console.ReadKey();
        }

        private static AoCDay GetAoCDay(int day)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var type = assembly.GetTypes()
                .First(t => t.Name == $"Day{day}");

            return (AoCDay)Activator.CreateInstance(type);
        }
    }
}
