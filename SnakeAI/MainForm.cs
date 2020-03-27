using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAI
{
	public partial class MainForm : Form
	{
		private List<Field> fields = new List<Field>();
		private List<Snake> population = new List<Snake>();
		private int populationCount = 100;
		private List<Snake> dead = new List<Snake>();
		private BackgroundWorker worker;
		private int populationNumber = 1;
		private int fieldSize = 16;
		private int cellSize = 4;
		private int fieldGap = 1;
		private int snakeInputs = 23;
		private int snakeOutputs = 2;
		private int snakeMemory = 4;
		private double shakeRate = 0.1;
		private Random R = new Random();
		private Dictionary<string, int> scores = new Dictionary<string, int>();
		private Snake topSnake;
		private Snake topSnake2;
		private StringFormat stringFormat;

		private List<List<int>> hiddenLayersConfig = new List<List<int>>
		{
			new List<int> { 3 }
		};

		public MainForm()
		{
			InitializeComponent();

			worker = new BackgroundWorker();
			worker.DoWork += Worker_DoWork;
			worker.ProgressChanged += Worker_ProgressChanged;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;

			scores["maxFitness"] = 0;
			scores["lastFitness"] = 0;

			cbUpdates.SelectedIndex = 1;
			InitSnakes();
			_neuroNetworkRepresentation = new NeuroNetworkRepresentation(pbNeuroNetwork);
			stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
		}

		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			pbUpdateProgress.Value = e.ProgressPercentage;
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			tTicker.Start();
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			if (cbSynchronous.Checked) return;
			var count = (int)e.Argument;

			for (int i = 0; i < count; i++)
			{
				if (worker.CancellationPending)
				{
					e.Cancel = true;
					worker.ReportProgress(100);
					break;
				}
				var statement = Parallel.ForEach(population, (snake) => UpdateSnake(snake));
				if (!statement.IsCompleted) throw new Exception("Stacked in parallel for-loop update");
				dead = population;
				population = new List<Snake>();
				CreatePopulation();
				var progress = (int)((double)i / count * 100);
				worker.ReportProgress(progress);
			}
			worker.ReportProgress(100);
		}

		private void InitSnakes()
		{
			for (int i = 0; i < populationCount; i++)
			{
				fields.Add(new Field(fieldSize, fieldSize, i * (int)DateTime.Now.Ticks));
				var neuroNetwork = new NeuroNetwork(snakeInputs, hiddenLayersConfig[R.Next(hiddenLayersConfig.Count)], snakeOutputs, snakeMemory);
				population.Add(new Snake(fields[i], neuroNetwork));
			}
		}

		private void Redraw()
		{
			if (pbField.Image == null) pbField.Image = new Bitmap(pbField.Width, pbField.Height);
			var bmp = pbField.Image;
			using (var g = Graphics.FromImage(bmp))
			{
				for (var k = 0; k < populationCount; k++)
				{
					var field = fields[k];
					var offsetSquare = (int)Math.Ceiling(Math.Sqrt(populationCount) * 1.2);
					var xOffset = k % offsetSquare * (fieldSize + fieldGap) * cellSize;
					var yOffset = k / offsetSquare * (fieldSize + fieldGap) * cellSize;
					if (field.Snake == null || field.Snake.Dead)
					{
						g.FillRectangle(Brushes.Black, xOffset, yOffset, cellSize * fieldSize, cellSize * fieldSize);
						g.DrawString("DEAD", Font, Brushes.LightGray,
							xOffset + cellSize * fieldSize * 0.5f,
							yOffset + cellSize * fieldSize * 0.5f,
							stringFormat);
						continue;
					}
					for (int i = 0; i < field.Width; i++)
					{
						for (int j = 0; j < field.Height; j++)
						{
							var color = Color.FromArgb(0, 0, 0);
							if (field[i, j] == Content.SNAKE)
							{
								color = field.Snake == null ? Color.White : field.Snake.NeuroNetwork.Color;
							}
							else if (field[i, j] == Content.FOOD) color = Color.FromArgb(0, 200, 80);
							using (var b = new SolidBrush(color))
							{
								g.FillRectangle(b, xOffset + cellSize * i, yOffset + cellSize * j, cellSize, cellSize);
							}
						}
					}
					//g.DrawString($"{field.Food.Count}", Font, Brushes.White, xOffset, yOffset);
				}
			}
			pbField.Image = bmp;
		}

		private void Debug()
		{
			lblPopulation.Text = $"Population: {populationNumber}";
			lblMaxFitness.Text = $"Max  Fitness: {scores["maxFitness"]}";
			lblLastFitness.Text = $"Last Fitness: {scores["lastFitness"]}";
			/*if (population.Count > 0 && !worker.IsBusy)
            {
                tbNeuro.Text = population[0].NeuroNetwork.ToString();
            }*/
		}

		private void CreatePopulation()
		{
			dead.Sort(new SnakeComparer());
			dead.Reverse();
			if (topSnake == null || dead[0].Fitness > topSnake.Fitness)
			{
				topSnake2 = topSnake;
				topSnake = dead[0];
			}
			if (topSnake2 == null) topSnake2 = dead[1];
			scores["maxFitness"] = Math.Max(scores["maxFitness"], dead[0].Fitness);
			scores["lastFitness"] = dead[0].Fitness;
			if (dead[0].Fitness == 0) throw new Exception();

			var fieldCount = 0;
			/*for (int i = 0; i < populationCount * 0.0; i++)
            {
                var neuroNetwork = dead[0].NeuroNetwork.Copy();
                neuroNetwork.NeuroShake(shakeRate);
                population.Add(new Snake(fields[fieldCount], neuroNetwork));
                fieldCount++;
            }*/
			for (int i = 0; i < populationCount * 0.3; i++)
			{
				var neuroNetwork = topSnake.NeuroNetwork.Copy();
				neuroNetwork.Cross(dead[0].NeuroNetwork);
				neuroNetwork.NeuroShake(shakeRate);
				population.Add(new Snake(fields[fieldCount], neuroNetwork));
				fieldCount++;
			}
			for (int i = 0; i < populationCount * 0.3; i++)
			{
				var neuroNetwork = topSnake.NeuroNetwork.Copy();
				neuroNetwork.NeuroShake(shakeRate);
				population.Add(new Snake(fields[fieldCount], neuroNetwork));
				fieldCount++;
			}
			for (int i = 0; i < populationCount * 0.2; i++)
			{
				var neuroNetwork = topSnake2.NeuroNetwork.Copy();
				neuroNetwork.NeuroShake(shakeRate);
				population.Add(new Snake(fields[fieldCount], neuroNetwork));
				fieldCount++;
			}
			for (int i = 0; i < populationCount * 0.2; i++)
			{
				var neuroNetwork = new NeuroNetwork(snakeInputs, hiddenLayersConfig[R.Next(hiddenLayersConfig.Count)], snakeOutputs, snakeMemory);
				population.Add(new Snake(fields[fieldCount], neuroNetwork));
				fieldCount++;
			}
			foreach (var field in fields)
			{
				field.ReviewFood();
			}
			dead.Clear();
			populationNumber++;
		}

		private void Update()
		{
			SynchronousUpdateDone = false;
			for (int i = population.Count - 1; i >= 0; i--)
			{
				if (population[i].Dead)
				{
					dead.Add(population[i]);
					population.RemoveAt(i);
				}
				else
				{
					population[i].Update();
				}
			}
			if (population.Count == 0)
			{
				CreatePopulation();
				SynchronousUpdateDone = true;
			}
		}

		private bool SynchronousUpdateDone = false;

		private void tTicker_Tick(object sender, EventArgs e)
		{
			if (cbSynchronous.Checked) return;
			tTicker.Stop();
			worker.RunWorkerAsync(int.Parse((string)cbUpdates.SelectedItem));
		}

		private NeuroNetworkRepresentation _neuroNetworkRepresentation;

		private void tDrawer_Tick(object sender, EventArgs e)
		{
			if (cbSynchronous.Checked)
			{
				if (worker.IsBusy) return;
				Update();
				Redraw();
				_neuroNetworkRepresentation.Redraw();
				if (population.Count > 0 && population[0] != null)
				{
					_neuroNetworkRepresentation.NeuroNetwork = population[0].NeuroNetwork;
				}
			}
			Debug();
		}

		private void cbSynchronous_CheckedChanged(object sender, EventArgs e)
		{
			if (cbSynchronous.Checked)
			{
				tTicker.Stop();
				if (worker.IsBusy) worker.CancelAsync();
				trackDrawer.Enabled = true;
			}
			else
			{
				while (!SynchronousUpdateDone) Update();
				tTicker.Start();
				trackDrawer.Enabled = false;
			}
		}

		private void UpdateSnake(Snake snake)
		{
			try
			{
				while (!snake.Dead)
				{
					snake.Update();
				}
			}
			catch (AggregateException e)
			{
				Console.WriteLine(e.InnerException.Message);
				return;
			}
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			pbField.Image = new Bitmap(pbField.Width, pbField.Height);
		}

		private void pbNeuroNetwork_Click(object sender, EventArgs e)
		{
			var representationForm = new Form()
			{
				Width = 800,
				Height = 600
			};
			var pb = new PictureBox()
			{
				Dock = DockStyle.Fill
			};
			representationForm.Controls.Add(pb);
			_neuroNetworkRepresentation.PictureBox = pb;
			ShowInWindowMsg();
			representationForm.Show();
			representationForm.FormClosed += RepresentationForm_FormClosed;
			representationForm.ResizeEnd += RepresentationForm_ResizeEnd;
		}

		private void ShowInWindowMsg()
		{
			if (pbNeuroNetwork.Image == null) pbNeuroNetwork.Image = new Bitmap(pbNeuroNetwork.Width, pbNeuroNetwork.Height);
			using (var g = Graphics.FromImage(pbNeuroNetwork.Image))
			{
				g.Clear(Color.Black);
				float halfW = pbNeuroNetwork.Width * 0.5f;
				float halfH = pbNeuroNetwork.Height * 0.5f;
				g.DrawString("Showing in window...", Font, Brushes.LightGray, halfW, halfH, stringFormat);
			}
			pbNeuroNetwork.Refresh();
		}

		private void RepresentationForm_ResizeEnd(object sender, EventArgs e)
		{
			_neuroNetworkRepresentation.Resize();
		}

		private void RepresentationForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			_neuroNetworkRepresentation.PictureBox = pbNeuroNetwork;
		}

		private void trackDrawer_ValueChanged(object sender, EventArgs e)
		{
			//tDrawer.Stop();
			tDrawer.Interval = 10000 / trackDrawer.Value;
			//tDrawer.Start();
		}
	}
}