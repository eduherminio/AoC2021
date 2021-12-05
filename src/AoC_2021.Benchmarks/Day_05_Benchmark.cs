/*
 *  |                          Method |     Mean |    Error |   StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 |    Gen 2 | Allocated |
 *  |-------------------------------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|---------:|----------:|
 *  |                           Part1 | 22.43 ms | 0.443 ms | 0.844 ms |  1.00 |    0.00 | 1218.7500 | 1031.2500 | 781.2500 |     11 MB |
 *  |          Part1_Parallel_Foreach | 63.92 ms | 1.551 ms | 4.450 ms |  2.82 |    0.21 | 2222.2222 | 1111.1111 | 555.5556 |     13 MB |
 *  | Part1_Parallel_Foreach_Hardcore | 61.44 ms | 1.481 ms | 4.368 ms |  2.77 |    0.23 | 2375.0000 | 1250.0000 | 500.0000 |     14 MB |
 *
 *  |                          Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
 *  |-------------------------------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|----------:|----------:|
 *  |                           Part2 |  46.56 ms |  0.911 ms |  1.418 ms |  46.57 ms |  1.00 |    0.00 | 1636.3636 | 1272.7273 | 1000.0000 |     21 MB |
 *  |          Part2_Parallel_Foreach | 132.26 ms |  2.619 ms |  3.496 ms | 132.37 ms |  2.86 |    0.13 | 3500.0000 | 1500.0000 |  500.0000 |     23 MB |
 *  | Part2_Parallel_Foreach_Hardcore | 164.56 ms | 11.737 ms | 34.605 ms | 155.40 ms |  3.34 |    0.79 | 4250.0000 | 2000.0000 | 1000.0000 |     24 MB |
 *
 */

using AoC_2020.Benchmarks;
using BenchmarkDotNet.Attributes;

namespace AoC_2021.Benchmarks
{
    public class Day_05_Benchmark_Part1 : BaseDayBenchmark
    {
        private readonly Day_05 _problem = new();

        [Benchmark(Baseline = true)]
        public async Task<string> Part1() => await _problem.Solve_1();

        [Benchmark]
        public async Task<string> Part1_Parallel_Foreach() => await _problem.Solve_1_Parallel_ForEach();

        [Benchmark]
        public async Task<string> Part1_Parallel_Foreach_Hardcore() => await _problem.Solve_1_Parallel_ForEach_Hardcore();
    }

    public class Day_05_Benchmark_Part2 : BaseDayBenchmark
    {
        private readonly Day_05 _problem = new();

        [Benchmark(Baseline = true)]
        public async Task<string> Part2() => await _problem.Solve_2();

        [Benchmark]
        public async Task<string> Part2_Parallel_Foreach() => await _problem.Solve_2_Parallel_ForEach();

        [Benchmark]
        public async Task<string> Part2_Parallel_Foreach_Hardcore() => await _problem.Solve_2_Parallel_ForEach_Hardcore();
    }
}
