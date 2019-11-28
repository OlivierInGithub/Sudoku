using System;
using System.Linq;

namespace Sudoku
{
    public class Cell
    {
        public Int16 Value { get; set; }
        public bool IsValidated { get; set; }
        public bool[] CanHaveValue;

        public Cell()
        {
            Value = 0;
            IsValidated = false;
            CanHaveValue = new bool[9];
            ResetStatus();
        }

        public void ResetStatus()
        {
            for (int i = 1; i <= 9; i++)
            {
                CanHaveValue[i - 1] = true;
            }
        }

        public void SetOnlyPossibleValue(short number)
        {
            for (int i=1; i<= 9; i++)
            {
                if (i != number)
                    CanHaveValue[i - 1] = false;
            }
        }


        public bool SetToOnlyPossibleValue()
        {
            if (CanHaveOnlyOneValue())
            {
                for (short i=1; i<=9; i++)
                {
                    if (CanHaveValue[i - 1])
                    {
                        Value = i;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanHaveOnlyOneValue()
        {
            return CanHaveValue.Count((b) => b == true) == 1;
        }

        public void FlagCantHaveNumber(short number)
        {
            CanHaveValue[number - 1] = false;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
