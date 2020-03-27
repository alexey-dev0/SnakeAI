namespace SnakeAI
{
	internal class Neuron
	{
		public NeuronType Type { get; }
		public double[] Weights { get; }
		public double Value { get; set; }

		private NeuroNetwork _neuroNetwork;

		public Neuron(NeuroNetwork neuroNetwork)
		{
			Value = 0;
			Type = NeuronType.INPUT;
			_neuroNetwork = neuroNetwork;
		}

		public Neuron(NeuroNetwork neuroNetwork, int weights, double value, NeuronType type)
		{
			Value = value;
			Weights = new double[weights];
			Type = type;
			_neuroNetwork = neuroNetwork;
			GenerateWeights(weights);
		}

		public Neuron(NeuroNetwork neuroNetwork, double[] weights, double value, NeuronType type)
		{
			Value = value;
			if (weights != null)
			{
				Weights = new double[weights.Length];
				weights.CopyTo(Weights, 0);
			}
			Type = type;
			_neuroNetwork = neuroNetwork;
		}

		private void GenerateWeights(int weights)
		{
			for (int i = 0; i < weights; i++)
			{
				Weights[i] = _neuroNetwork.GetRandomWeight();
			}
		}

		public Neuron Copy(NeuroNetwork neuroNetwork)
		{
			return new Neuron(neuroNetwork, Weights, Value, Type);
		}
	}
}