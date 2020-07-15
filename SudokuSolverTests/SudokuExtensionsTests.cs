using NUnit.Framework;

namespace SudokuSolver.Tests
{
    public class SudokuExtensionsTests
    {
        [Test]
        public void CheckTest()
        {
            var sudoku = Sudokus.Easy.Sudoku;

            Assert.That(sudoku.Check(3, 9, 1));
            Assert.That(!sudoku.Check(3, 9, 2));

            Assert.That(sudoku.Check(5, 7, 8));
            Assert.That(!sudoku.Check(5, 7, 7));

            Assert.That(sudoku.Check(7, 8, 7));
            Assert.That(!sudoku.Check(7, 8, 6));
        }

        [Test]
        public void SudokuPart()
        {
            var sudoku = Sudokus.Easy.Sudoku;

            Assert.That(sudoku.Row(3), Is.EqualTo(new[] { 9, 3, 6, 8, 7, 4, 2, 5, 0 }));
            Assert.That(sudoku.Column(2), Is.EqualTo(new[] { 0, 5, 3, 6, 0, 0, 4, 8, 1 }));
            Assert.That(sudoku.Square(2), Is.EqualTo(new[] { 0, 1, 5, 0, 0, 0, 8, 7, 4 }));
        }
    }
}