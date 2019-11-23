using System;

namespace Sudoku
{
    public class Cell
    {
        public Int16 Value { get; set; }
        public bool IsValidated { get; set; }

        public Cell()
        {
            Value = 0;
            IsValidated = false;
        }
    }
}
