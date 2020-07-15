using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class SinglePossibilitySolverStrategy : ISolverStrategy
    {
        public Sudoku Solve(Sudoku sudoku)
        {
            return sudoku.ForEachEmpty((f, q) => {
                try {
                    return GetPossibilities(sudoku, f, q).Single();
                }
                catch {
                    return 0;
                }
            });
        }

        public static IEnumerable<int> GetPossibilities(Sudoku sudoku, int fila, int columna)
        {
            var (f, c, q) = sudoku.Get(fila, columna);
            return Enumerable.Range(1, 9).Except(f.Union(c).Union(q));
        }
    }
}
