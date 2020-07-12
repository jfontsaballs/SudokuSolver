using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuSolver;
using SudokuSolver.Tests;
using System;

namespace SudokuSolverPerformanceTests
{
    public class SudokuSolverPerformanceTests
    {
        private CombinationSudokuSolver CombinationSolver => new CombinationSudokuSolver(new SinglePossibilitySolverStrategy(), new BruteForceSolverStrategy());

        [Benchmark]
        public Sudoku BruteForceSolver_Facil() => new BruteForceSolverStrategy().Solve(Sudokus.Easy.Sudoku);

        [Benchmark]
        public Sudoku BruteForceSolver_Intermedi() => new BruteForceSolverStrategy().Solve(Sudokus.Intermediate.Sudoku);

        [Benchmark]
        public Sudoku BruteForceSolver_Dificil() => new BruteForceSolverStrategy().Solve(Sudokus.Difficult.Sudoku);
       
        [Benchmark]
        public Sudoku BruteForceSolver_Expert() => new BruteForceSolverStrategy().Solve(Sudokus.Expert.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) SinglePossibilitySolver_Facil() => new SudokuSolver.CombinationSudokuSolver(new SinglePossibilitySolverStrategy()).Solve(Sudokus.Easy.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) Single_BruteForceSolver_Intermedi() => CombinationSolver.Solve(Sudokus.Intermediate.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) Single_BruteForceSolver_Dificil() => CombinationSolver.Solve(Sudokus.Difficult.Sudoku);

        [Benchmark]
        public (Sudoku, ISolverStrategy) Single_BruteForceSolver_Expert() => CombinationSolver.Solve(Sudokus.Expert.Sudoku);

        static void Main()
        {
            BenchmarkRunner.Run<SudokuSolverPerformanceTests>();
            Console.ReadLine();
        }
    }
}
