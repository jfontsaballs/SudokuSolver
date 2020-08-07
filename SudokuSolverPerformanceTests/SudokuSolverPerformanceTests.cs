using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuSolver;
using SudokuSolver.Tests;
using System;

namespace SudokuSolverPerformanceTests
{
    public class SudokuSolverPerformanceTests
    {
        private CombinationSudokuSolver CombinationSolver => new CombinationSudokuSolver(new SinglePossibilitySolverStrategy(), new SinglePlaceSolverStrategy(), new BruteForceSolverStrategy());

        [Benchmark]
        public Sudoku BruteForceSolver_Facil() => new BruteForceSolverStrategy().Solve(Sudokus.Easy.Sudoku);

        [Benchmark]
        public Sudoku BruteForceSolver_Intermedi() => new BruteForceSolverStrategy().Solve(Sudokus.Intermediate.Sudoku);

        [Benchmark]
        public Sudoku BruteForceSolver_Dificil() => new BruteForceSolverStrategy().Solve(Sudokus.Difficult.Sudoku);
       
        [Benchmark]
        public Sudoku BruteForceSolver_Expert() => new BruteForceSolverStrategy().Solve(Sudokus.Expert.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) SinglePossibilitySolver_Facil() => new SudokuSolver.CombinationSudokuSolver(new SinglePossibilitySolverStrategy()).ExecuteSolve(Sudokus.Easy.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) SinglePlaceSolver_Intermedi() => CombinationSolver.ExecuteSolve(Sudokus.Intermediate.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) SinglePlaceSolver_Dificil() => CombinationSolver.ExecuteSolve(Sudokus.Difficult.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) Single_BruteForceSolver_Expert() => CombinationSolver.ExecuteSolve(Sudokus.Expert.Sudoku);

        static void Main()
        {
            BenchmarkRunner.Run<SudokuSolverPerformanceTests>();
            Console.ReadLine();
        }
    }
}
