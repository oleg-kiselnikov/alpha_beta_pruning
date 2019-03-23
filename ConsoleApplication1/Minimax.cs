using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Minimax
    {
        public int VisitedStatesCounter;
        
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
            VisitedStatesCounter = 0;

            int depth = MaxDepth; // 1;

            if (MinimizeLoss)
            {
                int minValue = int.MaxValue;

                foreach (var nextState in CurrentState.Successors)
                {
                    int x = MaxValue(nextState, depth);

                    if (x < minValue)
                    {
                        minValue = x;

                        ResultState = nextState;
                    }
                }
            } 
            else
            {
                int maxValue = int.MinValue + 1;

                foreach (var nextState in CurrentState.Successors)
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
        private int MaxValue(State state, int depth)
        {
            VisitedStatesCounter++;

            if (depth == 0 || state.IsEnd || !state.Successors.Any())
                return state.Value;

            int bestValue = int.MinValue + 1;
            
            foreach (var nextState in state.Successors)
            {
                var v = MinValue(nextState, depth - 1);

                if (v > bestValue)
                    bestValue = v;
            }

            return bestValue;
        }

        private int MinValue(State state, int depth)
        {
            VisitedStatesCounter++;

            if (depth == 0 || state.IsEnd || !state.Successors.Any())
                return state.Value;

            int bestValue = int.MaxValue;
            
            foreach (var nextState in state.Successors)
            {
                var v = MaxValue(nextState, depth - 1);

                if (v < bestValue)
                    bestValue = v;
            }

            return bestValue;
        }
    }
}
