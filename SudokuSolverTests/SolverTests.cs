using NUnit.Framework;

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
            Sudokus.TestSudoku(Sudokus.Easy, sudoku => new CombinationSudokuSolver(new SinglePossibilitySolverStrategy()).Solve(sudoku).Solution);
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

            Sudokus.TestSudoku(Sudokus.Easy, singlePossibilitySolverStrategy, solver.Solve);
            Sudokus.TestSudoku(Sudokus.Intermediate, singlePlaceSolverStrategy, solver.Solve);
            Sudokus.TestSudoku(Sudokus.Difficult, singlePlaceSolverStrategy, solver.Solve);
            Sudokus.TestSudoku(Sudokus.Expert, bruteForceSolverStrategy, solver.Solve);
        }
    }
}