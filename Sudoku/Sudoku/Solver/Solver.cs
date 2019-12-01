using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Solver
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
            for (short i = 1; i <= 9; i++)
            {
                if (TryHarderSolveOneCell(i))
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
            if (!_mainGrid.IsValid())
                throw new Exception("Input grid is not valid!");

            FlagAllCellsHavingValue();
            FlagAllCellsInSubGridsHavingNumber(_mainGrid.Rows, number);
            FlagAllCellsInSubGridsHavingNumber(_mainGrid.Columns, number);
            FlagAllCellsInSubGridsHavingNumber(_mainGrid.SubGrids3x3, number);

            if (TrySetOneCell(number))
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

        private bool TrySetOneCell(short number)
        {
            foreach (Cell cell in _mainGrid.Cells)
            {
                if (cell.Value == 0 && cell.SetToOnlyPossibleValue())
                    return true;
            }

            if (SetToOnlyPossibleCell(_mainGrid.SubGrids3x3, number))
                return true;
            if (SetToOnlyPossibleCell(_mainGrid.Rows, number))
                return true;
            if (SetToOnlyPossibleCell(_mainGrid.Columns, number))
                return true;
            return false;
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

        private bool TryHarderSolveOneCell(short number)
        {
            FlagMoreCells(number);

            if (TrySetOneCell(number))
                return true;

            return false;
        }

        private List<InnerRow> _innerRows;
        private List<InnerCol> _innerCols;

        private void FlagMoreCells(short number)
        {
            FillInnerRows();
            FillInnerCols();
            for (int subGridId = 0; subGridId < 9; subGridId++)
            {
                var subGrid = _mainGrid.SubGrids3x3[subGridId];
                if (!subGrid.Cells.Any((cell) => cell.Value == number))
                {
                    TryFlagInnerRows(subGridId, number);
                    TryFlagInnerCols(subGridId, number);
                }
            }
        }

        private void FillInnerRows()
        {
            _innerRows = new List<InnerRow>();
            for (int rowId = 0; rowId < 9; rowId++)
            {
                var row = _mainGrid.Rows[rowId];
                _innerRows.Add(new InnerRow(row.Cells.Take(3), rowId / 3, rowId));
                _innerRows.Add(new InnerRow(row.Cells.Skip(3).Take(3), rowId / 3 + 1, rowId));
                _innerRows.Add(new InnerRow(row.Cells.Skip(6), rowId / 3 + 2, rowId));
            }
        }

        private void FillInnerCols()
        {
            _innerCols = new List<InnerCol>();
            for (int colId = 0; colId < 9; colId++)
            {
                var col = _mainGrid.Columns[colId];
                _innerCols.Add(new InnerCol(col.Cells.Take(3), colId / 3, colId));
                _innerCols.Add(new InnerCol(col.Cells.Skip(3).Take(3), colId / 3 + 3, colId));
                _innerCols.Add(new InnerCol(col.Cells.Skip(6), colId / 3 + 6, colId));
            }
        }

        private void TryFlagInnerRows(int subGridId, short number)
        {
            var subGridInnerRows = _innerRows.Where((innerRow) => innerRow.IsInSubGrid(subGridId));
            var innerRowsThatCanHaveValue = subGridInnerRows.Where((innerRow) => innerRow.CanHaveValue(number));
            if (innerRowsThatCanHaveValue.Count() == 1)
            {
                var innerRowWithValue = innerRowsThatCanHaveValue.First();
                var innerRowsInSameRow = _innerRows.Where((row) =>
                    row.IsInSameRow(innerRowWithValue) &&
                    !row.IsInSameSubGrid(innerRowWithValue));
                foreach (InnerRow row in innerRowsInSameRow)
                {
                    row.FlagCantHaveNumber(number);
                }
            }
        }

        private void TryFlagInnerCols(int subGridId, short number)
        {
            var subGridInnerCols = _innerCols.Where((innerCol) => innerCol.IsInSubGrid(subGridId));
            var innerColsThatCanHaveValue = subGridInnerCols.Where((innerCol) => innerCol.CanHaveValue(number));
            if (innerColsThatCanHaveValue.Count() == 1)
            {
                var innerColWithValue = innerColsThatCanHaveValue.First();
                var innerColsInSameCol = _innerCols.Where((col) =>
                    col.IsInSameCol(innerColWithValue) &&
                    !col.IsInSameSubGrid(innerColWithValue));
                foreach (InnerCol col in innerColsInSameCol)
                {
                    col.FlagCantHaveNumber(number);
                }
            }
        }

        private void TryFlagCells(IEnumerable<Cell> subGridCells, IEnumerable<Cell> rowOtherCells, short number, int possibleCellsNb)
        {
            if (subGridCells.Count((cell) => cell.CanHaveValue(number)) == possibleCellsNb)
            {
                foreach (Cell cell in rowOtherCells)
                {
                    cell.FlagCantHaveNumber(number);
                }
            }
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
