using System;
using System.Threading;

namespace SnakeAI
{
	public static class StaticRandom
	{
		private static int seed = Environment.TickCount;

		private static readonly ThreadLocal<Random> random =
			new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

		public static int Rand()
		{
			return random.Value.Next();
		}
	}
}