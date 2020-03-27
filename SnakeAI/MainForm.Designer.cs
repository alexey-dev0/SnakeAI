namespace SnakeAI
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.pDebug = new System.Windows.Forms.Panel();
			this.trackDrawer = new System.Windows.Forms.TrackBar();
			this.pbNeuroNetwork = new System.Windows.Forms.PictureBox();
			this.lblLastFitness = new System.Windows.Forms.Label();
			this.lblMaxFitness = new System.Windows.Forms.Label();
			this.lblPopulation = new System.Windows.Forms.Label();
			this.pbUpdateProgress = new System.Windows.Forms.ProgressBar();
			this.cbUpdates = new System.Windows.Forms.ComboBox();
			this.cbSynchronous = new System.Windows.Forms.CheckBox();
			this.lblUpdates = new System.Windows.Forms.Label();
			this.pbField = new System.Windows.Forms.PictureBox();
			this.tTicker = new System.Windows.Forms.Timer(this.components);
			this.tDrawer = new System.Windows.Forms.Timer(this.components);
			this.pDebug.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackDrawer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbNeuroNetwork)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbField)).BeginInit();
			this.SuspendLayout();
			// 
			// pDebug
			// 
			this.pDebug.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pDebug.Controls.Add(this.trackDrawer);
			this.pDebug.Controls.Add(this.pbNeuroNetwork);
			this.pDebug.Controls.Add(this.lblLastFitness);
			this.pDebug.Controls.Add(this.lblMaxFitness);
			this.pDebug.Controls.Add(this.lblPopulation);
			this.pDebug.Controls.Add(this.pbUpdateProgress);
			this.pDebug.Controls.Add(this.cbUpdates);
			this.pDebug.Controls.Add(this.cbSynchronous);
			this.pDebug.Controls.Add(this.lblUpdates);
			this.pDebug.Dock = System.Windows.Forms.DockStyle.Left;
			this.pDebug.Location = new System.Drawing.Point(0, 0);
			this.pDebug.Name = "pDebug";
			this.pDebug.Size = new System.Drawing.Size(234, 478);
			this.pDebug.TabIndex = 0;
			// 
			// trackDrawer
			// 
			this.trackDrawer.AutoSize = false;
			this.trackDrawer.Enabled = false;
			this.trackDrawer.Location = new System.Drawing.Point(15, 114);
			this.trackDrawer.Maximum = 625;
			this.trackDrawer.Minimum = 1;
			this.trackDrawer.Name = "trackDrawer";
			this.trackDrawer.Size = new System.Drawing.Size(213, 30);
			this.trackDrawer.TabIndex = 11;
			this.trackDrawer.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackDrawer.Value = 625;
			this.trackDrawer.ValueChanged += new System.EventHandler(this.trackDrawer_ValueChanged);
			// 
			// pbNeuroNetwork
			// 
			this.pbNeuroNetwork.Location = new System.Drawing.Point(3, 204);
			this.pbNeuroNetwork.Name = "pbNeuroNetwork";
			this.pbNeuroNetwork.Size = new System.Drawing.Size(228, 271);
			this.pbNeuroNetwork.TabIndex = 10;
			this.pbNeuroNetwork.TabStop = false;
			this.pbNeuroNetwork.Click += new System.EventHandler(this.pbNeuroNetwork_Click);
			// 
			// lblLastFitness
			// 
			this.lblLastFitness.AutoSize = true;
			this.lblLastFitness.Location = new System.Drawing.Point(12, 183);
			this.lblLastFitness.Name = "lblLastFitness";
			this.lblLastFitness.Size = new System.Drawing.Size(112, 18);
			this.lblLastFitness.TabIndex = 8;
			this.lblLastFitness.Text = "Last Fitness:";
			// 
			// lblMaxFitness
			// 
			this.lblMaxFitness.AutoSize = true;
			this.lblMaxFitness.Location = new System.Drawing.Point(12, 165);
			this.lblMaxFitness.Name = "lblMaxFitness";
			this.lblMaxFitness.Size = new System.Drawing.Size(112, 18);
			this.lblMaxFitness.TabIndex = 7;
			this.lblMaxFitness.Text = "Max  Fitness:";
			// 
			// lblPopulation
			// 
			this.lblPopulation.AutoSize = true;
			this.lblPopulation.Location = new System.Drawing.Point(12, 147);
			this.lblPopulation.Name = "lblPopulation";
			this.lblPopulation.Size = new System.Drawing.Size(96, 18);
			this.lblPopulation.TabIndex = 5;
			this.lblPopulation.Text = "Population:";
			// 
			// pbUpdateProgress
			// 
			this.pbUpdateProgress.Location = new System.Drawing.Point(15, 57);
			this.pbUpdateProgress.Name = "pbUpdateProgress";
			this.pbUpdateProgress.Size = new System.Drawing.Size(213, 23);
			this.pbUpdateProgress.TabIndex = 4;
			// 
			// cbUpdates
			// 
			this.cbUpdates.FormattingEnabled = true;
			this.cbUpdates.Items.AddRange(new object[] {
            "1",
            "1000",
            "10000",
            "100000",
            "1000000",
            "10000000"});
			this.cbUpdates.Location = new System.Drawing.Point(15, 30);
			this.cbUpdates.Name = "cbUpdates";
			this.cbUpdates.Size = new System.Drawing.Size(213, 26);
			this.cbUpdates.TabIndex = 3;
			// 
			// cbSynchronous
			// 
			this.cbSynchronous.AutoSize = true;
			this.cbSynchronous.Location = new System.Drawing.Point(15, 86);
			this.cbSynchronous.Name = "cbSynchronous";
			this.cbSynchronous.Size = new System.Drawing.Size(155, 22);
			this.cbSynchronous.TabIndex = 2;
			this.cbSynchronous.Text = "Synchronous Draw";
			this.cbSynchronous.UseVisualStyleBackColor = true;
			this.cbSynchronous.CheckedChanged += new System.EventHandler(this.cbSynchronous_CheckedChanged);
			// 
			// lblUpdates
			// 
			this.lblUpdates.AutoSize = true;
			this.lblUpdates.Location = new System.Drawing.Point(12, 9);
			this.lblUpdates.Name = "lblUpdates";
			this.lblUpdates.Size = new System.Drawing.Size(144, 18);
			this.lblUpdates.TabIndex = 0;
			this.lblUpdates.Text = "Updates per step:";
			// 
			// pbField
			// 
			this.pbField.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbField.Location = new System.Drawing.Point(234, 0);
			this.pbField.Name = "pbField";
			this.pbField.Size = new System.Drawing.Size(617, 478);
			this.pbField.TabIndex = 1;
			this.pbField.TabStop = false;
			// 
			// tTicker
			// 
			this.tTicker.Enabled = true;
			this.tTicker.Interval = 1;
			this.tTicker.Tick += new System.EventHandler(this.tTicker_Tick);
			// 
			// tDrawer
			// 
			this.tDrawer.Enabled = true;
			this.tDrawer.Interval = 15;
			this.tDrawer.Tick += new System.EventHandler(this.tDrawer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 478);
			this.Controls.Add(this.pbField);
			this.Controls.Add(this.pDebug);
			this.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.Text = "SnakeAI";
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.pDebug.ResumeLayout(false);
			this.pDebug.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackDrawer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbNeuroNetwork)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbField)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pDebug;
        private System.Windows.Forms.PictureBox pbField;
		private System.Windows.Forms.Timer tTicker;
		private System.Windows.Forms.Label lblUpdates;
		private System.Windows.Forms.Timer tDrawer;
		private System.Windows.Forms.CheckBox cbSynchronous;
		private System.Windows.Forms.ComboBox cbUpdates;
		private System.Windows.Forms.ProgressBar pbUpdateProgress;
		private System.Windows.Forms.Label lblPopulation;
		private System.Windows.Forms.Label lblLastFitness;
		private System.Windows.Forms.Label lblMaxFitness;
		private System.Windows.Forms.PictureBox pbNeuroNetwork;
		private System.Windows.Forms.TrackBar trackDrawer;
	}
}

