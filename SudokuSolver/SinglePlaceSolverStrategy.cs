using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static SudokuSolver.SudokuExtensions;

namespace SudokuSolver
{
    public class SinglePlaceSolverStrategy : ISolverStrategy
    {
        public Sudoku Solve(Sudoku sudoku)
        {
            ImmutableArray<ImmutableArray<IEnumerable<int>>> calculatePossibilities()
            {
                var p = Enumerable.Range(0, 9)
                        .Select(_ => Enumerable.Range(0, 9).Select(x => Enumerable.Empty<int>()).ToImmutableArray())
                        .ToImmutableArray();
                sudoku.ForEachEmpty((f, c) => {
                    p = p.SetItem(f - 1, p[f - 1].SetItem(c - 1, SinglePossibilitySolverStrategy.GetPossibilities(sudoku, f, c)));
                    return 0;
                });
                return p;
            }

            void analyzeSinglePlace(ImmutableArray<IEnumerable<int>> row, Action<(int n, int i)> setter)
            {
                var singlePossibilities = row
                    .SelectMany((ns, i) => ns?.Select(n => (n, i)) ?? Enumerable.Empty<(int, int)>())
                    .GroupBy(x => x.n)
                    .Where(g => g.Count() == 1);
                foreach (var possibility in singlePossibilities)
                    setter(possibility.Single());
            }

            var possibilities = calculatePossibilities();
            ForEachPosition(p =>
                analyzeSinglePlace(
                    possibilities.Row(p),
                    x => sudoku = sudoku.Set(p, x.i + 1, x.n)
                ));

            possibilities = calculatePossibilities();
            ForEachPosition(p =>
                analyzeSinglePlace(
                    possibilities.Column(p),
                    x => sudoku = sudoku.Set(x.i + 1, p, x.n)
                ));

            possibilities = calculatePossibilities();
            ForEachPosition(p => {
                var squareIndex = p - 1;
                var squareStartX = squareIndex % 3 * 3 + 1;
                var squareStartY = squareIndex / 3 * 3 + 1;
                analyzeSinglePlace(
                        possibilities.Square(p),
                        x => sudoku = sudoku.Set(squareStartY + x.i / 3, squareStartX + x.i % 3, x.n)
                    );
            });

            return sudoku;
        }
    }
}
