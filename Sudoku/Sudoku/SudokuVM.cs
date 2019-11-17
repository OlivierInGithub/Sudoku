using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sudoku
{
    public class SudokuVM
    {
        public FullGrid MainGrid { get; set; }
        public ICommand ValidateCellsCmd { get; set; }

        public SudokuVM()
        {
            MainGrid = new FullGrid();
            ValidateCellsCmd = new RelayCommand<object>((o) => ValidateCells());
        }

        private void ValidateCells()
        {
            foreach (Cell cell in MainGrid.Cells)
            {
                if (cell.Value > 0)
                    cell.IsValidated = true;
            }
        }


    }
}
