using System;

namespace SudokuSolver
{
    public class BruteForceSolver
    {
        public static Sudoku Solve(Sudoku sudoku) => solve(sudoku, 0);

        private static Sudoku solve(Sudoku sudoku, int i)
        {
            int fila = i / 9 + 1;
            int columna = i % 9 + 1;
            if (i > 80)
                return sudoku;
            else if (sudoku[fila, columna] > 0)
                return solve(sudoku, i + 1);
            else
                for (int j = 1; j <= 9; j++)
                    try {
                        var solucio = solve(sudoku.Set(fila, columna, j), i + 1);
                        return solucio;
                    }
                    catch {
                        continue;
                    }
            throw new Exception("No té solució");
        }
    }
}
