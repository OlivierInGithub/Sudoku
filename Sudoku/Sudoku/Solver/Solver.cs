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

            if (TrySetOneCell(number))
                return true;

            FlagMoreCells(number);
            
            return false;
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

        private void FlagMoreCells(short number)
        {
            for (int subGridRow = 0; subGridRow < 3; subGridRow++)
            {
                FlagMoreRowCells(number, _mainGrid.SubGrids3x3[subGridRow * 3], subGridRow, (cellList) => cellList.Skip(3));
                FlagMoreRowCells(number, _mainGrid.SubGrids3x3[subGridRow * 3 + 1], subGridRow, (cellList) => cellList.Take(3).Concat(cellList.Skip(6)));
                FlagMoreRowCells(number, _mainGrid.SubGrids3x3[subGridRow * 3 + 2], subGridRow, (cellList) => cellList.Take(6));
            }
            //TODO same for colmuns?
        }

        private void FlagMoreRowCells(short number, SubGrid subGrid, int subGridRow, Func<List<Cell>, IEnumerable<Cell>> selectCells)
        {
            if (!subGrid.Cells.Any((cell) => cell.Value == number))
            {
                int possibleCellsNb = subGrid.Cells.Count((cell) => cell.CanHaveValue(number));
                if (possibleCellsNb == 2 || possibleCellsNb == 3)
                {
                    var subGridInnerRows = GetInnerRows(subGrid);
                    for (int subGridInnerRowId = 0; subGridInnerRowId < 3; subGridInnerRowId++)
                    {
                        TryFlagCells(subGridInnerRows[subGridInnerRowId], selectCells(_mainGrid.Rows[subGridRow * 3 + subGridInnerRowId].Cells), number, possibleCellsNb);
                    }
                }
            }
        }

        private List<IEnumerable<Cell>> GetInnerRows(SubGrid subGrid)
        {
            return new List<IEnumerable<Cell>> {
                            subGrid.Cells.Take(3),
                            subGrid.Cells.Skip(3).Take(3),
                            subGrid.Cells.Skip(6)
            };
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
            if (!_mainGrid.IsValid())
                throw new Exception("Input grid is not valid!");
            while (TrySolveOneCell())
            {
                if (_mainGrid.Cells.All((cell) => cell.Value > 0))
                    return true;
            }
            return false;
        }
    }
}
