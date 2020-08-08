using SudokuSolver;
using NUnit.Framework;

using static SudokuSolver.SudokuUtils;
using System.Linq;
using System.Collections.Immutable;

namespace SudokuSolver.Tests
{
    [TestFixture()]
    public class SudokuTests
    {
        [Test]
        public void PossibilitiesAreUpdated()
        {
            _ = Sudokus.Easy.Sudoku.Possibilities; //To force calculation of possibilities;
            Assert.That(Sudokus.Easy.Sudoku.Set(1, 2, 2).Possibilities.PrettyPrint(PrintSequence), Is.EqualTo(
@"
{}, {}, {8}, {6}, {}, {}, {}, {}, {4, 6, 8}, 
{1, 8}, {}, {}, {3, 6}, {2, 3, 9}, {2, 6}, {}, {6}, {1, 6, 8}, 
{}, {}, {}, {}, {}, {}, {}, {}, {1}, 
{4}, {}, {}, {}, {}, {1}, {}, {}, {}, 
{}, {7, 9}, {2, 8}, {4, 6, 7}, {4, 5}, {6}, {8}, {}, {5, 6, 8}, 
{8}, {7}, {}, {}, {5}, {}, {}, {2, 6}, {5, 6, 8}, 
{6}, {}, {}, {}, {2}, {}, {}, {7}, {}, 
{}, {}, {}, {1, 3}, {3}, {}, {}, {}, {}, 
{}, {}, {}, {4}, {}, {}, {}, {}, {}"));

            Assert.That(Sudokus.Easy.Sudoku.Set(1, 3, 8).Possibilities.PrettyPrint(PrintSequence), Is.EqualTo(
@"
{}, {2}, {}, {6}, {}, {}, {}, {}, {4, 6}, 
{1}, {}, {}, {3, 6}, {2, 3, 9}, {2, 6}, {}, {6}, {1, 6, 8}, 
{}, {}, {}, {}, {}, {}, {}, {}, {1}, 
{4}, {}, {}, {}, {}, {1}, {}, {}, {}, 
{}, {2, 7, 9}, {2}, {4, 6, 7}, {4, 5}, {6}, {8}, {}, {5, 6, 8}, 
{8}, {2, 7}, {}, {}, {5}, {}, {}, {2, 6}, {5, 6, 8}, 
{6}, {}, {}, {}, {2}, {}, {}, {7}, {}, 
{}, {}, {}, {1, 3}, {3}, {}, {}, {}, {}, 
{}, {}, {}, {4}, {}, {}, {}, {}, {}"));
        }

        [Test()]
        public void AreInSameSquareTest()
        {
            Assert.That(AreInSameSquare((4, 4), (6, 6)));
        }

        [Test]
        public void CheckTest()
        {
            var sudoku = Sudokus.Easy.Sudoku;

            Assert.That(sudoku.Check(3, 9, 1));
            Assert.That(!sudoku.Check(3, 9, 2));

            Assert.That(sudoku.Check(5, 7, 8));
            Assert.That(!sudoku.Check(5, 7, 7));

            Assert.That(sudoku.Check(7, 8, 7));
            Assert.That(!sudoku.Check(7, 8, 6));
        }

        [Test]
        public void SudokuPart()
        {
            var sudoku = Sudokus.Easy.Sudoku;

            Assert.That(sudoku.Row(3), Is.EqualTo(new[] { 9, 3, 6, 8, 7, 4, 2, 5, 0 }));
            Assert.That(sudoku.Column(2), Is.EqualTo(new[] { 0, 5, 3, 6, 0, 0, 4, 8, 1 }));
            Assert.That(sudoku.Square(2), Is.EqualTo(new[] { 0, 1, 5, 0, 0, 0, 8, 7, 4 }));
        }

        [Test]
        public void ForEachPositionTest()
        {
            var originalArray = Enumerable.Range(0, 3).Select(_ => Enumerable.Range(0, 3).Select(__ => 1).ToImmutableArray()).ToImmutableArray();
            var modifiedArray = originalArray.ForEachPosition((_, __, ___, setter) => setter(2));
            Assert.That(modifiedArray.All(x => x.All(y => y == 2)));
        }

    }
}
