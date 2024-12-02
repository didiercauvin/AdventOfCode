using static AdventOfCode_2024.UnitTests.Day1.InputInfo;

namespace AdventOfCode_2024.UnitTests.Day1;

[TestClass]
public sealed class Day1Tests
{
    [TestMethod]
    public void Part1()
    {
        var totalDistance = CalculateDistance.FindTotalDistance(Input);

        Assert.AreEqual(3714264, totalDistance);
    }

    [TestMethod]
    public void Part2_LeftNumberNotFound_SimilarityOf0()
    {
        var input = @"1   0";

        var similarity = CalculateDistance.FindSimilarity(input);

        Assert.AreEqual(0, similarity);
    }

    [TestMethod]
    public void Part2_LeftNumberFoundXTime_SimilarityMultipliedByX()
    {
        var input = @"3   4
4   3
2   5
1   3
3   9
3   3";

        var similarity = CalculateDistance.FindSimilarity(input);

        Assert.AreEqual(31, similarity);
    }

    [TestMethod]
    public void Part2_FindSimilarityFromInputData()
    {
        var similarity = CalculateDistance.FindSimilarity(Input);

        Assert.AreEqual(18805872, similarity);
    }
}

public static class CalculateDistance
{
    public static int FindTotalDistance(string input)
    {
        var (left, right) = Parse(input);

        return FindTotalDistance(left.ToArray(), right.ToArray());
    }

    public static int FindSimilarity(string input)
    {
        var (left, right) = Parse(input);

        return FindSimilarity(left.ToArray(), right.ToArray());
    }

    private static (int[] left, int[] right) Parse(string input)
    {
        var lines = input.Split("\n");
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in lines)
        {
            var locationIds = line.Split("   ");
            left.Add(int.Parse(locationIds[0]));
            right.Add(int.Parse(locationIds[1]));
        }

        return (left.ToArray(), right.ToArray());
    }

    private static int FindSimilarity(int[] left, int[] right)
    {
        var similariry = 0;

        for (var i = 0; i < left.Length; i++)
        {
            var nbAppearance = right.Count(x => left[i] == x);
            similariry += left[i] * nbAppearance;
        }

        return similariry;
    }

    private static int FindTotalDistance(int[] left, int[] right)
    {
        var distance = 0;
        var orderedLeft = left.OrderBy(x => x).ToArray();
        var orderedRight = right.OrderBy(x => x).ToArray();

        for (var i = 0; i < orderedLeft.Count(); i++)
        {
            distance += FindDistance(orderedLeft[i], orderedRight[i]);
        }

        return distance;
    }

    private static int FindDistance(int left, int right)
    {
        return Math.Abs(left - right);
    }
}