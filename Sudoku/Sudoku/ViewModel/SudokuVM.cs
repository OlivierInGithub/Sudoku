using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;

namespace Sudoku
{
    public class SudokuVM : INotifyPropertyChanged
    {
        private FullGrid _mainGrid { get; set; }
        private const string FilePath = "c:\\Temp\\Sudoku.json";
        private SelectedNumber _selectedNumber;


        public BindingList<SubGridVM> SubGrids { get; set; }
        public ICommand ValidateCellsCmd { get; set; }
        public ICommand SaveCmd { get; set; }
        public ICommand LoadCmd { get; set; }
        public ICommand NumberButtonCmd { get; set; }
        public string SelectedNumberLabel
        {
            get { return $"Selected number: {_selectedNumber.Value}"; }
        }
        

        public SudokuVM()
        {
            _mainGrid = new FullGrid();
            _selectedNumber = new SelectedNumber { Value = 1 };
            SubGrids = new BindingList<SubGridVM>();
            foreach (SubGrid subGrid in _mainGrid.SubGrids)
            {
                SubGrids.Add(new SubGridVM(subGrid, _selectedNumber));
            }
            ValidateCellsCmd = new RelayCommand<object>((o) => ValidateCells());
            SaveCmd = new RelayCommand<object>((o) => SaveCells());
            LoadCmd = new RelayCommand<object>((o) => LoadCells());
            NumberButtonCmd = new RelayCommand<Button>((button) => ApplyNumberButton(button));
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

        private void SaveCells()
        {
            var cellValues = _mainGrid.Cells.Select((cell) => cell.Value).ToArray();
            using (StreamWriter file = File.CreateText(FilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, cellValues);
            }
        }

        private void LoadCells()
        {
            var cellValues = JsonConvert.DeserializeObject<short[]>(File.ReadAllText(FilePath));

            int index = 0;
            foreach (Cell cell in _mainGrid.Cells)
            {
                cell.Value = cellValues[index];
                index++;
            }
            RefreshAll();
        }

        private void RefreshAll()
        {
            foreach (SubGridVM subGrid in SubGrids)
            {
                subGrid.Refresh();
            }
        }

        private void ApplyNumberButton(Button button)
        {
            _selectedNumber.Value = short.Parse(button.Content.ToString());
            OnPropertyChanged(nameof(SelectedNumberLabel));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class SelectedNumber
    {
        public short Value { get; set; }
    }
}
