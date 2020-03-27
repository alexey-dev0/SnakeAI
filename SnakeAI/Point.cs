using System;

namespace SnakeAI
{
	internal struct Point
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point(Point other)
		{
			X = other.X;
			Y = other.Y;
		}

		public static Point operator +(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static Point operator -(Point a)
		{
			return new Point(-a.X, -a.Y);
		}

		public static Point operator -(Point a, Point b)
		{
			return a + -b;
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				Point p = (Point)obj;
				return (X == p.X) && (Y == p.Y);
			}
		}

		public static Point operator *(Point a, int r)
		{
			return new Point(a.X * r, a.Y * r);
		}

		public double Distance(Point other)
		{
			return Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2);
		}
	}
}