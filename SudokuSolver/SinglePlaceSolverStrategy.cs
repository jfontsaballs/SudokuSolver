using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static SudokuSolver.SudokuUtils;

namespace SudokuSolver
{
    public class SinglePlaceSolverStrategy : ISolverStrategy
    {
        public Sudoku Solve(Sudoku sudoku)
        {
            void analyzeSinglePlace(ImmutableArray<IEnumerable<int>> row, Action<(int n, int i)> setter)
            {
                var singlePossibilities = row
                    .SelectMany((ns, i) => ns?.Select(n => (n, i)) ?? Enumerable.Empty<(int, int)>())
                    .GroupBy(x => x.n)
                    .Where(g => g.Count() == 1);
                foreach (var possibility in singlePossibilities)
                    setter(possibility.Single());
            }

            For1To9(p =>
                analyzeSinglePlace(
                    sudoku.RowPossibilities(p),
                    x => sudoku = sudoku.Set(p, x.i + 1, x.n)
                ));

            For1To9(p =>
                analyzeSinglePlace(
                    sudoku.ColumnPossibilities(p),
                    x => sudoku = sudoku.Set(x.i + 1, p, x.n)
                ));

            For1To9(p => {
                var squareIndex = p - 1;
                var squareStartX = squareIndex % 3 * 3 + 1;
                var squareStartY = squareIndex / 3 * 3 + 1;
                analyzeSinglePlace(
                        sudoku.SquarePossibilities(p),
                        x => sudoku = sudoku.Set(squareStartY + x.i / 3, squareStartX + x.i % 3, x.n)
                    );
            });

            return sudoku;
        }
    }
}
