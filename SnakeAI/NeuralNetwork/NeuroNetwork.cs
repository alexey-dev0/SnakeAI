using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SnakeAI
{
	internal class NeuroNetwork
	{
		public Random R;
		public static int ID = 0;

		public Color Color { get; set; }
		public List<Neuron> Inputs { get; }
		public List<List<Neuron>> HiddenLayers { get; }
		public List<Neuron> Outputs { get; }
		public List<Neuron> Memory { get; }
		public int Id = ++ID;

		private Snake snake;

		public Snake Snake
		{
			get
			{
				return snake;
			}
			set
			{
				if (snake != null) throw new Exception();
				snake = value;
			}
		}

		public NeuroNetwork(int inputs, List<int> hiddenLayers, int outputs, int memory = 0, bool generate = true)
		{
			R = new Random(Id);
			Inputs = new List<Neuron>();
			HiddenLayers = new List<List<Neuron>>();
			for (int i = 0; i < hiddenLayers.Count; i++)
				HiddenLayers.Add(new List<Neuron>());
			Outputs = new List<Neuron>();
			Memory = new List<Neuron>();

			if (generate) GenerateRand(inputs, hiddenLayers, outputs, memory);
			Color = Color.FromArgb(R.Next(256), R.Next(256), R.Next(256));
		}

		private void GenerateRand(int inputs, List<int> hiddenLayers, int outputs, int memory)
		{
			for (int i = 0; i < inputs; i++)
				Inputs.Add(new Neuron(this));
			for (int i = 0; i < memory; i++)
				Memory.Add(new Neuron(this, hiddenLayers[hiddenLayers.Count - 1], 0, NeuronType.MEMORY));
			for (int i = 0; i < hiddenLayers.Count; i++)
			{
				for (int j = 0; j < hiddenLayers[i]; j++)
				{
					var value = GetRandomWeight();
					var prevLayer = i == 0 ? Inputs.Concat(Memory).ToList() : HiddenLayers[i - 1];
					HiddenLayers[i].Add(new Neuron(this, prevLayer.Count, value, NeuronType.HIDDEN));
				}
			}
			for (int i = 0; i < outputs; i++)
				Outputs.Add(new Neuron(this, hiddenLayers[hiddenLayers.Count - 1], 0, NeuronType.OUTPUT));
		}

		public double GetRandomWeight()
		{
			return R.NextDouble() * 2 - 1;
		}

		private void ProcessLayer(List<Neuron> prevLayer, List<Neuron> nextLayer, Func<double, double> activateFunction)
		{
			foreach (var neuron in nextLayer)
			{
				double sum = 0.0;
				for (int i = 0; i < prevLayer.Count; i++)
				{
					sum += prevLayer[i].Value * neuron.Weights[i];
				}
				neuron.Value = activateFunction(sum);
			}
		}

		private double HiddenFunc(double value)
		{
			return value > 0.5 ? 1 : 0;
		}

		private double OutputFunc(double value)
		{
			return value > 0 ? 1 : 0;
		}

		public void Cross(NeuroNetwork other)
		{
			for (int i = 0; i < HiddenLayers.Count; i++)
			{
				for (int j = 0; j < HiddenLayers[i].Count; j++)
				{
					if (R.NextDouble() > 0.5) HiddenLayers[i][j] = other.HiddenLayers[i][j].Copy(this);
				}
			}
			for (int j = 0; j < Outputs.Count; j++)
			{
				if (R.NextDouble() > 0.5) Outputs[j] = other.Outputs[j].Copy(this);
			}
		}

		public void Update(List<double> inputs)
		{
			for (int i = 0; i < inputs.Count; i++)
			{
				Inputs[i].Value = inputs[i];
			}
			ProcessLayer(Inputs.Concat(Memory).ToList(), HiddenLayers[0], HiddenFunc);
			for (int i = 1; i < HiddenLayers.Count; i++)
			{
				ProcessLayer(HiddenLayers[i - 1], HiddenLayers[i], HiddenFunc);
			}
			ProcessLayer(HiddenLayers[HiddenLayers.Count - 1], Outputs.Concat(Memory).ToList(), HiddenFunc);
		}

		public NeuroNetwork Copy()
		{
			var hidden = new List<int>();
			for (int i = 0; i < HiddenLayers.Count; i++)
				hidden.Add(HiddenLayers[i].Count);
			var result = new NeuroNetwork(Inputs.Count, hidden, Outputs.Count, Memory.Count, false);
			for (int i = 0; i < Inputs.Count; i++)
			{
				result.Inputs.Add(Inputs[i].Copy(result));
			}
			for (int i = 0; i < Memory.Count; i++)
			{
				result.Memory.Add(Memory[i].Copy(result));
			}
			for (int i = 0; i < hidden.Count; i++)
			{
				for (int j = 0; j < hidden[i]; j++)
				{
					result.HiddenLayers[i].Add(HiddenLayers[i][j].Copy(result));
				}
			}
			for (int i = 0; i < Outputs.Count; i++)
			{
				result.Outputs.Add(Outputs[i].Copy(result));
			}
			result.Color = Color;
			return result;
		}

		public void NeuroShake(double rate)
		{
			for (int i = 0; i < HiddenLayers.Count; i++)
			{
				for (int j = 0; j < HiddenLayers[i].Count; j++)
				{
					for (int w = 0; w < HiddenLayers[i][j].Weights.Length; w++)
					{
						if (R.NextDouble() < rate)
						{
							HiddenLayers[i][j].Weights[w] = GetRandomWeight();
						}
					}
				}
			}

			for (int i = 0; i < Outputs.Count; i++)
			{
				for (int w = 0; w < Outputs[i].Weights.Length; w++)
				{
					if (R.NextDouble() < rate)
					{
						Outputs[i].Weights[w] = GetRandomWeight();
					}
				}
			}

			for (int i = 0; i < Memory.Count; i++)
			{
				for (int w = 0; w < Memory[i].Weights.Length; w++)
				{
					if (R.NextDouble() < rate)
					{
						Memory[i].Weights[w] = GetRandomWeight();
					}
				}
			}

			/*for (int i = 0; i < 10 * rate; i++)
            {
                if (R.NextDouble() > 0.3)
                {
                    var j = R.Next(HiddenLayers.Count);
                    var k = R.Next(HiddenLayers[j].Count);
                    var prevLayer = j == 0 ? Inputs : HiddenLayers[j - 1];
                    var l = R.Next(prevLayer.Count);
                    HiddenLayers[j][k].Weights[l] = GetRandomWeight();
                }
                else if (R.NextDouble() > 0.5)
                {
                    var j = R.Next(Memory.Count);
                    var k = R.Next(HiddenLayers[HiddenLayers.Count - 1].Count);
                    Memory[j].Weights[k] = GetRandomWeight();
                }
                else
                {
                    var j = R.Next(Outputs.Count);
                    var k = R.Next(HiddenLayers[HiddenLayers.Count - 1].Count);
                    Outputs[j].Weights[k] = GetRandomWeight();
                }
            }*/
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			for (int i = 0; i < Inputs.Count; i++)
			{
				sb.Append($"{Inputs[i].Value:F2}  ");
			}
			sb.Append("\n");
			for (int j = 0; j < HiddenLayers.Count; j++)
			{
				for (int i = 0; i < HiddenLayers[j].Count; i++)
				{
					sb.Append($"{HiddenLayers[j][i].Value:F2}  ");
				}
				sb.Append("\n");
			}
			for (int i = 0; i < Outputs.Count; i++)
			{
				sb.Append($"{Outputs[i].Value:F2}  ");
			}
			sb.Append("\n");
			return sb.ToString();
		}
	}
}