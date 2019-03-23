using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class ConsolePlayer : Player
    {
        public override Position Ask()
        {
            Console.WriteLine("\r\nYour step! ");
            
            int row;

            do { Console.Write("Row    : "); } while (!int.TryParse(Console.ReadLine(), out row));

            int col;

            do { Console.Write("Column : "); } while (!int.TryParse(Console.ReadLine(), out col));

            return new Position() { Row = row, Col = col };            
        }
    }
}
