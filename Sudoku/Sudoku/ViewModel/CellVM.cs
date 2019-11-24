using System.ComponentModel;
using System.Windows.Input;

namespace Sudoku
{
    public class CellVM : INotifyPropertyChanged
    {
        private Cell _cell;
        private SelectedNumber _selectedNumber;

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

        private bool _toHide;
        public bool ToHide {
            get { return _toHide; }
            set { _toHide = value; OnPropertyChanged((nameof(ToHide))); }
        }

        public ICommand CellClickCmd { get; set; }

        public CellVM(Cell cell, SelectedNumber selectedNumber)
        {
            _cell = cell;
            _selectedNumber = selectedNumber;
            CellClickCmd = new RelayCommand<object>((o) => CellClick());
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(Value));
        }

        private void CellClick()
        {
            Value = _selectedNumber.Value;
        }
    }
}
