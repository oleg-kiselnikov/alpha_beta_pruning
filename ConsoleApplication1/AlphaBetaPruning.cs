namespace ConsoleApplication1
{
    class AlphaBetaPruning
    {
        public int VisitedStatesCounter;

        public State CurrentState;

        public State ResultState;

        public int MaxDepth;

        public bool MinimizeLoss;

        public AlphaBetaPruning(State currentState, bool minimizeLoss = true, int maxDepth = 8)
        {
            CurrentState = currentState;

            MaxDepth = maxDepth;

            MinimizeLoss = minimizeLoss;
        }
        public void Execute()
        {
            VisitedStatesCounter = 0;

            if (MinimizeLoss)
            {
                int maxValue = int.MinValue + 1;

                foreach (var nextState in CurrentState.Successors)
                {
                    int x = AlphaBeta(nextState, int.MinValue + 1, int.MaxValue, MaxDepth);

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
                    int x = -AlphaBeta(nextState, int.MaxValue, int.MinValue + 1, MaxDepth);

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
            VisitedStatesCounter++;

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
