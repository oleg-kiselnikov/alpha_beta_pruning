using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public abstract class State
    {
        public abstract int Value { get; }
        public abstract bool IsEnd
        {
            get;
        }

        public abstract IEnumerable<State> Successors { get; }
    }
}
