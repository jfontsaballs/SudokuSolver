using System.Linq;

namespace SudokuSolver
{
    public class SinglePossibilitySolverStrategy : ISolverStrategy
    {
        public Sudoku Solve(Sudoku sudoku)
        {
            return sudoku.ForEachEmpty((_, __, f, c, q) => {
                try {
                    return Enumerable.Range(1, 9).Except(f.Union(c).Union(q)).Single();
                }
                catch {
                    return 0;
                }
            });
        }
    }
}
