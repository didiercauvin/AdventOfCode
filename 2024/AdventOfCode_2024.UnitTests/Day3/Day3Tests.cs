using static AdventOfCode_2024.UnitTests.Day3.InputInfo;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace AdventOfCode_2024.UnitTests.Day3;

[TestClass]
public class Day3Tests
{
    [DataRow("mul(2,4)", 8)]
    [DataRow("mul(2,2)", 4)]
    [DataTestMethod]
    public void OneMulAtATime(string input, int expected)
    {
        int result = Multiplicator.Part1(input);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void MultipleMul()
    {
        int result = Multiplicator.Part1("mul(2,4)mul(2,2)");

        Assert.AreEqual(12, result);
    }

    [TestMethod]
    public void MalFormattedMustNotProduceSomething()
    {
        int result = Multiplicator.Part1("mul ( 2 , 4 )");
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void CombinationOfWellAndMalFormattedMustProduceResult()
    {
        int result = Multiplicator.Part1("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))");
        Assert.AreEqual(161, result);
    }

    [TestMethod]
    public void PART1_Resolver()
    {
        int result = Multiplicator.Part1(Input);
        Assert.AreEqual(159833790, result);
    }

    [TestMethod]
    public void PART2_ExcludeDontAndKeepDo()
    {
        int result = Multiplicator.Part2(@"xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))");
        Assert.AreEqual(48, result);
    }

    [TestMethod]
    public void PART2_Resolver()
    {
        int result = Multiplicator.Part2(Input);
        Assert.AreEqual(89349241, result);
    }
}

public static class Multiplicator
{
    public static int Part1(string input)
    {
        return Times(input);
    }

    public static int Part2(string input)
    {
        return Times(Clean(input));
    }

    private static string Clean(string input)
    {
        var skip = false;
        var input_cleaned = "";

        foreach (var match in Regex.Matches(input, @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)").Cast<Match>())
        {
            skip = ShouldBeSkipped(skip, match);

            if (!skip)
            {
                input_cleaned += $"{match.Groups[0]}";
            }
        }

        return input_cleaned;
    }

    private static bool ShouldBeSkipped(bool skip, Match match)
    {
        if (IsDont(match))
        {
            skip = true;
        }

        if (IsDo(match))
        {
            skip = false;
        }

        return skip;
    }

    private static bool IsDo(Match match)
    {
        return match.Groups[0].Value == "do()";
    }

    private static bool IsDont(Match match)
    {
        return match.Groups[0].Value == "don't()";
    }

    public static int Times(string input)
    {
        var matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)");
        var result = 0;

        foreach (var match in matches.Cast<Match>())
        {
            var left = int.Parse(match.Groups[1].Value);
            var right = int.Parse(match.Groups[2].Value);

            result += left * right;
        }

        return result;
    }
}
