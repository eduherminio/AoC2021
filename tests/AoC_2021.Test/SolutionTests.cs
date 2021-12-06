using AoCHelper;
using NUnit.Framework;

namespace AoC_2021.Test
{
    public static class SolutionTests
    {
        public class Solutions
        {
            [TestCase(typeof(Day_01), "1527", "1575")]
            [TestCase(typeof(Day_02), "1499229", "1340836560")]
            [TestCase(typeof(Day_03), "3687446", "4406844")]
            [TestCase(typeof(Day_04), "39984", "8468")]
            [TestCase(typeof(Day_05), "7438", "21406")]
            [TestCase(typeof(Day_06), "373378", "1682576647495")]
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
