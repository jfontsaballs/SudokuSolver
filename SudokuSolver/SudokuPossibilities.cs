using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SudokuSolver
{
    public partial class Sudoku
    {
        public ImmutableArray<IEnumerable<int>> RowPossibilities(int position) => Possibilities.Row(position);
        public ImmutableArray<IEnumerable<int>> ColumnPossibilities(int position) => Possibilities.Column(position);
        public ImmutableArray<IEnumerable<int>> SquarePossibilities(int position) => Possibilities.Square(position);

        private ImmutableArray<ImmutableArray<IEnumerable<int>>> _possibilities = ImmutableArray<ImmutableArray<IEnumerable<int>>>.Empty;
        public ImmutableArray<ImmutableArray<IEnumerable<int>>> Possibilities {
            get {
                if (_possibilities.Any())
                    return _possibilities;

                var p = Enumerable.Range(0, 9)
                        .Select(_ => Enumerable.Range(0, 9).Select(x => Enumerable.Empty<int>()).ToImmutableArray())
                        .ToImmutableArray();

                this.ForEachEmpty((f, c) => {
                    p = p.Set(f, c, SinglePossibilitySolverStrategy.GetPossibilities(this, f, c));
                    return 0;
                });
                
                return _possibilities = p;
            }
        }
    }
}
