using System.ComponentModel;
using System.Windows.Input;

namespace Sudoku
{
    public class SudokuVM
    {
        private FullGrid _mainGrid { get; set; }
        public BindingList<SubGridVM> SubGrids { get; set; }
        public ICommand ValidateCellsCmd { get; set; }

        public SudokuVM()
        {
            _mainGrid = new FullGrid();
            SubGrids = new BindingList<SubGridVM>();
            foreach (SubGrid subGrid in _mainGrid.SubGrids)
            {
                SubGrids.Add(new SubGridVM(subGrid));
            }
            ValidateCellsCmd = new RelayCommand<object>((o) => ValidateCells());
        }

        private void ValidateCells()
        {
            foreach (SubGridVM subGrid in SubGrids)
            {
                foreach (CellVM cell in subGrid.Cells)
                {
                    if (cell.Value > 0)
                        cell.IsValidated = true;
                }
            }
        }
    }

    public class SubGridVM
    {
        private SubGrid _subGrid;
        public BindingList<CellVM> Cells { get; set; }

        public SubGridVM(SubGrid subGrid)
        {
            _subGrid = subGrid;
            Cells = new BindingList<CellVM>();
            foreach (Cell cell in _subGrid.Cells)
            {
                Cells.Add(new CellVM(cell));
            }
        }
    }

    public class CellVM: INotifyPropertyChanged
    {
        private Cell _cell;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public short Value
        {
            get
            {
                return _cell.Value;
            }
            set
            {
                _cell.Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public bool IsValidated
        {
            get
            {
                return _cell.IsValidated;
            }
            set
            {
                _cell.IsValidated = true;
                OnPropertyChanged(nameof(IsValidated));
            }
        }

        public ICommand CellClickCmd { get; set; }

        public CellVM(Cell cell)
        {
            _cell = cell;
            CellClickCmd = new RelayCommand<object>((o) => CellClick());
        }

        private void CellClick()
        {
            var newValue = Value + 1;
            if (newValue > 9)
                newValue = 0;
            Value = (short)newValue;
        }
    }
}
