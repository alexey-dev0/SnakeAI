namespace SnakeAI
{
	internal class Neuron
	{
		public NeuronType Type { get; }
		public double[] Weights { get; }
		public double Value { get; set; }

		public Neuron()
		{
			Value = 0;
			Type = NeuronType.INPUT;
		}

		public Neuron(int weights, double value, NeuronType type)
		{
			Value = value;
			Weights = new double[weights];
			Type = type;
			GenerateWeights(weights);
		}

		public Neuron(double[] weights, double value, NeuronType type)
		{
			Value = value;
			if (weights != null)
			{
				Weights = new double[weights.Length];
				weights.CopyTo(Weights, 0);
			}
			Type = type;
		}

		private void GenerateWeights(int weights)
		{
			for (int i = 0; i < weights; i++)
			{
				Weights[i] = NeuroNetwork.GetRandomWeight();
			}
		}

		public Neuron Copy()
		{
			return new Neuron(Weights, Value, Type);
		}
	}
}