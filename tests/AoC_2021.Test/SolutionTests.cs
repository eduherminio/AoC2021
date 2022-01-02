using AoCHelper;
using NUnit.Framework;

namespace AoC_2021.Test;

public class SolutionTests
{
    [TestCase(typeof(Day_01), "1527", "1575")]
    [TestCase(typeof(Day_02), "1499229", "1340836560")]
    [TestCase(typeof(Day_03), "3687446", "4406844")]
    [TestCase(typeof(Day_04), "39984", "8468")]
    [TestCase(typeof(Day_05), "7438", "21406")]
    [TestCase(typeof(Day_06), "373378", "1682576647495")]
    [TestCase(typeof(Day_07), "355989", "102245489")]
    [TestCase(typeof(Day_08), "440", "1046281")]
    [TestCase(typeof(Day_09), "423", "1198704")]
    [TestCase(typeof(Day_10), "394647", "2380061249")]
    [TestCase(typeof(Day_11), "1785", "354")]
    [TestCase(typeof(Day_12), "4691", "140718")]
    [TestCase(typeof(Day_13), "666", @"
.XX....XX.X..X..XX..XXXX.X..X.X..X.X..X
X..X....X.X..X.X..X....X.X..X.X.X..X..X
X.......X.XXXX.X..X...X..XXXX.XX...X..X
X.......X.X..X.XXXX..X...X..X.X.X..X..X
X..X.X..X.X..X.X..X.X....X..X.X.X..X..X
.XX...XX..X..X.X..X.XXXX.X..X.X..X..XX.
")]
    [TestCase(typeof(Day_14), "2937", "")]
    [TestCase(typeof(Day_15), "717", "2993")]
    [TestCase(typeof(Day_16), "953", "246225449979")]
    [TestCase(typeof(Day_17), "9180", "3767")]
    public async Task Test(Type type, string sol1, string sol2)
    {
        if (Activator.CreateInstance(type) is BaseProblem instance)
        {
            Assert.AreEqual(sol1, await instance.Solve_1());
            Assert.AreEqual(sol2, await instance.Solve_2());
        }
        else
        {
            Assert.Fail($"{type} is not a BaseDay");
        }
    }
}
