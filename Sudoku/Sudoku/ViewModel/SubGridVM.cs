using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Sudoku
{
    public class SubGridVM
    {
        private SubGrid _subGrid;
        public BindingList<CellVM> Cells { get; set; }

        public SubGridVM(SubGrid subGrid, SelectedNumber selectedNumber)
        {
            _subGrid = subGrid;
            Cells = new BindingList<CellVM>();
            foreach (Cell cell in _subGrid.Cells)
            {
                Cells.Add(new CellVM(cell, selectedNumber));
            }
        }

        public void Refresh()
        {
            foreach (CellVM cell in Cells)
            {
                cell.Refresh();
            }
        }
    }
}
