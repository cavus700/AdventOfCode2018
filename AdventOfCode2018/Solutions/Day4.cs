using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    class Day4 : AoCDay
    {
        public override void startA()
        {
            List<Guard> guardSleepRecords = readInput<List<Guard>>();           

            int guardWhichSleptMost = guardSleepRecords.GroupBy(g => g.Id)
                        .Select(g => new
                        {
                            Id = g.Key,
                            Count = g.Count()
                        })
                        .OrderByDescending(a => a.Count).First().Id;

            int minuteTheGuardSleptTheMost = guardSleepRecords.Where(g => g.Id == guardWhichSleptMost)
                .GroupBy(g => g.Date.Minute)
                .Select(g => new
                {
                    Minute = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(a => a.Count).First().Minute;

            Console.WriteLine($"Solution for Day 4.1 is {guardWhichSleptMost * minuteTheGuardSleptTheMost}");
        }

        public override void startB()
        {
            List<Guard> guardSleepRecords = readInput<List<Guard>>();

            var result = guardSleepRecords.GroupBy(g => g.Id)
                        .Select(g => new
                        {
                            Id = g.Key,
                            MostFrequentlyAsleep = g.GroupBy(g2 => g2.Date.Minute)
                                                        .Select(g2 => new
                                                        {
                                                            Minute = g2.Key,
                                                            Count = g2.Count()
                                                        })
                                                        .OrderByDescending(a => a.Count).First()
                        })
                        .OrderByDescending(a => a.MostFrequentlyAsleep.Count).First();
            

            Console.WriteLine($"Solution for Day 4.2 is {result.Id * result.MostFrequentlyAsleep.Minute}");
        }

        protected override T readInput<T>()
        {

            Regex pattern = new Regex(@"(?<date>\d+-\d+-\d+\s\d{2}:\d{2})\]\s(?<action>[\bwakes\b|\bGuard\b|\bfalls\b].*)");
            var guards = File.ReadAllLines("../../Input/Day4.txt").Select(x => { var match = pattern.Match(x); return new Guard(match.Groups["date"].Value, match.Groups["action"].Value); }).OrderBy(g => g.Date).ToArray();

            List<Guard> guardSleepRecords = new List<Guard>();

            int idLastGuard = -1;
            DateTime fallsAsleep = DateTime.Now;

            foreach (Guard guard in guards)
            {
                if (guard.Action == Guard.Actions.start)
                {
                    idLastGuard = guard.Id;
                }
                else if (guard.Action == Guard.Actions.wakesup)
                {
                    while (fallsAsleep.CompareTo(guard.Date) != 0)
                    {
                        guardSleepRecords.Add(new Guard(idLastGuard, fallsAsleep));
                        fallsAsleep = fallsAsleep.AddMinutes(1.0);
                    }
                }
                else
                {
                    fallsAsleep = guard.Date;
                }
            }

            return (T)Convert.ChangeType(guardSleepRecords, typeof(T));
        }

        private class Guard
        {
            public enum Actions { start, sleep, wakesup}
            public Guard(int id, DateTime date)
            {
                Id = id;
                Date = date;
            }

            public Guard(string date, string action)
            {
                Date = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                if (action.Equals("wakes up"))
                    Action = Actions.wakesup;
                else if (action.Equals("falls asleep"))
                    Action = Actions.sleep;
                else
                {
                    Regex pattern = new Regex(@"\d+");

                    var m = pattern.Match(action);
                    Id = int.Parse(m.Value);
                    Action = Actions.start;
                }
            }
            public int Id;
            public DateTime Date; //DateTime.ParseExact(input, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            public Actions Action;

            

        }
    }
}
