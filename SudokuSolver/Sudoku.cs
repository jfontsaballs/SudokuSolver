using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class Sudoku : IEquatable<Sudoku>, IEnumerable<int>
    {
        private readonly ImmutableArray<ImmutableArray<int>> sudoku;

        public int this[int fila, int columna] => sudoku[fila - 1][columna - 1];

        public ImmutableArray<int> Row(int position) => sudoku.Row(position);
        public ImmutableArray<int> Column(int position) => sudoku.Column(position);
        public ImmutableArray<int> Square(int position) => sudoku.Square(position);

        public Sudoku(params int[] values)
        {
            sudoku = values
                .Select((v, i) => (v, i))
                .GroupBy(x => x.i / 9)
                .Select(g => g.Select(x => x.v).ToImmutableArray())
                .ToImmutableArray();
        }

        private Sudoku(ImmutableArray<ImmutableArray<int>> sudoku)
        {
            this.sudoku = sudoku;
        }

        public Sudoku Set(int fila, int columna, int valor)
        {
            var (x, y) = getCoordenades(fila, columna);
            if (sudoku[x][y] != 0)
                throw new Exception("Aquesta posició ja està ocupada");
            if (!this.Check(fila, columna, valor))
                throw new Exception("Aquest valor no és correcte per aquesta posició");
            return new Sudoku(sudoku.SetItem(x, sudoku[x].SetItem(y, valor)));
        }

        public (ImmutableArray<int> fila, ImmutableArray<int> columna, ImmutableArray<int> quadrat)
            Get(int fila, int columna)
        {
            var (x, y) = getCoordenades(fila, columna);
            var (qx, qy) = (x / 3 * 3, y / 3 * 3);
            return (
                sudoku[x],
                sudoku
                    .Select(f => f[y])
                    .ToImmutableArray(),
                sudoku.Skip(qx)
                    .Take(3)
                    .SelectMany(row => row.Skip(qy).Take(3))
                    .ToImmutableArray());
        }

        private (int x, int y) getCoordenades(int fila, int columna) => (fila - 1, columna - 1);

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 9; i++) {
                sb.AppendLine();
                for (int j = 0; j < 9; j++)
                    sb.Append($"{sudoku[i][j]}, ");
            }
            return sb.ToString(0, sb.Length - 2);
        }

        public override bool Equals(object obj) => Equals(obj as Sudoku);
        public bool Equals(Sudoku other)
        {
            if (other is null)
                return false;

            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    if (this[i, j] != other[i, j])
                        return false;

            return true;
        }
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var row in sudoku)
                foreach (var element in row)
                    hashCode.Add(element);
            return hashCode.ToHashCode();
        }

        public IEnumerator<int> GetEnumerator() => sudoku.SelectMany(x => x).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator ==(Sudoku left, Sudoku right) => left?.Equals(right) == true;
        public static bool operator !=(Sudoku left, Sudoku right) => !(left == right);
    }

    public static class SudokuExtensions
    {
        public static bool Check(this Sudoku sudoku, int fila, int columna, int valor)
        {
            var (f, c, q) = sudoku.Get(fila, columna);
            return f.All(x => x != valor) && c.All(x => x != valor) && q.All(x => x != valor);
        }

        public static Sudoku ForEachEmpty(this Sudoku sudoku, Func<int, int, int> action)
        {
            ForEachPosition(i =>
                ForEachPosition(j => {
                    if (sudoku[i, j] == 0) {
                        var newValue = action(i, j);
                        if (newValue != 0)
                            sudoku = sudoku.Set(i, j, newValue);
                    }
                }));
            return sudoku;
        }

        public static ImmutableArray<T> Row<T>(this ImmutableArray<ImmutableArray<T>> square, int rowNumber)
            => square[rowNumber - 1];

        public static ImmutableArray<T> Column<T>(this ImmutableArray<ImmutableArray<T>> square, int columnNumber)
            => square.Select(row => row[columnNumber - 1]).ToImmutableArray();

        public static ImmutableArray<T> Square<T>(this ImmutableArray<ImmutableArray<T>> square, int squarePosition)
        {
            var squareIndex = squarePosition - 1;
            var squareStartX = squareIndex / 3 * 3;
            var squareStartY = squareIndex % 3 * 3;
            return square
                .Skip(squareStartX)
                .Take(3)
                .SelectMany(row => row.Skip(squareStartY).Take(3))
                .ToImmutableArray();
        }

        public static void ForEachPosition(Action<int> action)
        {
            for (int i = 1; i <= 9; i++)
                action(i);
        }
    }
}
