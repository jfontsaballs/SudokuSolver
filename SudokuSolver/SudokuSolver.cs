using System;
using System.Linq;

namespace SudokuSolver
{
    public interface ISolverStrategy
    {
        Sudoku Solve(Sudoku sudoku);
    }

    public class CombinationSudokuSolver
    {
        private readonly ISolverStrategy[] strategies;

        public CombinationSudokuSolver(params ISolverStrategy[] strategies)
        {
            this.strategies = strategies;
        }

        public (Sudoku Solution, ISolverStrategy Strategy) Solve(Sudoku sudoku)
        {
            int maxStrategy = 0;
            for (int c = 0; c < 9999; c++) {
                if (sudoku.All(x => x != 0))
                    return (sudoku, strategies[maxStrategy]);
                else {
                    Sudoku previousSudoku;
                    for (var currentStrategy = 0; currentStrategy < strategies.Length; currentStrategy++) {
                        previousSudoku = sudoku;
                        sudoku = strategies[currentStrategy].Solve(sudoku);
                        if (maxStrategy < currentStrategy)
                            maxStrategy = currentStrategy;
                        if (previousSudoku != sudoku)
                            break;
                    }
                }
            }

            throw new Exception("Sudoku has no solution");
        }
    }
}
