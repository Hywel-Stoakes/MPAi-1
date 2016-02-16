﻿using MPAid.Models;

namespace MPAid
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.headerBox = new System.Windows.Forms.PictureBox();
            this.categoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administratorConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submitFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationPanel = new MPAid.UserControls.OperationPanel();
            this.recordingPanel = new MPAid.UserControls.RecordingPanel();
            ((System.ComponentModel.ISupportInitialize)(this.headerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerBox
            // 
            this.headerBox.BackgroundImage = global::MPAid.Properties.Resources.header;
            this.headerBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.headerBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerBox.Location = new System.Drawing.Point(0, 32);
            this.headerBox.Name = "headerBox";
            this.headerBox.Size = new System.Drawing.Size(864, 81);
            this.headerBox.TabIndex = 0;
            this.headerBox.TabStop = false;
            this.headerBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.headerBox_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.operationPanel);
            this.panel1.Controls.Add(this.recordingPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 409);
            this.panel1.TabIndex = 7;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.configToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(864, 32);
            this.mainMenuStrip.TabIndex = 8;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.administratorConsoleToolStripMenuItem,
            this.changePasswordToolStripMenuItem,
            this.logToolStripMenuItem});
            this.usersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("usersToolStripMenuItem.Image")));
            this.usersToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(71, 28);
            this.usersToolStripMenuItem.Text = "Users";
            // 
            // administratorConsoleToolStripMenuItem
            // 
            this.administratorConsoleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("administratorConsoleToolStripMenuItem.Image")));
            this.administratorConsoleToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.administratorConsoleToolStripMenuItem.Name = "administratorConsoleToolStripMenuItem";
            this.administratorConsoleToolStripMenuItem.Size = new System.Drawing.Size(201, 30);
            this.administratorConsoleToolStripMenuItem.Text = "Administrator Console";
            this.administratorConsoleToolStripMenuItem.Click += new System.EventHandler(this.administratorConsoleToolStripMenuItem_Click);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("changePasswordToolStripMenuItem.Image")));
            this.changePasswordToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(201, 30);
            this.changePasswordToolStripMenuItem.Text = "Change password";
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logToolStripMenuItem.Image")));
            this.logToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(201, 30);
            this.logToolStripMenuItem.Text = "Sign out";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem,
            this.recordingToolStripMenuItem});
            this.configToolStripMenuItem.Image = global::MPAid.Properties.Resources.configure_icon;
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(71, 28);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.Image = global::MPAid.Properties.Resources.system_icon;
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.systemToolStripMenuItem.Text = "System";
            this.systemToolStripMenuItem.Click += new System.EventHandler(this.systemToolStripMenuItem_Click);
            // 
            // recordingToolStripMenuItem
            // 
            this.recordingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadToolStripMenuItem,
            this.renameToolStripMenuItem});
            this.recordingToolStripMenuItem.Image = global::MPAid.Properties.Resources.recording_icon;
            this.recordingToolStripMenuItem.Name = "recordingToolStripMenuItem";
            this.recordingToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.recordingToolStripMenuItem.Text = "Recording";
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Image = global::MPAid.Properties.Resources.upload_icon;
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.uploadToolStripMenuItem.Text = "Upload";
            this.uploadToolStripMenuItem.Click += new System.EventHandler(this.uploadToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::MPAid.Properties.Resources.rename_icon;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submitFeedbackToolStripMenuItem,
            this.tutorialToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripMenuItem.Image")));
            this.helpToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(64, 28);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // submitFeedbackToolStripMenuItem
            // 
            this.submitFeedbackToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("submitFeedbackToolStripMenuItem.Image")));
            this.submitFeedbackToolStripMenuItem.Name = "submitFeedbackToolStripMenuItem";
            this.submitFeedbackToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.submitFeedbackToolStripMenuItem.Text = "Submit Feedback";
            this.submitFeedbackToolStripMenuItem.Click += new System.EventHandler(this.submitFeedbackToolStripMenuItem_Click);
            // 
            // tutorialToolStripMenuItem
            // 
            this.tutorialToolStripMenuItem.Image = global::MPAid.Properties.Resources.tutorials_icon;
            this.tutorialToolStripMenuItem.Name = "tutorialToolStripMenuItem";
            this.tutorialToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.tutorialToolStripMenuItem.Text = "Instruction";
            this.tutorialToolStripMenuItem.Click += new System.EventHandler(this.tutorialToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // operationPanel
            // 
            this.operationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operationPanel.Location = new System.Drawing.Point(219, 7);
            this.operationPanel.Name = "operationPanel";
            this.operationPanel.Size = new System.Drawing.Size(642, 390);
            this.operationPanel.TabIndex = 8;
            // 
            // recordingPanel
            // 
            this.recordingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recordingPanel.Location = new System.Drawing.Point(12, 6);
            this.recordingPanel.Name = "recordingPanel";
            this.recordingPanel.Size = new System.Drawing.Size(201, 391);
            this.recordingPanel.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(864, 522);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.headerBox);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(720, 560);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Māori Pronunciation Aid";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.headerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox headerBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administratorConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem submitFeedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordingToolStripMenuItem;
        private System.Windows.Forms.BindingSource categoryBindingSource;
        private UserControls.RecordingPanel recordingPanel;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private UserControls.OperationPanel operationPanel;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
    }
}

