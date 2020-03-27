using System;
using System.Collections.Generic;

namespace SnakeAI
{
	internal class Field
	{
		public Random R;

		public List<List<Content>> Grid { get; set; }
		public List<Point> Food { get; set; }
		public int Width { get; }
		public int Height { get; }

		private Snake snake;

		public Snake Snake
		{
			get { return snake; }
			set
			{
				if (value != null && snake != null) throw new Exception();
				snake = value;
			}
		}

		public Field(int width, int height, int seed)
		{
			R = new Random(seed);
			Width = width;
			Height = height;
			Grid = new List<List<Content>>();
			for (int i = 0; i < height; i++)
			{
				Grid.Add(new List<Content>());
				for (int j = 0; j < width; j++)
				{
					Grid[i].Add(Content.EMPTY);
				}
			}
			Food = new List<Point>();
			PlaceFood();
		}

		public void PlaceFood()
		{
			var point = new Point(R.Next(Width), R.Next(Height));
			int count = 0;
			while (this[point] != Content.EMPTY)
			{
				if (++count > 1000) throw new Exception();
				point = new Point(R.Next(Width), R.Next(Height));
			}
			this[point] = Content.FOOD;
			Food.Add(point);
		}

		public void ReviewFood()
		{
			foreach (var food in Food)
			{
				if (this[food] != Content.FOOD)
				{
					this[food] = Content.FOOD;
				}
			}
		}

		public Point FindFood(Point snake)
		{
			if (Food.Count == 0) PlaceFood();
			var result = Food[0];
			var maxDistance = snake.Distance(Food[0]);
			for (int i = 1; i < Food.Count; i++)
			{
				var distance = snake.Distance(Food[i]);
				if (distance > maxDistance)
				{
					maxDistance = distance;
					result = Food[i];
				}
			}
			return result;
		}

		public void Eat(Point food)
		{
			this[food] = Content.SNAKE;
			Food.Remove(food);
			PlaceFood();
		}

		public Content this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width) return Content.WALL;
				else if (y < 0 || y >= Height) return Content.WALL;
				else return Grid[y][x];
			}
			set
			{
				Grid[y][x] = value;
			}
		}

		public Content this[Point p]
		{
			get => this[p.X, p.Y];
			set
			{
				this[p.X, p.Y] = value;
			}
		}
	}
}