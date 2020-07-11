using NUnit.Framework;
using System;

namespace SudokuSolver.Tests
{
    class Sudokus
    {
        public static Sudoku Facil() 
            => new Sudoku(
                7, 0, 0, 0, 1, 5, 9, 3, 0,
                0, 5, 4, 0, 0, 0, 7, 0, 0,
                9, 3, 6, 8, 7, 4, 2, 5, 0,
                0, 6, 5, 2, 8, 0, 3, 9, 7,
                3, 0, 0, 0, 0, 0, 0, 1, 0,
                0, 0, 1, 9, 0, 3, 4, 0, 0,
                0, 4, 9, 5, 0, 8, 1, 0, 3,
                5, 8, 7, 0, 0, 9, 6, 4, 2,
                2, 1, 3, 0, 6, 7, 5, 8, 9);

        public static Sudoku FacilSolucionat()
            => new Sudoku(
                7, 2, 8, 6, 1, 5, 9, 3, 4,
                1, 5, 4, 3, 9, 2, 7, 6, 8,
                9, 3, 6, 8, 7, 4, 2, 5, 1,
                4, 6, 5, 2, 8, 1, 3, 9, 7,
                3, 9, 2, 7, 4, 6, 8, 1, 5,
                8, 7, 1, 9, 5, 3, 4, 2, 6,
                6, 4, 9, 5, 2, 8, 1, 7, 3,
                5, 8, 7, 1, 3, 9, 6, 4, 2,
                2, 1, 3, 4, 6, 7, 5, 8, 9);

        public static void TestSudokuFacil(Func<Sudoku, Sudoku> solver)
            => Assert.That(solver(Facil()), Is.EqualTo(FacilSolucionat()));
    }
}
