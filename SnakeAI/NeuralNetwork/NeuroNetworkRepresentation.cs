using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeAI
{
	internal class NeuroNetworkRepresentation
	{
		private PictureBox _pictureBox;

		public PictureBox PictureBox
		{
			get { return _pictureBox; }
			set
			{
				if (_pictureBox != value)
				{
					_pictureBox = value;
					if (value != null) Resize();
				}
			}
		}

		private NeuroNetwork _neuroNetwork;

		public NeuroNetwork NeuroNetwork
		{
			get { return _neuroNetwork; }
			set
			{
				if (_neuroNetwork != value)
				{
					_neuroNetwork = value;
					if (value != null) Initialize();
				}
			}
		}

		private List<NeuronRepresentation> _inputs;
		private List<List<NeuronRepresentation>> _hiddenLayers;
		private List<NeuronRepresentation> _outputs;
		private List<NeuronRepresentation> _memory;
		private float _neuronSize;
		private int _layersCount;
		private Color WEIGHT_NEGATIVE = Color.FromArgb(52, 198, 205);
		private Color WEIGHT_POSITIVE = Color.FromArgb(255, 69, 64);
		private Color WEIGHT_MEM_NEGATIVE = Color.FromArgb(76, 16, 174);
		private Color WEIGHT_MEM_POSITIVE = Color.FromArgb(255, 225, 0);

		public NeuroNetworkRepresentation(PictureBox pictureBox)
		{
			_pictureBox = pictureBox;
		}

		private void Initialize()
		{
			_inputs = new List<NeuronRepresentation>();
			foreach (var neuron in _neuroNetwork.Inputs)
			{
				_inputs.Add(new NeuronRepresentation(neuron, new PointF()));
			}

			_hiddenLayers = new List<List<NeuronRepresentation>>();
			for (int i = 0; i < _neuroNetwork.HiddenLayers.Count; i++)
			{
				_hiddenLayers.Add(new List<NeuronRepresentation>());
				foreach (var neuron in _neuroNetwork.HiddenLayers[i])
				{
					_hiddenLayers[i].Add(new NeuronRepresentation(neuron, new PointF()));
				}
			}

			_outputs = new List<NeuronRepresentation>();
			foreach (var neuron in _neuroNetwork.Outputs)
			{
				_outputs.Add(new NeuronRepresentation(neuron, new PointF()));
			}

			_memory = new List<NeuronRepresentation>();
			foreach (var neuron in _neuroNetwork.Memory)
			{
				_memory.Add(new NeuronRepresentation(neuron, new PointF()));
			}

			_layersCount = 2 + _hiddenLayers.Count;
			Resize();
		}

		public void Resize()
		{
			if (_neuroNetwork == null) return;
			_pictureBox.Image = new Bitmap(_pictureBox.Width, _pictureBox.Height);
			int width = _pictureBox.Width;
			int height = _pictureBox.Height;
			if (width < 1 || height < 1) return;
			float indentHor = width * 0.01f;
			float indentVert = height * 0.01f;

			int maxNeuronPerLayerCount = 0;
			float gapHor = (width - 2 * indentHor) / (_layersCount + 1);
			int indentMemory = _memory.Count == 0 ? 0 : 3;

			for (int i = 0; i < _layersCount; i++)
			{
				maxNeuronPerLayerCount = Math.Max(this[i].Count + indentMemory, maxNeuronPerLayerCount);
				float gapVert = (height - 2 * indentVert) / (this[i].Count + 1 + indentMemory);
				for (int j = 0; j < this[i].Count; j++)
				{
					this[i][j].Position = new PointF(indentHor + gapHor * (i + 1), indentVert + gapVert * (j + 1));
				}
			}
			_neuronSize = (height - 2 * indentHor) / (maxNeuronPerLayerCount + 1) * 0.8f;

			if (_memory.Count > 0)
			{
				indentHor = width * 0.1f;
				float memVertPos = (height - 2 * indentVert) / (maxNeuronPerLayerCount + 1) * maxNeuronPerLayerCount;
				gapHor = (width - 2 * indentHor) / (_memory.Count + 1);
				for (int i = 0; i < _memory.Count; i++)
				{
					_memory[i].Position = new PointF(indentHor + gapHor * (i + 1), memVertPos);
				}
			}

			Redraw();
		}

		public void Redraw()
		{
			if (_neuroNetwork == null) return;
			if (_pictureBox.Image == null) _pictureBox.Image = new Bitmap(_pictureBox.Width, _pictureBox.Height);
			using (var g = Graphics.FromImage(_pictureBox.Image))
			{
				g.Clear(Color.Black);
				for (int i = 1; i < _layersCount; i++)
				{
					for (int j = 0; j < this[i].Count; j++)
					{
						for (int w = 0; w < this[i][j].Neuron.Weights.Length; w++)
						{
							var weight = this[i][j].Neuron.Weights[w];
							if (i == 1 && w >= _inputs.Count)
							{
								using (var pen = new Pen(Color.FromArgb((int)(weight * weight * 255),
									weight < 0 ? WEIGHT_MEM_NEGATIVE : WEIGHT_MEM_POSITIVE), (float)(weight * weight) * 2))
								{
									g.DrawLine(pen, _memory[w - _inputs.Count].Position, this[i][j].Position);
								}
							}
							else
							{
								using (var pen = new Pen(Color.FromArgb((int)(weight * weight * 255),
									weight < 0 ? WEIGHT_NEGATIVE : WEIGHT_POSITIVE), (float)(weight * weight) * 2))
								{
									g.DrawLine(pen, this[i - 1][w].Position, this[i][j].Position);
								}
							}
						}
					}
				}
				if (_memory.Count > 0)
				{
					for (int i = 0; i < _memory.Count; i++)
					{
						for (int w = 0; w < _memory[i].Neuron.Weights.Length; w++)
						{
							var weight = _memory[i].Neuron.Weights[w];
							using (var pen = new Pen(Color.FromArgb((int)(weight * weight * 255),
								weight < 0 ? WEIGHT_MEM_NEGATIVE : WEIGHT_MEM_POSITIVE), (float)(weight * weight) * 2))
							{
								g.DrawLine(pen, this[_layersCount - 2][w].Position, _memory[i].Position);
							}
						}
					}
				}
				for (int i = 0; i < _layersCount; i++)
				{
					using (var brush = new SolidBrush(this[i][0].Color))
					using (var pen = new Pen(this[i][0].Color))
					{
						for (int j = 0; j < this[i].Count; j++)
						{
							var value = Math.Abs(this[i][j].Neuron.Value);
							g.FillEllipse(value > 0.5 ? brush : Brushes.Black,
								this[i][j].Position.X - _neuronSize / 2,
								this[i][j].Position.Y - _neuronSize / 2,
								_neuronSize, _neuronSize);
							g.DrawEllipse(pen,
								this[i][j].Position.X - _neuronSize / 2,
								this[i][j].Position.Y - _neuronSize / 2,
								_neuronSize, _neuronSize);
						}
					}
				}
				using (var brush = new SolidBrush(_memory[0].Color))
				using (var pen = new Pen(_memory[0].Color))
				{
					for (int i = 0; i < _memory.Count; i++)
					{
						var value = Math.Abs(_memory[i].Neuron.Value);
						g.FillEllipse(value > 0.5 ? brush : Brushes.Black,
							_memory[i].Position.X - _neuronSize / 2,
							_memory[i].Position.Y - _neuronSize / 2,
							_neuronSize, _neuronSize);
						g.DrawEllipse(pen,
							_memory[i].Position.X - _neuronSize / 2,
							_memory[i].Position.Y - _neuronSize / 2,
							_neuronSize, _neuronSize);
					}
				}
				g.DrawString($"#{_neuroNetwork.Id}", new Font("Consolas", 12), Brushes.LightGray, 8, 8);
			}
			_pictureBox.Refresh();
		}

		private List<NeuronRepresentation> this[int i]
		{
			get
			{
				if (i == 0) return _inputs;
				else if (i == _layersCount - 1) return _outputs;
				else return _hiddenLayers[i - 1];
			}
		}
	}
}