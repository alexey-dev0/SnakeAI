using System.Collections.Generic;

namespace SnakeAI
{
	internal class Snake
	{
		public int LifeTime { get; set; }
		public int Score { get; set; }
		public bool Dead { get; set; }
		private int energy;

		public int Energy
		{
			get { return energy; }
			set
			{
				energy = value;
				if (energy <= 0) Death();
			}
		}

		public NeuroNetwork NeuroNetwork { get; }
		public Field Field { get; }
		public int Fitness => Score * Score * LifeTime + LifeTime;

		private List<double> lastTurns { get; set; }
		private List<Point> body { get; set; }
		private Point head => body[0];

		private int rotation;

		private int Rotation
		{
			get { return rotation; }
			set { rotation = (value + 4) % 4; }
		}

		public Snake(Field field, NeuroNetwork neuroNetwork)
		{
			Field = field;
			field.Snake = this;

			NeuroNetwork = neuroNetwork;
			neuroNetwork.Snake = this;

			body = new List<Point>();
			Energy = 50;
			Rotation = NeuroNetwork.R.Next(4);
			lastTurns = new List<double> { 0, 0, 0 };

			Initialize();
		}

		private void Initialize()
		{
			var point = new Point(NeuroNetwork.R.Next(1, Field.Width - 1), NeuroNetwork.R.Next(1, Field.Height - 1));
			while (Field[point] != Content.EMPTY)
			{
				point = new Point(NeuroNetwork.R.Next(1, Field.Width - 1), NeuroNetwork.R.Next(1, Field.Height - 1));
			}
			Field[point] = Content.SNAKE;
			body.Add(point);
		}

		private List<double> GetInputs()
		{
			var lSnake = 0;
			var delta = GetDeltaRotation((Rotation - 1 + 4) % 4);
			var testHead = new Point(head);
			testHead += delta;
			var count = 1;
			while (Field[testHead] != Content.WALL)
			{
				if (Field[testHead] == Content.SNAKE && lSnake == 0)
					lSnake = count;
				testHead += delta;
				count++;
			}
			var lWall = count;

			var sSnake = 0;
			delta = GetDeltaRotation();
			testHead = new Point(head);
			testHead += delta;
			count = 1;
			while (Field[testHead] != Content.WALL)
			{
				if (Field[testHead] == Content.SNAKE && sSnake == 0)
					sSnake = count;
				testHead += delta;
				count++;
			}
			var sWall = count;

			var rSnake = 0;
			delta = GetDeltaRotation((Rotation + 1 + 4) % 4);
			testHead = new Point(head);
			testHead += delta;
			count = 1;
			while (Field[testHead] != Content.WALL)
			{
				if (Field[testHead] == Content.SNAKE && rSnake == 0)
					rSnake = count;
				testHead += delta;
				count++;
			}
			var rWall = count;

			var food = Field.FindFood(head);
			double horFood = food.X - head.X;
			double vertFood = food.Y - head.Y;
			double xRot = Rotation == 0 ? 1 : Rotation == 2 ? -1 : 0;
			double yRot = Rotation == 1 ? 1 : Rotation == 3 ? -1 : 0;

			return new List<double>() {
				lWall, sWall, rWall,
				lSnake, sSnake, rSnake,
				horFood, vertFood,
				xRot, yRot
			};
		}

