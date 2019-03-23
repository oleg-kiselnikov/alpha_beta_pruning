using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public sealed class Game
    {
        public int BoardSize { get; private set; }

        public GameState CurrentState { get; private set; }

        public Player Player;

        public Game(Player player, int boardSize = 3)
        {
            Player = player;

            var board = new string[boardSize, boardSize];

            BoardSize = boardSize;

            CurrentState = new GameState(board);
        }


        public void Start()
        {
            InvokeResponseEvent(GameResult.GameStarted);

            int stepsCounter = 1;

            int maxDepth = BoardSize * BoardSize;
            
            while (!CurrentState.IsEnd && CurrentState.Successors.Any())
            {
                if (stepsCounter % 2 != 0)
                {
                    do
                    {
                        var position = Player.Ask();

                        CurrentState = (GameState)CurrentState.Clone();

                        ErrorMessage = CurrentState.SetSign(position);

                        if (ErrorMessage != null)
                            InvokeResponseEvent(GameResult.Error);

                    } while (ErrorMessage != null);               
                }
                else
                {
                    InvokeResponseEvent(GameResult.Thinking);

                    //Task.Delay(TimeSpan.FromSeconds(0.75)).Wait();

                    var algorythm =
                        //new Minimax(CurrentState, minimizeLoss: true, depth:maxDepth);
                        new AlphaBetaPruning(CurrentState, minimizeLoss: true, depth: maxDepth);

                    algorythm.Execute();

                    CurrentState = (GameState)algorythm.ResultState;
                }

                stepsCounter++;

                maxDepth--;

                InvokeResponseEvent(GameResult.StateChanged);
            }

            if (!CurrentState.IsEnd)
                InvokeResponseEvent(GameResult.DeadHeat);
            else if (stepsCounter % 2 != 0)
                InvokeResponseEvent(GameResult.ComputerWon);
            else
                InvokeResponseEvent(GameResult.PlayerWon);

            Task.Delay(TimeSpan.FromSeconds(2)).Wait();
        }

        public string ErrorMessage { get; set; }

        public event EventHandler<GameResult> Response;

        public void InvokeResponseEvent(GameResult result)
        {
            var handler = Response;

            if (handler != null)
            {
                handler(this, result);
            }
        }
    }
}
