using System.Drawing;

namespace SnakeAI
{
	internal class NeuronRepresentation
	{
		public Color Color
		{
			get
			{
				switch (Neuron.Type)
				{
					case NeuronType.INPUT: return Color.FromArgb(1, 147, 154);
					case NeuronType.HIDDEN: return Color.FromArgb(255, 171, 0);
					case NeuronType.OUTPUT: return Color.FromArgb(255, 7, 0);
					case NeuronType.MEMORY: return Color.FromArgb(1, 180, 57);
				}
				return Color.White;
			}
		}

		public PointF Position;
		public Neuron Neuron;

		public NeuronRepresentation(Neuron neuron, PointF position)
		{
			Neuron = neuron;
			Position = position;
		}
	}
}