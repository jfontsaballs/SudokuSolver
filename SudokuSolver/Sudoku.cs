using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using static SudokuSolver.SudokuUtils;

namespace SudokuSolver
{
    public partial class Sudoku : IEquatable<Sudoku>
    {
        private readonly ImmutableArray<ImmutableArray<int>> sudoku;

        public int this[int fila, int columna] => sudoku.Get(fila, columna);

        public ImmutableArray<int> Row(int position) => sudoku.Row(position);
        public ImmutableArray<int> Column(int position) => sudoku.Column(position);
        public ImmutableArray<int> Square(int position) => sudoku.Square(position);
        public Sudoku ForEachPosition(Action<int, int, int, Action<int>> action) => new Sudoku(sudoku.ForEachPosition(action));
        public void ForEachPosition(Action<int, int, int> action) => sudoku.ForEachPosition(action);
        public IEnumerable<(int Fila, int Columna, int Valor)> GetAllPositions() => sudoku.GetAllPositions();

        public Sudoku(params int[] values)
        {
            sudoku = values
                .Select((v, i) => (v, i))
                .GroupBy(x => x.i / 9)
                .Select(g => g.Select(x => x.v).ToImmutableArray())
                .ToImmutableArray();
        }

        protected Sudoku(ImmutableArray<ImmutableArray<int>> sudoku)
        {
            this.sudoku = sudoku;
        }

        public virtual Sudoku Set(int fila, int columna, int valor)
        {
            if (this[fila, columna] != 0)
                throw new Exception("Aquesta posició ja està ocupada");
            if (!this.Check(fila, columna, valor))
                throw new Exception("Aquest valor no és correcte per aquesta posició");
            return new Sudoku(sudoku.Set(fila, columna, valor)) {
                _possibilities = updatePossibilities(fila, columna, valor)
            };
        }

        public (ImmutableArray<int> fila, ImmutableArray<int> columna, ImmutableArray<int> quadrat)
            Get(int fila, int columna) => (
                sudoku.Row(fila),
                sudoku.Column(columna),
                sudoku.Square(fila, columna));

        public override string ToString() => sudoku.PrettyPrint();

        public override bool Equals(object obj) => Equals(obj as Sudoku);
        public bool Equals(Sudoku other)
        {
            if (other is null)
                return false;

            return sudoku.GetAllPositions().All(p => this[p.Fila, p.Columna] == other[p.Fila, p.Columna]);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var row in sudoku)
                foreach (var element in row)
                    hashCode.Add(element);
            return hashCode.ToHashCode();
        }

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
            => sudoku.ForEachPosition((i, j, value, setter) => {
                if (sudoku[i, j] == 0) {
                    var newValue = action(i, j);
                    if (newValue != 0)
                        setter(newValue);
                }
            });
    }

    public static class ArrayOfArrayExtensions
    {
        public static T Get<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, int fila, int columna)
            => twoDimensionalArray[fila - 1][columna - 1];

        public static ImmutableArray<ImmutableArray<T>> Set<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, int fila, int columna, T item)
            => twoDimensionalArray.SetItem(fila - 1, twoDimensionalArray[fila - 1].SetItem(columna - 1, item));

        public static ImmutableArray<T> Row<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, int rowNumber)
            => twoDimensionalArray[rowNumber - 1];

        public static ImmutableArray<T> Column<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, int columnNumber)
            => twoDimensionalArray.Select(row => row[columnNumber - 1]).ToImmutableArray();

        public static ImmutableArray<T> Square<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, int squarePosition)
            => extractSquare(twoDimensionalArray, squareStart: getSquareStartForSquareNumber(squarePosition));
        
        private static (int, int) getSquareStartForSquareNumber(int squarePosition) 
            => ((squarePosition - 1) / 3 * 3, (squarePosition - 1) % 3 * 3);
        
        public static ImmutableArray<T> Square<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, int fila, int columna)
            => extractSquare(twoDimensionalArray, squareStart: GetSquareStartIndexForPosition(fila, columna));

        private static ImmutableArray<T> extractSquare<T>(ImmutableArray<ImmutableArray<T>> twoDimensionalArray, (int X, int Y) squareStart)
            // Square distribution in array
            // 0 1 2
            // 3 4 5
            // 6 7 8
            => twoDimensionalArray
                .Skip(squareStart.X)
                .Take(3)
                .SelectMany(row => row.Skip(squareStart.Y).Take(3))
                .ToImmutableArray();

        public static IEnumerable<(int Fila, int Columna, T Valor)> GetAllPositions<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray)
        {
            for (int i = 0; i < twoDimensionalArray.Length; i++)
                for (int j = 0; j < twoDimensionalArray.Length; j++)
                    yield return (i + 1, j + 1, twoDimensionalArray[i][j]);
        }

        public static ImmutableArray<ImmutableArray<T>> ForEachPosition<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, Action<int, int, T, Action<T>> action)
        {
            foreach (var (fila, columna, valor) in twoDimensionalArray.GetAllPositions()) {
                void setter(T newItem) => twoDimensionalArray = twoDimensionalArray.Set(fila, columna, newItem);
                action(fila, columna, valor, setter);
            }
            return twoDimensionalArray;
        }

        public static void ForEachPosition<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, Action<int, int, T> action)
            => twoDimensionalArray.ForEachPosition((i, j, item, setter) => action(i, j, item));

        public static string PrettyPrint<T>(this ImmutableArray<ImmutableArray<T>> twoDimensionalArray, Func<T, string> itemPrinter = null)
        {
            itemPrinter = itemPrinter ?? (x => x.ToString());
            var sb = new StringBuilder();
            For1To9(f => {
                sb.AppendLine();
                For1To9(c =>
                    sb.Append($"{itemPrinter(twoDimensionalArray.Get(f, c))}, "));
            });
            return sb.ToString(0, sb.Length - 2);
        }
    }

    public static class SudokuUtils
    {
        public static void For1To9(Action<int> action)
        {
            for (int i = 1; i <= 9; i++)
                action(i);
        }

        public static bool AreInSameSquare((int fila, int columna) posicio1, (int fila, int columna) posicio2)
        {
            //if (Math.Abs(posicio2.fila - posicio1.fila) >= 3 || Math.Abs(posicio2.columna - posicio1.columna) >= 3)
            //    return false;
            var squareStart = GetSquareStartIndexForPosition(posicio1.fila, posicio1.columna);
            return (posicio2.fila > squareStart.iFila && posicio2.columna > squareStart.iColumna && posicio2.fila <= squareStart.iFila + 3 && posicio2.columna <= squareStart.iColumna + 3);
        }

        public static (int iFila, int iColumna) GetSquareStartIndexForPosition(int fila, int columna)
            => ((fila - 1) / 3 * 3, (columna - 1) / 3 * 3);

        public static string PrintSequence(IEnumerable<int> sequence)
        {
            var count = sequence.Count();
            if (count == 0)
                return "{}";
            else if (count == 1)
                return $"{{{sequence.Single()}}}";
            else
                return "{" + sequence.Select(i => i.ToString()).Aggregate((i, j) => $"{i}, {j}") + "}";
        }
    }
}
