using AoCHelper;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AoC_2021.Test
{
    public static class SolutionTests
    {
        public class Solutions
        {
            [TestCase(typeof(Day_01), "988771", "171933104")]
            public async Task Test(Type type, string sol1, string sol2)
            {
                var instance = Activator.CreateInstance(type) as BaseDay;

                Assert.AreEqual(sol1, await instance.Solve_1());
                Assert.AreEqual(sol2, await instance.Solve_2());
            }
        }
    }
}
