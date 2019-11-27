using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Solver
    {
        private FullGrid _mainGrid;

        public Solver(FullGrid mainGrid)
        {
            _mainGrid = mainGrid;
        }

        public bool TrySolveOneCell()
        {
            ResetCells();
            for (short i=1; i<=9;i++)
            {
                if (TrySolveOneCell(i))
                    return true;
            }
            return false;
        }

        private void ResetCells()
        {
            foreach (Cell cell in _mainGrid.Cells)
            {
                cell.ResetStatus();
            }
        }

        /// <summary>
        /// Try to find one cell where to add the number and add it
        /// </summary>
        /// <param name="number">The number to add</param>
        /// <returns>true if a cell is found</returns>
        private bool TrySolveOneCell(short number)
        {
            FlagAllCellsHavingValue();
            FlagAllCellsInSubGridsHavingNumber(_mainGrid.Rows, number);
            FlagAllCellsInSubGridsHavingNumber(_mainGrid.Columns, number);
            FlagAllCellsInSubGridsHavingNumber(_mainGrid.SubGrids3x3, number);

            foreach (Cell cell in _mainGrid.Cells)
            {
                if (cell.Value == 0 && cell.SetToOnlyPossibleValue())
                    return true;
            }

            if (SetToOnlyPossibleCell(_mainGrid.Rows, number))
                return true;
            if (SetToOnlyPossibleCell(_mainGrid.Columns, number))
                return true;
            if (SetToOnlyPossibleCell(_mainGrid.SubGrids3x3, number))
                return true;
            return false;
        }

        private void FlagAllCellsHavingValue()
        {
            foreach (Cell cell in _mainGrid.Cells)
            {
                if (cell.Value > 0)
                {
                    cell.SetOnlyPossibleValue(cell.Value);
                }
            }
        }

        private void FlagAllCellsInSubGridsHavingNumber(List<SubGrid> subGrids, short number)
        {
            foreach (SubGrid subGrid in subGrids)
            {
                if (subGrid.HasNumber(number))
                {
                    subGrid.FlagCellsThatCantHaveNumber(number);
                }
            }
        }

        private bool SetToOnlyPossibleCell(List<SubGrid> subGrids, short number)
        {
            foreach (SubGrid subGrid in subGrids)
            {
                if (!subGrid.HasNumber(number))
                {
                    if (subGrid.SetToOnlyPossibleCell(number))
                        return true;
                }
            }
            return false;
        }

        public bool TrySolveGrid()
        {
            while (TrySolveOneCell())
            {
                if (_mainGrid.Cells.All((cell) => cell.Value > 0))
                    return true;
            }
            return false;
        }
    }
}
