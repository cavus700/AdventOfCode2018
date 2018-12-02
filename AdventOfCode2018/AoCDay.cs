using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018
{
    public abstract class AoCDay
    {
        public abstract void startA();
        public abstract void startB();
        protected abstract T readInput<T>();
    }
}
