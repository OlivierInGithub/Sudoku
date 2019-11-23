using System.Collections.Generic;

namespace Sudoku
{
    public class Row
    {
        public List<Cell> Cells;

        public Row()
        {
            Cells = new List<Cell>();
        }
    }
}
