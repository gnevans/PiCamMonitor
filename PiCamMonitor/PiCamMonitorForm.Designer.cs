namespace PiCamMonitor
{
	partial class PiCamMonitorForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PiCamMonitorForm));
			this.listBoxPiCam = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonCheckForFiles = new System.Windows.Forms.Button();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.labelFramePos = new System.Windows.Forms.Label();
			this.trackBar = new System.Windows.Forms.TrackBar();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboBoxFramesets = new System.Windows.Forms.ComboBox();
			this.radioButtonViewDownloadedFrames = new System.Windows.Forms.RadioButton();
			this.radioButtonViewLiveFeed = new System.Windows.Forms.RadioButton();
			this.radioButtonViewNothing = new System.Windows.Forms.RadioButton();
			this.buttonPlay = new System.Windows.Forms.Button();
			this.buttonFirst = new System.Windows.Forms.Button();
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonLast = new System.Windows.Forms.Button();
			this.timerPlayback = new System.Windows.Forms.Timer(this.components);
			this.labelDownloadProgress = new System.Windows.Forms.Label();
			this.labelFrameName = new System.Windows.Forms.Label();
			this.checkBoxTail = new System.Windows.Forms.CheckBox();
			this.timerDownload = new System.Windows.Forms.Timer(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBoxPiCam
			// 
			this.listBoxPiCam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxPiCam.FormattingEnabled = true;
			this.listBoxPiCam.Location = new System.Drawing.Point(12, 25);
			this.listBoxPiCam.Name = "listBoxPiCam";
			this.listBoxPiCam.Size = new System.Drawing.Size(720, 69);
			this.listBoxPiCam.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "PiCamMonitor Log";
			// 
			// buttonCheckForFiles
			// 
			this.buttonCheckForFiles.Location = new System.Drawing.Point(6, 19);
			this.buttonCheckForFiles.Name = "buttonCheckForFiles";
			this.buttonCheckForFiles.Size = new System.Drawing.Size(134, 23);
			this.buttonCheckForFiles.TabIndex = 2;
			this.buttonCheckForFiles.Text = "Check for Files";
			this.buttonCheckForFiles.UseVisualStyleBackColor = true;
			this.buttonCheckForFiles.Click += new System.EventHandler(this.buttonCheckForFiles_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox.BackColor = System.Drawing.SystemColors.GrayText;
			this.pictureBox.Location = new System.Drawing.Point(12, 191);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(720, 406);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 4;
			this.pictureBox.TabStop = false;
			// 
			// labelFramePos
			// 
			this.labelFramePos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelFramePos.Location = new System.Drawing.Point(18, 632);
			this.labelFramePos.Name = "labelFramePos";
			this.labelFramePos.Size = new System.Drawing.Size(192, 13);
			this.labelFramePos.TabIndex = 5;
			this.labelFramePos.Text = "Frame n of m";
			// 
			// trackBar
			// 
			this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBar.AutoSize = false;
			this.trackBar.Location = new System.Drawing.Point(12, 603);
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new System.Drawing.Size(720, 25);
			this.trackBar.TabIndex = 8;
			this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBoxFramesets);
			this.groupBox1.Controls.Add(this.radioButtonViewDownloadedFrames);
			this.groupBox1.Controls.Add(this.radioButtonViewLiveFeed);
			this.groupBox1.Controls.Add(this.radioButtonViewNothing);
			this.groupBox1.Location = new System.Drawing.Point(12, 100);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(332, 85);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "View";
			// 
			// comboBoxFramesets
			// 
			this.comboBoxFramesets.FormattingEnabled = true;
			this.comboBoxFramesets.Location = new System.Drawing.Point(167, 59);
			this.comboBoxFramesets.Name = "comboBoxFramesets";
			this.comboBoxFramesets.Size = new System.Drawing.Size(121, 21);
			this.comboBoxFramesets.TabIndex = 3;
			this.comboBoxFramesets.SelectedIndexChanged += new System.EventHandler(this.comboBoxFramesets_SelectedIndexChanged);
			// 
			// radioButtonViewDownloadedFrames
			// 
			this.radioButtonViewDownloadedFrames.Location = new System.Drawing.Point(9, 61);
			this.radioButtonViewDownloadedFrames.Name = "radioButtonViewDownloadedFrames";
			this.radioButtonViewDownloadedFrames.Size = new System.Drawing.Size(152, 18);
			this.radioButtonViewDownloadedFrames.TabIndex = 2;
			this.radioButtonViewDownloadedFrames.Text = "Downloaded Frames for";
			this.radioButtonViewDownloadedFrames.UseVisualStyleBackColor = true;
			this.radioButtonViewDownloadedFrames.CheckedChanged += new System.EventHandler(this.ViewChanged);
			// 
			// radioButtonViewLiveFeed
			// 
			this.radioButtonViewLiveFeed.Location = new System.Drawing.Point(9, 40);
			this.radioButtonViewLiveFeed.Name = "radioButtonViewLiveFeed";
			this.radioButtonViewLiveFeed.Size = new System.Drawing.Size(125, 18);
			this.radioButtonViewLiveFeed.TabIndex = 1;
			this.radioButtonViewLiveFeed.Text = "Live Feed";
			this.radioButtonViewLiveFeed.UseVisualStyleBackColor = true;
			this.radioButtonViewLiveFeed.CheckedChanged += new System.EventHandler(this.ViewChanged);
			// 
			// radioButtonViewNothing
			// 
			this.radioButtonViewNothing.Checked = true;
			this.radioButtonViewNothing.Location = new System.Drawing.Point(9, 19);
			this.radioButtonViewNothing.Name = "radioButtonViewNothing";
			this.radioButtonViewNothing.Size = new System.Drawing.Size(125, 18);
			this.radioButtonViewNothing.TabIndex = 0;
			this.radioButtonViewNothing.TabStop = true;
			this.radioButtonViewNothing.Text = "Nothing";
			this.radioButtonViewNothing.UseVisualStyleBackColor = true;
			this.radioButtonViewNothing.CheckedChanged += new System.EventHandler(this.ViewChanged);
			// 
			// buttonPlay
			// 
			this.buttonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonPlay.Image = ((System.Drawing.Image)(resources.GetObject("buttonPlay.Image")));
			this.buttonPlay.Location = new System.Drawing.Point(241, 634);
			this.buttonPlay.Name = "buttonPlay";
			this.buttonPlay.Size = new System.Drawing.Size(50, 23);
			this.buttonPlay.TabIndex = 10;
			this.buttonPlay.UseVisualStyleBackColor = true;
			this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
			// 
			// buttonFirst
			// 
			this.buttonFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonFirst.Image = ((System.Drawing.Image)(resources.GetObject("buttonFirst.Image")));
			this.buttonFirst.Location = new System.Drawing.Point(322, 634);
			this.buttonFirst.Name = "buttonFirst";
			this.buttonFirst.Size = new System.Drawing.Size(50, 23);
			this.buttonFirst.TabIndex = 11;
			this.buttonFirst.UseVisualStyleBackColor = true;
			this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonPrevious.Image = ((System.Drawing.Image)(resources.GetObject("buttonPrevious.Image")));
			this.buttonPrevious.Location = new System.Drawing.Point(378, 634);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(50, 23);
			this.buttonPrevious.TabIndex = 12;
			this.buttonPrevious.UseVisualStyleBackColor = true;
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonNext.Image = ((System.Drawing.Image)(resources.GetObject("buttonNext.Image")));
			this.buttonNext.Location = new System.Drawing.Point(434, 634);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(50, 23);
			this.buttonNext.TabIndex = 13;
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonLast
			// 
			this.buttonLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonLast.Image = ((System.Drawing.Image)(resources.GetObject("buttonLast.Image")));
			this.buttonLast.Location = new System.Drawing.Point(490, 634);
			this.buttonLast.Name = "buttonLast";
			this.buttonLast.Size = new System.Drawing.Size(50, 23);
			this.buttonLast.TabIndex = 14;
			this.buttonLast.UseVisualStyleBackColor = true;
			this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
			// 
			// labelDownloadProgress
			// 
			this.labelDownloadProgress.Location = new System.Drawing.Point(6, 45);
			this.labelDownloadProgress.Name = "labelDownloadProgress";
			this.labelDownloadProgress.Size = new System.Drawing.Size(140, 13);
			this.labelDownloadProgress.TabIndex = 15;
			this.labelDownloadProgress.Text = "Download Progress";
			// 
			// labelFrameName
			// 
			this.labelFrameName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelFrameName.Location = new System.Drawing.Point(18, 647);
			this.labelFrameName.Name = "labelFrameName";
			this.labelFrameName.Size = new System.Drawing.Size(192, 13);
			this.labelFrameName.TabIndex = 16;
			this.labelFrameName.Text = "Frame name";
			// 
			// checkBoxTail
			// 
			this.checkBoxTail.AutoSize = true;
			this.checkBoxTail.Checked = true;
			this.checkBoxTail.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxTail.Location = new System.Drawing.Point(130, 7);
			this.checkBoxTail.Name = "checkBoxTail";
			this.checkBoxTail.Size = new System.Drawing.Size(43, 17);
			this.checkBoxTail.TabIndex = 17;
			this.checkBoxTail.Text = "Tail";
			this.checkBoxTail.UseVisualStyleBackColor = true;
			this.checkBoxTail.CheckedChanged += new System.EventHandler(this.checkBoxTail_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.buttonCheckForFiles);
			this.groupBox2.Controls.Add(this.labelDownloadProgress);
			this.groupBox2.Location = new System.Drawing.Point(350, 108);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(152, 77);
			this.groupBox2.TabIndex = 18;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Download";
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "PiCamMonitor";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(153, 76);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// PiCamMonitorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(744, 665);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.checkBoxTail);
			this.Controls.Add(this.labelFrameName);
			this.Controls.Add(this.buttonLast);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonPrevious);
			this.Controls.Add(this.buttonFirst);
			this.Controls.Add(this.buttonPlay);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.trackBar);
			this.Controls.Add(this.labelFramePos);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBoxPiCam);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PiCamMonitorForm";
			this.Text = "PiCamMonitor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PiCamForm_FormClosing);
			this.Load += new System.EventHandler(this.PiCamForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxPiCam;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonCheckForFiles;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Label labelFramePos;
		private System.Windows.Forms.TrackBar trackBar;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonViewDownloadedFrames;
		private System.Windows.Forms.RadioButton radioButtonViewLiveFeed;
		private System.Windows.Forms.RadioButton radioButtonViewNothing;
		private System.Windows.Forms.Button buttonPlay;
		private System.Windows.Forms.Button buttonFirst;
		private System.Windows.Forms.Button buttonPrevious;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonLast;
		private System.Windows.Forms.Timer timerPlayback;
		private System.Windows.Forms.Label labelDownloadProgress;
		private System.Windows.Forms.ComboBox comboBoxFramesets;
		private System.Windows.Forms.Label labelFrameName;
		private System.Windows.Forms.CheckBox checkBoxTail;
		private System.Windows.Forms.Timer timerDownload;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
	}
}

