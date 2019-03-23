using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class AlphaBetaPruning
    {
        public State CurrentState;

        public State ResultState;

        public int Depth;

        public bool MinimizeLoss;

        public AlphaBetaPruning(State currentState, bool minimizeLoss = true, int depth = 3)
        {
            CurrentState = currentState;

            Depth = depth;

            MinimizeLoss = minimizeLoss;
        }
        public void Execute()
        {
            if (MinimizeLoss)
            {
                int maxValue = int.MinValue;

                foreach (var nextState in CurrentState.Successors)
                {
                    int x = AlphaBeta(nextState, int.MinValue, int.MaxValue, Depth);

                    if (x > maxValue)
                    {
                        maxValue = x;

                        ResultState = nextState;
                    }
                }
            } 
            else
            {
                int minValue = int.MaxValue;

                foreach (var nextState in CurrentState.Successors)
                {
                    int x = - AlphaBeta(nextState, int.MinValue, int.MaxValue, Depth);

                    if (x < minValue)
                    {
                        minValue = x;

                        ResultState = nextState;
                    }
                }
            }            
        }
        private int AlphaBeta(State state, int a, int b, int depth)
        {
            if (depth == 0 || state.IsEnd)
                return state.Value;

            foreach (var nextState in state.Successors)
            {
                var x = - AlphaBeta(nextState, -b, -a, depth - 1);

                if (x > a)
                    a = x;

                if (b <= a)
                    return a;
            }

            return a;
        }
    }
}
