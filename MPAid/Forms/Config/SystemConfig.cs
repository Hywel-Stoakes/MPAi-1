﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MPAid.Cores;
using NAudio.CoreAudioApi;

namespace MPAid.Forms.Config
{
    public partial class SystemConfig : Form
    {
        public SystemConfig()
        {
            InitializeComponent();

            InitializeContent();
        }

        private void InitializeContent()
        {
            this.audioFolderTextBox.Text = SystemConfigration.configs.AudioFolderAddr.FolderAddr;
            this.audioFolderTextBox.TextChanged += AudioFolderTextBox_TextChanged;

            this.videoFolderTextBox.Text = SystemConfigration.configs.VideoFolderAddr.FolderAddr;
            this.videoFolderTextBox.TextChanged += VideoFolderTextBox_TextChanged;

            this.recordingFolderTextBox.Text = SystemConfigration.configs.RecordingFolderAddr.FolderAddr;
            this.recordingFolderTextBox.TextChanged += RecordingFolderTextBox_TextChanged;

            this.reportFolderTextBox.Text = SystemConfigration.configs.ReportFolderAddr.FolderAddr;
            this.reportFolderTextBox.TextChanged += ReportFolderTextBox_TextChanged;

            this.HTKFolderTextBox.Text = SystemConfigration.configs.HTKFolderAddr.FolderAddr;
            this.HTKFolderTextBox.TextChanged += HTKFolderTextBox_TextChanged;

            this.fomantFolderTextBox.Text = SystemConfigration.configs.FomantFolderAddr.FolderAddr;
            this.fomantFolderTextBox.TextChanged += FomantFolderTextBox_TextChanged;
        }

        private void AudioFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            SystemConfigration.configs.AudioFolderAddr.FolderAddr = this.audioFolderTextBox.Text;
        }
        private void VideoFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            SystemConfigration.configs.VideoFolderAddr.FolderAddr = this.videoFolderTextBox.Text;
        }
        private void RecordingFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            SystemConfigration.configs.RecordingFolderAddr.FolderAddr = this.recordingFolderTextBox.Text;
        }
        private void ReportFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            SystemConfigration.configs.ReportFolderAddr.FolderAddr = this.reportFolderTextBox.Text;
        }
        private void HTKFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            SystemConfigration.configs.HTKFolderAddr.FolderAddr = this.HTKFolderTextBox.Text;
        }
        private void FomantFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            SystemConfigration.configs.FomantFolderAddr.FolderAddr = this.fomantFolderTextBox.Text;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Serializer<SysCfg>.Save<BinaryFormatter>(SysCfg.path, SystemConfigration.configs);
            this.Close();
        }
        private void audioFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.audioFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void videoFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.videoFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void recordingFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.recordingFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void reportFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.reportFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void HTKFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.HTKFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void fomantFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.fomantFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
