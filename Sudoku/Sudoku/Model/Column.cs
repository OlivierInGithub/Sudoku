using System.Collections.Generic;

namespace Sudoku
{
    public class Column
    {
        public List<Cell> Cells;

        public Column()
        {
            Cells = new List<Cell>();
        }
    }
}
