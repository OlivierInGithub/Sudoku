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

        public void HideCellsAccordingToNumber(short number)
        {
            if (Cells.Any((cell) => cell.Value == number))
            {
                foreach (CellVM cell in Cells)
                {
                    if (cell.Value != number)
                        cell.ToHide = true;
                }
            }
            foreach (CellVM cell in Cells)
            {
                if (cell.Value != number && cell.Value > 0)
                    cell.ToHide = true;
            }
        }
    }

    public class SubCellsVM
    {
        public List<CellVM> Cells;
        public SubCellsVM()
        {
            Cells = new List<CellVM>();
        }

        public void HideCellsAccordingToNumber(short number)
        {
            if (Cells.Any((cell) => cell.Value == number))
            {
                foreach (CellVM cell in Cells)
                {
                    if (cell.Value != number)
                        cell.ToHide = true;
                }
            }
            foreach (CellVM cell in Cells)
            {
                if (cell.Value != number && cell.Value > 0)
                    cell.ToHide = true;
            }
        }
    }
}
