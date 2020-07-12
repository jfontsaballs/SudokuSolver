using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuSolver;
using SudokuSolver.Tests;
using System;

namespace SudokuSolverPerformanceTests
{
    public class SudokuSolverPerformanceTests
    {
        [Benchmark]
        public Sudoku BruteForceSolver_Facil() => BruteForceSolver.Solve(Sudokus.Easy.Sudoku);

        [Benchmark]
        public Sudoku BruteForceSolver_Intermedi() => BruteForceSolver.Solve(Sudokus.Intermediate.Sudoku);

        [Benchmark]
        public Sudoku BruteForceSolver_Dificil() => BruteForceSolver.Solve(Sudokus.Difficult.Sudoku);
       
        [Benchmark]
        public Sudoku BruteForceSolver_Expert() => BruteForceSolver.Solve(Sudokus.Expert.Sudoku);

        [Benchmark]
        public Sudoku SinglePossibilitySolver_Facil() => new SinglePossibilitySolverStrategy().Solve(Sudokus.Easy.Sudoku);

        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SudokuSolverPerformanceTests>();
            Console.ReadLine();
        }
    }
}
