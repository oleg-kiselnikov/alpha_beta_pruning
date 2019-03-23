using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(new ConsolePlayer(), 3);

            game.Response += (sender, gameState) =>
            {
                switch (gameState)
                {
                    case GameResult.GameStarted:
                        Console.WriteLine(game.CurrentState.ToString());
                        break;
                    case GameResult.PlayerWon:
                        Console.WriteLine("\r\nYou won!");
                        break;

                    case GameResult.ComputerWon:
                        Console.WriteLine("\r\nGame over!");
                        break;

                    case GameResult.DeadHeat:
                        Console.WriteLine("\r\nDead heat!");
                        break;

                    case GameResult.Error:
                        Console.WriteLine("\r\nError: {0}", game.ErrorMessage);
                        break;

                    case GameResult.Thinking:
                        Console.WriteLine("\r\nThinking...");
                        break;
                    case GameResult.StateChanged:
                        Console.WriteLine(game.CurrentState.ToString());
                        break;
                }
            };            

            game.Start();
        }
    }
}
