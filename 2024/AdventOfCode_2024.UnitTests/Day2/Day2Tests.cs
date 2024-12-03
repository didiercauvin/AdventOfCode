using System.Runtime.CompilerServices;
using static AdventOfCode_2024.UnitTests.Day2.InputInfo;

namespace AdventOfCode_2024.UnitTests.Day2;

[TestClass]
public class Day2Tests
{
    [TestMethod]
    public void OneLineOfLevelsOrdered_Safe()
    {
        int nbSafe = Reporter.HowManySafe("1");

        Assert.AreEqual(1, nbSafe);
    }

    [TestMethod]
    public void OneLineOfLevelsNotOrdered_Unsafe()
    {
        int nbSafe = Reporter.HowManySafe("37 35 36 37 34");

        Assert.AreEqual(0, nbSafe);
    }

    [TestMethod]
    public void LineOrderedButNotIncreasingNorDecreasing_Unsafe()
    {
        int nbSafe = Reporter.HowManySafe("2 2");

        Assert.AreEqual(0, nbSafe);
    }

    [TestMethod]
    public void LineOrderedButTooMuchDecreasing_Unsafe()
    {
        int nbSafe = Reporter.HowManySafe("6 2");

        Assert.AreEqual(0, nbSafe);
    }

    [TestMethod]
    public void LineOrderedButTooMuchIncreasing_Unsafe()
    {
        int nbSafe = Reporter.HowManySafe("2 6");

        Assert.AreEqual(0, nbSafe);
    }

    [DataRow("7 6 4 2 1")]
    [DataTestMethod]
    public void ManyLinesOfLevelsWithTwoOrdered_TwoAreSafe(string input)
    {
        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(1, nbSafe);
    }

    [DataRow("1 2 7 8 9")]
    [DataTestMethod]
    public void IncreaseOf5_Unsafe(string input)
    {
        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(0, nbSafe);
    }

    [DataRow("9 7 6 2 1")]
    [DataTestMethod]
    public void DecreasOf4_Unsafe(string input)
    {
        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(0, nbSafe);
    }

    [DataRow("1 3 2 4 5")]
    [DataTestMethod]
    public void NotOrdered_Unsafe(string input)
    {
        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(0, nbSafe);
    }

    [DataRow("8 6 4 4 1")]
    [DataTestMethod]
    public void NorIncreaseOrDecrease_Unsafe(string input)
    {
        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(0, nbSafe);
    }

    [DataRow("1 3 6 7 9")]
    [DataTestMethod]
    public void AllIncreasing_Safe(string input)
    {
        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(1, nbSafe);
    }

    [TestMethod]
    public void ManyLinesOfLevelsWithTwoOrdered_TwoAreSafe()
    {
        var input = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

        int nbSafe = Reporter.HowManySafe(input);

        Assert.AreEqual(2, nbSafe);
    }

    [TestMethod]
    public void Part1_FindHowManySafeFromInput()
    {
        int nbSafe = Reporter.HowManySafe(Input);

        Assert.AreEqual(479, nbSafe);
    }

    [TestMethod]
    public void Part2_HandleTwoErrorsFromInput()
    {
        int nbSafe = Reporter.HowManySafe("69 72 73 76 77 74 75 75", true);

        Assert.AreEqual(0, nbSafe);
    }

    [DataTestMethod]
    public void Part2_SafeByRemovingSecondLevel()
    {
        var input = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

        int nbSafe = Reporter.HowManySafe(input, true);

        Assert.AreEqual(4, nbSafe);
    }

    [TestMethod]
    public void Part2_FindHowManySafeFromInputWithDampener()
    {
        int nbSafe = Reporter.HowManySafe(Input, true);

        Assert.AreEqual(567, nbSafe);
    }
}

public static class Reporter
{
    public static int HowManySafe(string input, bool applyDampener = false)
    {
        var levels = input.Split("\n").Select(l => l.Split(" ").Select(s => int.Parse(s)).ToArray()).ToArray();

        return HowManySafe(levels, applyDampener);
    }

    private static int HowManySafe(int[][] levels, bool applyDampener)
    {
        var nbSafe = 0;

        foreach (var line in levels)
        {
            if (IsLineSafe(line, applyDampener)) nbSafe++;
        }

        return nbSafe;
    }

    private static bool IsLineSafe(int[] line, bool applyDampener)
    {
        var lineSafe = true;

        for (var i = 0; i < line.Length; i++)
        {
            var asc = line.OrderBy(x => x).ToArray();
            var desc = asc.OrderByDescending(x => x).ToArray();

            if ((!line.SequenceEqual(asc) && !line.SequenceEqual(desc)) || IsLineUnsafe(line, i))
            {
                lineSafe = false;

                if (applyDampener)
                {
                    lineSafe = !IsLineUnsafeDampered(line.Skip(1).ToArray(), line, 0);
                }
            }
        }

        return lineSafe;
    }

    private static bool IsLineUnsafeDampered(int[] line, int[] refLines, int indexToSkip)
    {
        var isLineUnsafe = true;

        var asc = line.OrderBy(x => x).ToArray();
        var desc = asc.OrderByDescending(x => x).ToArray();

        if (indexToSkip < line.Length - 1)
        {
            for(int j = 0; j < line.Length; j++)
            {
                isLineUnsafe = (!line.SequenceEqual(asc) && !line.SequenceEqual(desc)) || IsLineUnsafe(line, j);

                if (isLineUnsafe)
                {
                    indexToSkip++;
                    return IsLineUnsafeDampered(refLines.Where((_, index) => index != indexToSkip).ToArray(), refLines, indexToSkip);
                }
            }

            isLineUnsafe = false;
        }

        return isLineUnsafe;
    }

    private static bool IsLineUnsafe(int[] line, int i)
    {
        var nextIndex = i + 1;

        return 
            (nextIndex < line.Length && (line[i] == line[nextIndex] ||
            Math.Abs(line[i] - line[nextIndex]) < 1 || Math.Abs(line[i] - line[nextIndex]) > 3));
    }
}
