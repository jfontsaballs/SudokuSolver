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
            var bruteForceSolverStrategy = new BruteForceSolverStrategy();
            var solver = new CombinationSudokuSolver(
                singlePossibilitySolverStrategy,
                bruteForceSolverStrategy);

            Sudokus.TestSudoku(Sudokus.Easy, singlePossibilitySolverStrategy, solver.Solve);
            Sudokus.TestSudoku(Sudokus.Intermediate, bruteForceSolverStrategy, solver.Solve);
            Sudokus.TestSudoku(Sudokus.Difficult, bruteForceSolverStrategy, solver.Solve);
            Sudokus.TestSudoku(Sudokus.Expert, bruteForceSolverStrategy, solver.Solve);
        }
    }
}