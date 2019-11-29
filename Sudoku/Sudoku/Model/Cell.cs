using System;
using System.Linq;

namespace Sudoku
{
    public class Cell
    {
        public Int16 Value { get; set; }
        public bool IsValidated { get; set; }
        private bool[] _canHaveValue;

        public Cell()
        {
            Value = 0;
            IsValidated = false;
            _canHaveValue = new bool[9];
            ResetStatus();
        }

        public void ResetStatus()
        {
            for (int i = 1; i <= 9; i++)
            {
                _canHaveValue[i - 1] = true;
            }
        }

        public void SetOnlyPossibleValue(short number)
        {
            for (int i=1; i<= 9; i++)
            {
                if (i != number)
                    _canHaveValue[i - 1] = false;
            }
        }


        public bool SetToOnlyPossibleValue()
        {
            if (CanHaveOnlyOneValue())
            {
                for (short i=1; i<=9; i++)
                {
                    if (_canHaveValue[i - 1])
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
            return _canHaveValue.Count((b) => b == true) == 1;
        }

        public void FlagCantHaveNumber(short number)
        {
            _canHaveValue[number - 1] = false;
        }

        public bool CanHaveValue(short number)
        {
            return _canHaveValue[number - 1];
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
