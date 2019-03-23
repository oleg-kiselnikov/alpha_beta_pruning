using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Minimax
    {
        public State CurrentState;

        public State ResultState;

        public int MaxDepth;

        public bool MinimizeLoss;

        public Minimax(State currentState, bool minimizeLoss = true, int maxDepth = 3)
        {
            CurrentState = currentState;

            MaxDepth = maxDepth;

            MinimizeLoss = minimizeLoss;
        }
        public void Execute()
        {
            int depth = MaxDepth; // 1;

            if (MinimizeLoss)
            {
                int maxValue = int.MinValue;

                foreach (var nextState in CurrentState.Successors)
                {
                    //iterative deepening
                    //for (; depth < MaxDepth; depth += 2)
                    {
                        int x = MinValue(nextState, depth);

                        if (x > maxValue)
                        {
                            maxValue = x;

                            ResultState = nextState;
                        }
                    }                    
                }
            } 
            else
            {
                int minValue = int.MaxValue;

                foreach (var nextState in CurrentState.Successors)
                {
                    //iterative deepening
                    //for (; depth < MaxDepth; depth += 2)
                    {
                        int x = MaxValue(nextState, depth);

                        if (x < minValue)
                        {
                            minValue = x;

                            ResultState = nextState;
                        }
                    }
                }
            }            
        }
        private int MaxValue(State state, int depth)
        {
            if (depth == 0 || state.IsEnd)
                return state.Value;

            int bestValue = int.MinValue;
            
            foreach (var nextState in state.Successors)
            {
                var x = MinValue(nextState, depth - 1);

                if (bestValue < x)
                    bestValue = x;
            }

            return bestValue;
        }

        private int MinValue(State state, int depth)
        {
            if (depth == 0)
                return state.Value;

            int bestValue = int.MaxValue;
            
            foreach (var nextState in state.Successors)
            {
                var x = MaxValue(nextState, depth - 1);

                if (x < bestValue)
                    bestValue = x;
            }

            return bestValue;
        }
    }
}
