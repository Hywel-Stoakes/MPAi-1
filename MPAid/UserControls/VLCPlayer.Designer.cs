﻿namespace MPAid.UserControls
{
    partial class VlcPlayer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VlcPlayer));
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.stopButton = new System.Windows.Forms.Button();
            this.stopImageList = new System.Windows.Forms.ImageList(this.components);
            this.playButton = new System.Windows.Forms.Button();
            this.playImageList = new System.Windows.Forms.ImageList(this.components);
            this.backImageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.forwardButton = new System.Windows.Forms.Button();
            this.forwardImageList = new System.Windows.Forms.ImageList(this.components);
            this.backButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vlcControl
            // 
            this.vlcControl.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.tableLayoutPanel1.SetColumnSpan(this.vlcControl, 10);
            this.vlcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcControl.Location = new System.Drawing.Point(3, 3);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(539, 256);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 3;
            this.vlcControl.Text = "vlcControl1";
            this.vlcControl.VlcLibDirectory = null;
            this.vlcControl.VlcMediaplayerOptions = null;
            this.vlcControl.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.OnVlcControlNeedLibDirectory);
            this.vlcControl.Stopped += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs>(this.OnVlcControlStopped);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.BackColor = System.Drawing.SystemColors.Control;
            this.stopButton.FlatAppearance.BorderSize = 0;
            this.stopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopButton.ImageIndex = 1;
            this.stopButton.ImageList = this.stopImageList;
            this.stopButton.Location = new System.Drawing.Point(273, 265);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(48, 59);
            this.stopButton.TabIndex = 7;
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            this.stopButton.MouseEnter += new System.EventHandler(this.stopButton_MouseEnter);
            this.stopButton.MouseLeave += new System.EventHandler(this.stopButton_MouseLeave);
            // 
            // stopImageList
            // 
            this.stopImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("stopImageList.ImageStream")));
            this.stopImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.stopImageList.Images.SetKeyName(0, "Light Stop.png");
            this.stopImageList.Images.SetKeyName(1, "Stop.png");
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.BackColor = System.Drawing.SystemColors.Control;
            this.playButton.FlatAppearance.BorderSize = 0;
            this.playButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.playButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.playButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playButton.ImageIndex = 1;
            this.playButton.ImageList = this.playImageList;
            this.playButton.Location = new System.Drawing.Point(219, 265);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(48, 59);
            this.playButton.TabIndex = 6;
            this.playButton.UseVisualStyleBackColor = false;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            this.playButton.MouseEnter += new System.EventHandler(this.playButton_MouseEnter);
            this.playButton.MouseLeave += new System.EventHandler(this.playButton_MouseLeave);
            // 
            // playImageList
            // 
            this.playImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("playImageList.ImageStream")));
            this.playImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.playImageList.Images.SetKeyName(0, "Light Play.png");
            this.playImageList.Images.SetKeyName(1, "Play.png");
            this.playImageList.Images.SetKeyName(2, "Light Pause.png");
            this.playImageList.Images.SetKeyName(3, "Pause.png");
            // 
            // backImageList
            // 
            this.backImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("backImageList.ImageStream")));
            this.backImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.backImageList.Images.SetKeyName(0, "Light Backward.png");
            this.backImageList.Images.SetKeyName(1, "Backward.png");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 10;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.vlcControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.forwardButton, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.backButton, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.playButton, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.stopButton, 5, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(545, 327);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // forwardButton
            // 
            this.forwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forwardButton.BackColor = System.Drawing.SystemColors.Control;
            this.forwardButton.FlatAppearance.BorderSize = 0;
            this.forwardButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.forwardButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.forwardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.forwardButton.ImageIndex = 1;
            this.forwardButton.ImageList = this.forwardImageList;
            this.forwardButton.Location = new System.Drawing.Point(327, 265);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(48, 59);
            this.forwardButton.TabIndex = 8;
            this.forwardButton.UseVisualStyleBackColor = false;
            this.forwardButton.MouseEnter += new System.EventHandler(this.forwardButton_MouseEnter);
            this.forwardButton.MouseLeave += new System.EventHandler(this.forwardButton_MouseLeave);
            // 
            // forwardImageList
            // 
            this.forwardImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("forwardImageList.ImageStream")));
            this.forwardImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.forwardImageList.Images.SetKeyName(0, "Light Forward.png");
            this.forwardImageList.Images.SetKeyName(1, "Forward.png");
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.BackColor = System.Drawing.SystemColors.Control;
            this.backButton.FlatAppearance.BorderSize = 0;
            this.backButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.ImageIndex = 1;
            this.backButton.ImageList = this.backImageList;
            this.backButton.Location = new System.Drawing.Point(165, 265);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(48, 59);
            this.backButton.TabIndex = 5;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.MouseEnter += new System.EventHandler(this.backButton_MouseEnter);
            this.backButton.MouseLeave += new System.EventHandler(this.backButton_MouseLeave);
            // 
            // VlcPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VlcPlayer";
            this.Size = new System.Drawing.Size(545, 327);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.ImageList backImageList;
        private System.Windows.Forms.ImageList stopImageList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.ImageList playImageList;
        private System.Windows.Forms.ImageList forwardImageList;
    }
}
