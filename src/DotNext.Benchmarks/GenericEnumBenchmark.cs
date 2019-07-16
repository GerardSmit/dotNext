﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using System;

namespace DotNext
{
    [SimpleJob(runStrategy: RunStrategy.Throughput, launchCount: 1)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class GenericEnumBenchmark
    {
        private static int ToInt32<T>(T value)
            where T : struct, IConvertible
            => value.ToInt32(null);

        private static long ToInt64<T>(T value)
            where T : struct, IConvertible
            => value.ToInt64(null);

        private static T ToEnum<T>(int value)
            where T : unmanaged, Enum
            => value.Bitcast<int, T>();

        [Benchmark]
        public void ToInt32UsingContrainedCall()
        {
            ToInt32(EnvironmentVariableTarget.Machine);
        }

        [Benchmark]
        public void ToInt64UsingConstrainedCall()
        {
            ToInt64(EnvironmentVariableTarget.Machine);
        }

        [Benchmark]
        public void ToInt32UsingGenericConverter()
        {
            EnvironmentVariableTarget.Machine.ToInt32();
        }

        [Benchmark]
        public void ToInt64UsingGenericConverter()
        {
            EnvironmentVariableTarget.Machine.ToInt64();
        }

        [Benchmark]
        public void ToEnumUsingBitcast()
        {
            ToEnum<EnvironmentVariableTarget>(2);
        }

        [Benchmark]
        public void ToEnumUsingGenericConverter()
        {
            2.ToEnum<EnvironmentVariableTarget>();
        }
    }
}