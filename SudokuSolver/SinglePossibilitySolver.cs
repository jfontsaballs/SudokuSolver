using System.Linq;

namespace SudokuSolver
{
    public class SinglePossibilitySolver
    {
        public static Sudoku Solve(Sudoku sudoku)
        {
            Sudoku previousSudoku;
            do {
                previousSudoku = sudoku;

                sudoku = sudoku.ForEachEmpty((_, __, f, c, q) => {
                    try {
                        return Enumerable.Range(1, 9).Except(f.Union(c).Union(q)).Single();
                    }
                    catch {
                        return 0;
                    }
                });
            } while (previousSudoku != sudoku);
            return sudoku;
        }
    }
}
