using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System;
using Microsoft.Win32;

namespace Sudoku
{
    public class SudokuVM : INotifyPropertyChanged
    {
        private FullGrid _mainGrid { get; set; }
        private const string FilePath = "c:\\Temp\\Sudoku.json";
        private SelectedNumber _selectedNumber;
        private List<SubCellsVM> _rows;
        private List<SubCellsVM> _columns;


        public BindingList<SubGridVM> SubGrids { get; set; }

        public ICommand ValidateCellsCmd { get; set; }
        public ICommand SaveCmd { get; set; }
        public ICommand LoadCmd { get; set; }
        public ICommand NumberButtonCmd { get; set; }
        public ICommand ResetSelectedNumberCmd { get; set; }
        public ICommand HideCellsCmd { get; set; }
        public ICommand ShowCellsCmd { get; set; }
        public ICommand TrySolveOneCellCmd { get; set; }
        public ICommand SolveGridCmd { get; set; }

        public string SelectedNumberLabel
        {
            get { return $"Selected number: {_selectedNumber.Value}"; }
        }

        public short SelectedNumber
        {
            get { return _selectedNumber.Value; }
            private set
            {
                _selectedNumber.Value = value;
                OnPropertyChanged(nameof(SelectedNumber));
                OnPropertyChanged(nameof(SelectedNumberLabel));
            }
        }

        bool _isHidingCells;
        public bool IsHindingCells
        {
            get { return _isHidingCells; }
            set
            {
                _isHidingCells = value;
                OnPropertyChanged((nameof(IsHindingCells)));
            }
        }

        public string HideCellsLabel
        {
            get
            {
                return $"Hide cells not having {_selectedNumber.Value}";
            }
        }

        public SudokuVM()
        {
            _mainGrid = new FullGrid();
            _selectedNumber = new SelectedNumber { Value = 1 };
            _rows = new List<SubCellsVM>(9);
            _columns = new List<SubCellsVM>(9);
            SubGrids = new BindingList<SubGridVM>();

            InitCells();
            InitCommands();    
        }

        private void InitCells()
        {
            Enumerable.Range(1, 9).ToList().ForEach((i) => _rows.Add(new SubCellsVM()));
            Enumerable.Range(1, 9).ToList().ForEach((i) => _columns.Add(new SubCellsVM()));

            foreach (SubGrid subGrid in _mainGrid.SubGrids3x3)
            {
                SubGrids.Add(new SubGridVM(subGrid, _selectedNumber));
            }

            for (int rowId=0; rowId < 9; rowId++)
            {
                var subGridOffset = 3 * (rowId / 3);
                var rowOffset = 3 * (rowId % 3);
                _rows[rowId].Cells.AddRange(SubGrids[subGridOffset].Cells.Skip(rowOffset).Take(3));
                _rows[rowId].Cells.AddRange(SubGrids[subGridOffset + 1].Cells.Skip(rowOffset).Take(3));
                _rows[rowId].Cells.AddRange(SubGrids[subGridOffset + 2].Cells.Skip(rowOffset).Take(3));
            }
            
            for (int colId=0; colId < 9; colId++)
            {
                for (int rowId = 0; rowId < 9; rowId++)
                {
                    _columns[colId].Cells.Add(_rows[rowId].Cells[colId]);
                }
            }
        }

        private void InitCommands()
        {
            ValidateCellsCmd = new RelayCommand<object>((o) => ValidateCells());
            SaveCmd = new RelayCommand<object>((o) => SaveCells());
            LoadCmd = new RelayCommand<object>((o) => LoadCells());
            NumberButtonCmd = new RelayCommand<Button>((button) => ApplyNumberButton(button));
            ResetSelectedNumberCmd = new RelayCommand<object>((o) => ResetSelectedNumber());
            HideCellsCmd = new RelayCommand<object>((o) => HideOrUnhideCells());
            ShowCellsCmd = new RelayCommand<object>((o) => ShowCells());
            TrySolveOneCellCmd = new RelayCommand<object>((o) => TrySolveOneCell());
            SolveGridCmd = new RelayCommand<object>((o) => SolveGrid());
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == false)
                return;
            var cellValues = _mainGrid.Cells.Select((cell) => cell.Value).ToArray();
            using (StreamWriter file = File.CreateText(saveFileDialog.FileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, cellValues);
            }
        }

        private void LoadCells()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false)
                return;
            var cellValues = JsonConvert.DeserializeObject<short[]>(File.ReadAllText(openFileDialog.FileName));

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
            SelectedNumber = short.Parse(button.Content.ToString());
            OnPropertyChanged((nameof(HideCellsLabel)));
        }

        private void ResetSelectedNumber()
        {
            SelectedNumber = 0;
            OnPropertyChanged(nameof(SelectedNumberLabel));
            OnPropertyChanged((nameof(HideCellsLabel)));
        }

        private void HideOrUnhideCells()
        {
            ShowCells();
            HideCellsAccordingToNumber(_selectedNumber.Value);
        }

        private void HideCellsAccordingToNumber(short number)
        {
            foreach (SubGridVM subGrid in SubGrids)
            {
                subGrid.HideCellsAccordingToNumber(number);
            }
            foreach (SubCellsVM row in _rows)
            {
                row.HideCellsAccordingToNumber(number);
            }
            foreach (SubCellsVM column in _columns)
            {
                column.HideCellsAccordingToNumber(number);
            }
            IsHindingCells = true;
        }

        private void ShowCells()
        {
            foreach (SubGridVM subGrid in SubGrids)
            {
                foreach (CellVM cell in subGrid.Cells)
                {
                    cell.ToHide = false;
                }
            }
            IsHindingCells = false;
        }

        private void TrySolveOneCell()
        {
            var solver = new Solver.Solver(_mainGrid);
            if (solver.TrySolveOneCell())
            {
                RefreshAll();
            }
            else
            {
                MessageBox.Show("No obvious cell :(");
            }
        }

        private void SolveGrid()
        {
            try
            {
                var solver = new Solver.Solver(_mainGrid);
                if (solver.TrySolveGrid())
                {
                    RefreshAll();
                }
                else
                {
                    MessageBox.Show("Can not solve this grid :(");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }

    public class SelectedNumber
    {
        public short Value { get; set; }
    }
}
