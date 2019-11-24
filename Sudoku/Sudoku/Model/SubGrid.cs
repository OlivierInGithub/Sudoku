using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class SubGrid
    {
        public List<Cell> Cells { get; set; }

        public SubGrid()
        {
            Cells = new List<Cell>();
        }

        public bool HasNumber(short number)
        {
            return Cells.Any((cell) => cell.Value == number);
        }

        public void FlagCellsThatCantHaveNumber(short number)
        {
            foreach (Cell cell in Cells)
            {
                if (cell.Value != number)
                {
                    cell.FlagCantHaveNumber(number);
                }
            }
        }

        public bool SetToOnlyPossibleCell(short number)
        {
            if (Cells.Count((c) => c.CanHaveValue[number-1]) == 1)
            {
                Cells.First((c) => c.CanHaveValue[number - 1]).Value = number;
                return true;
            }
            return false;
        }
    }
}