		private List<double> GetInputsNew()
		{
			double lSnake = 0, sSnake = 0, rSnake = 0;

			var delta = GetDeltaRotation((Rotation - 1 + 4) % 4);
			var testHead = new Point(head);
			testHead += delta;
			var count = 1;
			while (Field[testHead] != Content.WALL)
			{
				if (Field[testHead] == Content.SNAKE && rSnake == 0)
				{
					rSnake = count;
					break;
				}
				testHead += delta;
				count++;
			}

			delta = GetDeltaRotation();
			testHead = new Point(head);
			testHead += delta;
			count = 1;
			while (Field[testHead] != Content.WALL)
			{
				if (Field[testHead] == Content.SNAKE && sSnake == 0)
				{
					sSnake = count;
					break;
				}
				testHead += delta;
				count++;
			}

			delta = GetDeltaRotation((Rotation + 1) % 4);
			testHead = new Point(head);
			testHead += delta;
			count = 1;
			while (Field[testHead] != Content.WALL)
			{
				if (Field[testHead] == Content.SNAKE && lSnake == 0)
				{
					lSnake = count;
					break;
				}
				testHead += delta;
				count++;
			}

			var food = Field.FindFood(head);
			double horFood = food.X - head.X;
			double vertFood = food.Y - head.Y;
			if (Rotation % 2 == 0)
			{
				var temp = horFood;
				horFood = vertFood;
				vertFood = temp;
			}
			if (Rotation == 0) vertFood = -vertFood;
			else if (Rotation == 1)
			{
				horFood = -horFood;
				vertFood = -vertFood;
			}
			else if (Rotation == 2) horFood = -horFood;

			var result = new List<double>()
			{
				horFood == 0.0 ? 1.0 : 1.0 / horFood,
				vertFood == 0.0 ? 1.0 : 1.0 / vertFood,
				lSnake == 0.0 ? 0.0 : 1.0 / lSnake,
				rSnake == 0.0 ? 0.0 : 1.0 / rSnake,
				sSnake == 0.0 ? 0.0 : 1.0 / sSnake,
				Score == 0 ? 1.0 : 1.0 / Score
			};
			result.AddRange(lastTurns);
			var rotStraight = GetDeltaRotation();
			var rotLeft = GetDeltaRotation((Rotation - 1 + 4) % 4);
			var rotRight = -rotLeft;
			var rotBack = -rotStraight;
			var rotLS = rotStraight + rotLeft;
			var corner = head + rotLS * 2;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (i == 2 && j == 1) continue;
					var cell = Field[corner + rotRight * i + rotBack * j];
					result.Add(cell == Content.WALL || cell == Content.SNAKE ? -1 : cell == Content.FOOD ? 1 : 0);
				}
			}
			/*for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 2 && j == 1) continue;
                    var cell = Field[corner + rotRight * i + rotBack * j];
                    result.Add(cell == Content.SNAKE ? 1 : 0);
                }
            }*/
			return result;
		}

		private Point GetDeltaRotation(int rotation)
		{
			int dx = 0;
			int dy = 0;
			switch (rotation)
			{
				case 0: dx++; break;
				case 1: dy++; break;
				case 2: dx--; break;
				case 3: dy--; break;
			}
			return new Point(dx, dy);
		}

		private Point GetDeltaRotation()
		{
			return GetDeltaRotation(Rotation);
		}

		private void Rotate(int dr)
		{
			Rotation += dr;
			Energy--;
		}

		private void Move()
		{
			if (Dead) return;
			var deltaRotation = GetDeltaRotation();
			var newHead = new Point(head);
			newHead += deltaRotation;
			body.Insert(0, newHead);

			if (Field[head] == Content.WALL || Field[head] == Content.SNAKE)
			{
				body.Remove(head);
				Death();
				return;
			}
			else if (Field[head] != Content.FOOD)
			{
				var last = body[body.Count - 1];
				body.Remove(last);
				Field[last] = Content.EMPTY;
				Field[head] = Content.SNAKE;
			}
			else
			{
				Field.Eat(head);
				Energy += 50;
				Score++;
			}
			LifeTime++;
			Energy--;
		}

		private void Death()
		{
			Dead = true;
			foreach (var part in body)
				Field[part] = Content.EMPTY;
			Field.Snake = null;
		}

		public void Update()
		{
			if (Dead) return;
			NeuroNetwork.Update(GetInputsNew());
			var rotRight = NeuroNetwork.Outputs[0].Value;
			var rotLeft = NeuroNetwork.Outputs[1].Value;
			var turn = 0;
			if (rotRight > 0.4)
			{
				Rotate(1);
				turn = 1;
			}
			if (rotLeft > 0.4)
			{
				Rotate(-1);
				turn = -1;
			}
			for (int i = 1; i < lastTurns.Count; i++)
				lastTurns[i] = lastTurns[i - 1];
			lastTurns[0] = turn;
			Move();
		}
	}

	internal class SnakeComparer : IComparer<Snake>
	{
		public int Compare(Snake a, Snake b)
		{
			var aScore = a.Fitness;
			if (a.Energy == 0) aScore -= 100;
			var bScore = b.Fitness;
			if (b.Energy == 0) bScore -= 100;
			return aScore.CompareTo(bScore);
		}
	}
}