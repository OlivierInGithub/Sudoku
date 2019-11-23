using System.Collections.Generic;

namespace Sudoku
{
    public class SubGrid
    {
        public List<Cell> Cells { get; set; }

        public SubGrid()
        {
            Cells = new List<Cell>();
        }
    }
}
