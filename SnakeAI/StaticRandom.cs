using System;
using System.Threading;

namespace SnakeAI
{
	public static class StaticRandom
	{
		private static int seed = Environment.TickCount;

		private static readonly ThreadLocal<Random> random =
			new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

		public static int Next()
		{
			return random.Value.Next();
		}

		public static int Next(int max)
		{
			return random.Value.Next(max);
		}

		public static int Next(int min, int max)
		{
			return random.Value.Next(min, max);
		}

		public static double NextDouble()
		{
			return random.Value.NextDouble();
		}
	}
}