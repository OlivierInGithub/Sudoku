using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sudoku;

namespace UnitTest
{
    [TestClass]
    public class SolverTest
    {
        [TestMethod]
        public void SolverCanSolveEasyGrid()
        {
            var grid = new FullGrid();
            short[] values = {
                2, 0, 0, 4, 7, 3, 0, 0, 5,
                4, 7, 0, 0, 0, 0, 0, 1, 3,
                0, 3, 8, 0, 6, 0, 7, 4, 0,
                0, 9, 2, 0, 3, 0, 4, 8, 0,
                0, 4, 0, 6, 2, 8, 0, 7, 0,
                8, 0, 6, 0, 0, 0, 2, 0, 1,
                9, 0, 0, 0, 5, 0, 0, 0, 8,
                3, 1, 0, 0, 4, 0, 0, 2, 7,
                0, 0, 0, 2, 0, 9, 0, 0, 0
            };
            FillGrid(grid, values);

            var solver = new Solver(grid);
            var result = solver.TrySolveGrid();

            Assert.IsTrue(result, "Solver didn't succeed");
            Assert.IsTrue(grid.IsComplete(), "Grid is not complete");
            Assert.IsTrue(grid.IsValid(), "Grid is not valid");
        }

        private void FillGrid(FullGrid grid, short[] values)
        {
            for (int rowId=0; rowId<9; rowId++)
            {
                grid.Rows[rowId].SetValues(values.Skip(rowId*9).Take(9).ToArray());
            }
        }

        [TestMethod]
        public void SolverCanSolveDifficultGrid()
        {
            var grid = new FullGrid();
            short[] values = {
                0, 6, 0, 0, 7, 0, 0, 9, 0,
                0, 0, 0, 3, 0, 9, 0, 0, 0,
                0, 1, 0, 0, 0, 0, 0, 2, 0,
                0, 0, 0, 4, 2, 5, 0, 0, 0,
                2, 0, 0, 0, 8, 0, 0, 0, 6,
                4, 0, 0, 0, 0, 0, 0, 0, 1,
                0, 0, 4, 0, 0, 0, 5, 0, 0,
                0, 0, 0, 7, 1, 4, 0, 0, 0,
                0, 0, 9, 0, 0, 0, 8, 0, 0
            };
            FillGrid(grid, values);

            var solver = new Solver(grid);
            var result = solver.TrySolveGrid();

            Assert.IsTrue(result, "Solver didn't succeed");
            Assert.IsTrue(grid.IsComplete(), "Grid is not complete");
            Assert.IsTrue(grid.IsValid(), "Grid is not valid");
        }
    }
}
