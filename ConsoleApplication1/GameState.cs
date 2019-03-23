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
                var x = 0;

                for (int i = 0; i < RowCount; i++)
                {
                    x += Eval(Row(i));
                }

                for (int i = 0; i < ColCount; i++)
                {
                    x += Eval(Column(i));
                }

                x += Eval(Diagonal1);

                x += Eval(Diagonal2);
                
                return x; 
            }
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
