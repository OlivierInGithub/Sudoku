using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class FullGrid
    {
        public List<Cell> Cells { get; set; }
        public List<SubGrid> SubGrids3x3 { get; set; }
        public List<SubGrid> Rows;
        public List<SubGrid> Columns;

        public FullGrid()
        {
            Cells = new List<Cell>();
            Rows = new List<SubGrid>(9);
            Columns = new List<SubGrid>(9);
            SubGrids3x3 = new List<SubGrid>();
            Enumerable.Range(1, 9).ToList().ForEach((i)=> Rows.Add(new SubGrid()));
            Enumerable.Range(1, 9).ToList().ForEach((i) => Columns.Add(new SubGrid()));
            Enumerable.Range(1, 9).ToList().ForEach((i) => SubGrids3x3.Add(new SubGrid()));
            
            foreach (SubGrid row in Rows)
            {
                foreach (SubGrid col in Columns)
                {
                    var cell = new Cell();
                    Cells.Add(cell);
                    row.Cells.Add(cell);
                    col.Cells.Add(cell);
                }
            }

            for (int rowId = 0; rowId < 9; rowId++)
            {
                for (int colId = 0; colId < 9; colId++)
                {
                    int subGridId = 3*(rowId % 3) + (colId % 3);
                    SubGrids3x3[subGridId].Cells.Add(Rows[rowId].Cells[colId]);
                }
            }
        }
    }
}
