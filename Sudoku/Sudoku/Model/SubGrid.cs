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
            if (Cells.Count((c) => c.CanHaveValue(number)) == 1)
            {
                Cells.First((c) => c.CanHaveValue(number)).Value = number;
                return true;
            }
            return false;
        }

        public void SetValues(short[] values)
        {
            if (values.Length != Cells.Count)
                throw new ArgumentOutOfRangeException("SetValues parameter should contain 9 values");
            for (int i=0; i < values.Length; i++)
            {
                Cells[i].Value = values[i];
            }
        }

        public bool IsValid()
        {
            var hasValue = new bool[9];
            foreach (Cell cell in Cells)
            {
                var value = cell.Value;
                if (value > 0)
                {
                    var index = value - 1;
                    if (hasValue[index])
                        return false;
                    else
                        hasValue[index] = true;
                }
            }
            return true;
        }

        public override string ToString()
        {
            return String.Join(", ",Cells.Select((cell) => cell.ToString()));
        }
    }
}
