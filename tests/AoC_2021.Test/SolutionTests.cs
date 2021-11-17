using AdventOfCode;
using AoCHelper;
using NUnit.Framework;

namespace AoC_2021.Test
{
    public static class SolutionTests
    {
        public class Solutions
        {
            [TestCase(typeof(Day_01), "Solution to Day 1, part 1", "Solution to Day 1, part 2")]
            public async Task Test(Type type, string sol1, string sol2)
            {
                if (Activator.CreateInstance(type) is BaseDay instance)
                {
                    Assert.AreEqual(sol1, await instance.Solve_1());
                    Assert.AreEqual(sol2, await instance.Solve_2());
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}
