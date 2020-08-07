using NUnit.Framework;
using System.Linq;

namespace SudokuSolver.Tests
{
    public class SolverTests
    {
        [Test]
        public void BruteForceSolverTest()
        {
            Sudokus.TestSudokus(new BruteForceSolverStrategy().Solve);
        }

        [Test]
        public void SinglePossibilitySolverTest()
        {
            Assert.That(SinglePossibilitySolverStrategy.GetPossibilities(Sudokus.Easy.Sudoku, 7, 8).Single(), Is.EqualTo(7));

            Sudokus.TestSudoku(Sudokus.Easy, new CombinationSudokuSolver(new SinglePossibilitySolverStrategy()).Solve);
        }

        [Test]
        public void SolverTest()
        {
            var singlePossibilitySolverStrategy = new SinglePossibilitySolverStrategy();
            var singlePlaceSolverStrategy = new SinglePlaceSolverStrategy();
            var bruteForceSolverStrategy = new BruteForceSolverStrategy();
            var solver = new CombinationSudokuSolver(
                singlePossibilitySolverStrategy,
                singlePlaceSolverStrategy,
                bruteForceSolverStrategy);

            Sudokus.TestSudoku(Sudokus.Easy, singlePossibilitySolverStrategy, solver.ExecuteSolve);
            Sudokus.TestSudoku(Sudokus.Intermediate, singlePlaceSolverStrategy, solver.ExecuteSolve);
            Sudokus.TestSudoku(Sudokus.Difficult, singlePlaceSolverStrategy, solver.ExecuteSolve);
            Sudokus.TestSudoku(Sudokus.Expert, bruteForceSolverStrategy, solver.ExecuteSolve);
        }
    }
}