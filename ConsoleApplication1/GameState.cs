using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public sealed class GameState : State, ICloneable
    {
        private string[,] _board;
        public string[,] Board
        {
            get { return _board; }
            private set
            {
                _board = value;

                RowCount = _board.GetLength(0);
                ColCount = _board.GetLength(1);

                Size = RowCount > ColCount ? ColCount : RowCount;
            }
        }

        public int RowCount { get; private set; }

        public int ColCount { get; private set; }

        public int Size { get; private set; }

        public string _sign;
        public string Sign
        {
            get
            {
                return _sign;
            }
            private set
            {
                _sign = value;

                NextSign = _sign == "x" ? "o" : "x";
            }
        }

        public string NextSign { get; private set; }

        private GameState() 
        {
            Sign = "x";
        }
        public GameState(string[,] board)
            :this()
        {
            Board = board;
        }

        public string Message { get; set; }

        public string SetSign(Position position)
        {
            if (position.Row < 0 || position.Col < 0)
                return "A row index and a column index must be positive numbers";

            if (position.Row >= RowCount || position.Col >= ColCount)
                return "Specified row index or column index are out of range";

            if (!string.IsNullOrWhiteSpace(Board[position.Row, position.Col]))
                return "There is another sign in specified position";

            Sign = NextSign;

            Board[position.Row, position.Col] = Sign;

            return null;
        }

        public object Clone()
        {
            var state = new GameState();

            state.Sign = Sign;

            state.Board = (string[,])this.Board.Clone();

            return state;
        }

        public override int Value
        {
            get
            {
                //return Heuristic1();
                return Heuristic2();
                //return Heuristic3();
            }
        }

        /// <summary>
        /// Heuristic 1
        /// </summary>
        /// <returns>
        ///  1 - Winning  
        /// -1 - Losing
        ///  0 - Other game states
        /// </returns>
        private int Heuristic1()
        {
            int v;

            for (int i = 0; i < RowCount; i++)
            {
                v = Eval(Row(i));

                if (v == RowCount)
                    return 1;

                if (-v == RowCount)
                    return -1;
            }

            for (int i = 0; i < ColCount; i++)
            {
                v = Eval(Column(i));

                if (v == ColCount)
                    return 1;

                if (-v == ColCount)
                    return -1;
            }

            v = Eval(Diagonal1);

            if (v == Size)
                return 1;

            if (-v == Size)
                return -1;

            v = Eval(Diagonal2);

            if (v == Size)
                return 1;

            if (-v == Size)
                return -1;

            return 0; 
        }

        /// <summary>
        /// Heuristic 2
        /// </summary>
        /// <returns>
        ///     Difference of numbers of potential lines for 
        /// for a player and an opponent:
        /// 
        ///  [Current player's lines] - [Opponent's lines]
        /// </returns>
        private int Heuristic2()
        {
            int v;

            int result = 0;

            for (int i = 0; i < RowCount; i++)
            {
                v = Eval(Row(i));

                if (v > 1)
                    result++;

                if (-v > 1)
                    result--;
            }

            for (int i = 0; i < ColCount; i++)
            {
                v = Eval(Column(i));

                if (v > 1)
                    result++;

                if (-v > 1)
                    result--;
            }

            v = Eval(Diagonal1);

            if (v > 1)
                result++;

            if (-v > 1)
                result--;

            v = Eval(Diagonal2);

            if (v > 1)
                result++;

            if (-v > 1)
                result--;

            return result; 
        }

        /// <summary>
        /// Heuristic 3
        /// </summary>
        /// <returns>
        ///     Difference of numbers of signs in potential 
        ///  lines for a player and an opponent:
        /// 
        ///  [Current player's signs] - [Opponent's signs]
        /// </returns>
        private int Heuristic3()
        {
            int v;
            var x = 0;

            for (int i = 0; i < RowCount; i++)
            {
                v = Eval(Row(i));

                x += v;
            }

            for (int i = 0; i < ColCount; i++)
            {
                v = Eval(Column(i));

                x += v;
            }

            v = Eval(Diagonal1);

            x += v;

            v = Eval(Diagonal2);

            x += v;

            return x; 
        }

        public bool? _isEnd;
        public override bool IsEnd 
        {
            get
            {
                if (_isEnd.HasValue)
                    return _isEnd.Value;

                for (int i = 0; i < RowCount && !_isEnd.HasValue; i++)
                {
                    int signCount = Math.Abs(Eval(Row(i)));

                    if (signCount == RowCount)
                        _isEnd = true;
                }

                for (int j = 0; j < ColCount && !_isEnd.HasValue; j++)
                {
                    int signCount = Math.Abs(Eval(Column(j)));

                    if (signCount == ColCount)
                        _isEnd = true;
                }

                if (!_isEnd.HasValue)
                {
                    int signCount = Math.Abs(Eval(Diagonal1));

                    if (signCount == Size)
                        _isEnd = true;
                }

                if (!_isEnd.HasValue)
                {
                    int signCount = Math.Abs(Eval(Diagonal2));

                    if (signCount == Size)
                        _isEnd = true;
                }

                return _isEnd.HasValue ? _isEnd.Value : false;
            }
        }

        public override IEnumerable<State> Successors
        {
            get
            {
                return GetSuccessors();
            }
        }

        /// <summary>
        /// Counts number of noughts or crosses 
        /// </summary>
        /// <param name="line"></param>
        /// <returns> 
        /// 0 - line contains noughts and crosses
        /// n - line contains only signs of current player (noughts)
        /// -n - line contains only signs of opponent player (crosses)
        /// </returns>
        private int Eval(IEnumerable<string> line)
        {
            int p = 0, n = 0;
            
            foreach (var x in line)
            {
                if (x == Sign && n == 0)
                {
                    p++;
                }
                else if (x == NextSign && p == 0)
                {
                    n++;
                }
                else if (!string.IsNullOrWhiteSpace(x))
                {
                    p = 0; n = 0;

                    break;
                }
            }

            return p - n;
        }

        private IEnumerable<string> Column(int j)
        {
            for (int i = 0; i < RowCount; i++)
                yield return Board[i, j];
        }

        private IEnumerable<string> Row(int i)
        {
            for (int j = 0; j < ColCount; j++)
                yield return Board[i, j];
        }

        private IEnumerable<string> Rows
        {
            get
            {
                for (int i = 0; i < RowCount; i++)
                    for (int j = 0; j < ColCount; j++)
                        yield return Board[i, j];
            }
        }

        private IEnumerable<string> Columns
        {
            get
            {
                for (int j = 0; j < ColCount; j++)
                    for (int i = 0; i < RowCount; i++)
                        yield return Board[i, j];
            }
        }

        private IEnumerable<string> Diagonal1
        {
            get
            {
                for (int k = 0; k < Size; k++)
                    yield return Board[k, k];
            }
        }

        private IEnumerable<string> Diagonal2
        {
            get
            {
                for (int k = 0; k < Size; k++)
                    yield return Board[k, Size - k - 1];
            }
        }

        private IEnumerable<State> GetSuccessors()
        {
            var states = new List<State>();

            if (IsEnd)
                yield break;

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    if (string.IsNullOrWhiteSpace(Board[i, j]))
                    {
                        var state = (GameState)Clone();

                        state.SetSign(new Position() { Row = i, Col = j });

                        yield return state;
                    }
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("  ");
            
            for (int j = 0; j < ColCount; j++)
                stringBuilder.AppendFormat("{0:#0} ", j);

            stringBuilder.Append("\r\n  ");

            for (int j = 0; j < ColCount; j++)
                stringBuilder.Append("__");

            stringBuilder.Append("\r\n");

            for (int i = 0; i < RowCount; i++)
            {
                stringBuilder.AppendFormat("{0:#0} ", i);

                for (int j = 0; j < ColCount; j++)
                {
                    var x = Board[i, j];

                    if (string.IsNullOrWhiteSpace(x))
                        x = "_";

                    stringBuilder.AppendFormat(j < RowCount - 1 ? "{0} " : "{0}\r\n", x);
                }
            }
                
                    

            return stringBuilder.ToString();
        }
    }
}
