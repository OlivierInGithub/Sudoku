﻿using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class FullGrid
    {
        public List<Cell> Cells { get; set; }
        public List<SubGrid> SubGrids { get; set; }
        private List<Row> _rows;
        private List<Column> _columns;

        public FullGrid()
        {
            Cells = new List<Cell>();
            _rows = new List<Row>(9);
            _columns = new List<Column>(9);
            SubGrids = new List<SubGrid>();
            Enumerable.Range(1, 9).ToList().ForEach((i)=>_rows.Add(new Row()));
            Enumerable.Range(1, 9).ToList().ForEach((i) => _columns.Add(new Column()));
            Enumerable.Range(1, 9).ToList().ForEach((i) => SubGrids.Add(new SubGrid()));
            
            foreach (Row row in _rows)
            {
                foreach (Column col in _columns)
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
                    SubGrids[subGridId].Cells.Add(_rows[rowId].Cells[colId]);
                }
            }
        }
    }
}