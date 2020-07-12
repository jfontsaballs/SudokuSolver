using NUnit.Framework;

namespace SudokuSolver.Tests
{
    public class SolverTests
    {
        [Test]
        public void BruteForceSolverTest()
        {
            Sudokus.TestSudokus(BruteForceSolver.Solve);
        }

        [Test]
        public void SinglePossibilitySolverTest()
        {
            Sudokus.TestSudoku(Sudokus.Easy, SinglePossibilitySolver.Solve);
        }
    }
}