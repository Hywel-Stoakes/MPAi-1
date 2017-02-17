﻿using MPAi.Components;
using System;
using System.IO;
using System.Windows.Forms;

namespace MPAi.Forms.Popups
{
    /// <summary>
    /// Class for the system configuration form.
    /// </summary>
    public partial class SystemConfig : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SystemConfig()
        {
            InitializeComponent();
            InitializeContent();
        }
        /// <summary>
        /// Initialises all text boxes, and fills them in with values from the current system settings.
        /// </summary>
        public void InitializeContent()
        {
            try
            {
                // Audio folder.
                InitializeTextBox(this.audioFolderTextBox, Properties.Settings.Default.AudioFolder);
                // Video folder.
                InitializeTextBox(this.videoFolderTextBox, Properties.Settings.Default.VideoFolder);
                // Recording folder.
                InitializeTextBox(this.recordingFolderTextBox, Properties.Settings.Default.RecordingFolder);
                // Report folder.
                InitializeTextBox(this.reportFolderTextBox, Properties.Settings.Default.ReportFolder);
                // HTK folder.
                InitializeTextBox(this.HTKFolderTextBox, Properties.Settings.Default.HTKFolder);
                // Formant folder.
                InitializeTextBox(this.formantFolderTextBox, Properties.Settings.Default.FormantFolder);
            }
            catch(Exception exp)
            {
                MPAiMessageBoxFactory.Show(exp.Message);
            }
        }
        /// <summary>
        /// Populates the text boxes on the form and creates the referenced directories if they don't already exist.
        /// </summary>
        /// <param name="tb">The text box to initialise.</param>
        /// <param name="dir">The directory to create (if needed) and fill the text box with.</param>
        private void InitializeTextBox(TextBox tb, string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
                tb.Text = dir;
            }
            catch(Exception exp)
            {
                MPAiMessageBoxFactory.Show(exp.Message);
            }
        }
        /// <summary>
        /// When the save button is clicked, saves the changes to the settings and closes the window.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AudioFolder = this.audioFolderTextBox.Text;
            Properties.Settings.Default.VideoFolder = this.videoFolderTextBox.Text;
            Properties.Settings.Default.RecordingFolder = this.recordingFolderTextBox.Text;
            Properties.Settings.Default.ReportFolder = this.reportFolderTextBox.Text;
            Properties.Settings.Default.HTKFolder = this.HTKFolderTextBox.Text;
            Properties.Settings.Default.FormantFolder = this.formantFolderTextBox.Text;
            Properties.Settings.Default.Save();
            Close();
        }
        /// <summary>
        /// When the button to select the audio folder is clicked, opens a dialog allowing the user to navigate to their directory of choice.
        /// Once the user confirms their selection, updates the appropriate text box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void audioFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.audioFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// When the button to select the video folder is clicked, opens a dialog allowing the user to navigate to their directory of choice.
        /// Once the user confirms their selection, updates the appropriate text box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void videoFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.videoFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// When the button to select the recording folder is clicked, opens a dialog allowing the user to navigate to their directory of choice.
        /// Once the user confirms their selection, updates the appropriate text box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void recordingFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.recordingFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// When the button to select the report folder is clicked, opens a dialog allowing the user to navigate to their directory of choice.
        /// Once the user confirms their selection, updates the appropriate text box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void reportFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.reportFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// When the button to select the HTK folder is clicked, opens a dialog allowing the user to navigate to their directory of choice.
        /// Once the user confirms their selection, updates the appropriate text box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void HTKFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.HTKFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// When the button to select the Formant folder is clicked, opens a dialog allowing the user to navigate to their directory of choice.
        /// Once the user confirms their selection, updates the appropriate text box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void formantFolderSelectButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.formantFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// When the cancel button is clicked, close the form without saving folder changes.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
