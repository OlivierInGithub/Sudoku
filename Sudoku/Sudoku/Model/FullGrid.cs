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

            for (int gridRowId = 0; gridRowId < 3; gridRowId++)
            {
                for (int gridColId = 0; gridColId < 3; gridColId++)
                {
                    SubGrids3x3[gridRowId * 3 + gridColId].Cells.AddRange(Rows[gridRowId * 3].Cells.Skip(gridColId * 3).Take(3));
                    SubGrids3x3[gridRowId * 3 + gridColId].Cells.AddRange(Rows[gridRowId * 3 + 1].Cells.Skip(gridColId * 3).Take(3));
                    SubGrids3x3[gridRowId * 3 + gridColId].Cells.AddRange(Rows[gridRowId * 3 + 2].Cells.Skip(gridColId * 3).Take(3));
                }
            }
        }

        public bool IsComplete()
        {
            return Cells.All((cell) => cell.Value > 0);
        }

        public bool IsValid()
        {
            return AreValid(Rows) && 
                AreValid(Columns) && 
                AreValid(SubGrids3x3); 
        }

        private bool AreValid(List<SubGrid> subGrids)
        {
            return subGrids.All((sg) => sg.IsValid());
        }
    }
}
