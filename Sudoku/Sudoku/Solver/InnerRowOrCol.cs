using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Solver
{
    internal class InnerRowOrCol
    {
        private List<Cell> _cells;
        private int _subGridId;
        private int _rowOrColId;

        public InnerRowOrCol(IEnumerable<Cell> cells, int subGridId, int rowOrColId)
        {
            _cells = cells.ToList();
            _subGridId = subGridId;
            _rowOrColId = rowOrColId;
        }

        public bool CanHaveValue(short number)
        {
            return _cells.Any((cell) => cell.CanHaveValue(number));
        }

        public void FlagCantHaveNumber(short number)
        {
            foreach (Cell cell in _cells)
            {
                cell.FlagCantHaveNumber(number);
            }
        }

        public bool IsInSubGrid(int subGridId)
        {
            return _subGridId == subGridId;
        }

        public bool IsInSameSubGrid(InnerRowOrCol rowOrCol)
        {
            return rowOrCol._subGridId == _subGridId;
        }

        protected bool HasSameRowOrColId(InnerRowOrCol rowOrCol)
        {
            return rowOrCol._rowOrColId == _rowOrColId;
        }
    }

    internal class InnerRow : InnerRowOrCol
    {
        public InnerRow(IEnumerable<Cell> cells, int subGridId, int rowId)
            : base(cells, subGridId, rowId)
        {
        }

        public bool IsInSameRow(InnerRow row)
        {
            return HasSameRowOrColId(row);
        }
    }

    internal class InnerCol : InnerRowOrCol
    {
        public InnerCol(IEnumerable<Cell> cells, int subGridId, int colId)
            : base(cells, subGridId, colId)
        {
        }

        public bool IsInSameCol(InnerCol col)
        {
            return HasSameRowOrColId(col);
        }
    }
}
