using NUnit.Framework;

namespace SudokuSolver.Tests
{
    public class SudokuExtensionsTests
    {
        [Test]
        public void CheckTest()
        {
            Sudoku sudoku = Sudokus.Facil();

            Assert.That(sudoku.Check(3, 9, 1));
            Assert.That(!sudoku.Check(3, 9, 2));

            Assert.That(sudoku.Check(5, 7, 8));
            Assert.That(!sudoku.Check(5, 7, 7));

            Assert.That(sudoku.Check(7, 8, 7));
            Assert.That(!sudoku.Check(7, 8, 6));
        }
    }
}