using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SudokuVM _sudokuVM;

        public MainWindow()
        {
            _sudokuVM = new SudokuVM();
            DataContext = _sudokuVM;
            _sudokuVM.PropertyChanged += SudokuVM_PropertyChanged;
            InitializeComponent();
        }

        private void SudokuVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SudokuVM.SelectedNumber))
            {
                var resourceName = _sudokuVM.SelectedNumber == 0 ? "CursorReset" : $"Cursor{_sudokuVM.SelectedNumber}";
                SudokuCells.Cursor = ((TextBlock)this.Resources[resourceName]).Cursor;
            }
        }
    }
}
