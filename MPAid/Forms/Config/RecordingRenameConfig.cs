﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MPAid.Cores;
using NAudio.Wave;

namespace MPAid.Forms.Config
{
    /// <summary>
    /// Class for the form to rename an exsiting file according to the naming conventions of the recordings.
    /// </summary>
    public partial class RecordingRenameConfig : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecordingRenameConfig()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When the user selects the "..." button, opens a file picker to select a file to rename.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void filePickerButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.filenameTextBox.Text = openFileDialog.SafeFileName;
                    FileInfo fInfo = new FileInfo(openFileDialog.FileName);
                    if (!fInfo.Extension.Contains("wav")) this.testButton.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        /// <summary>
        /// When the rename button is clicked, passes all textbox values into the NameParser, then uses those values to change the file name.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void renameButton_Click(object sender, EventArgs e)
        {
            try
            {
                NamePaser paser = new NamePaser();
                paser.Address = Path.GetDirectoryName(openFileDialog.FileName);
                paser.Ext = Path.GetExtension(openFileDialog.FileName);
                paser.Speaker = this.speakerTextBox.Text;
                paser.Category = this.categoryTextBox.Text;
                paser.Word = this.wordTextBox.Text;
                paser.Label = this.labelTextBox.Text;

                File.Move(openFileDialog.FileName, paser.SingleFile);

                if (File.Exists(paser.SingleFile))
                {
                    this.filenameTextBox.Text = paser.FullName;
                    this.openFileDialog.FileName = paser.SingleFile;
                }
            }
            catch (Exception exp)
            {
                if (exp.GetType() == typeof(FileNotFoundException))
                {
                    MessageBox.Show(exp.Message, "No such file!");
                }
                else if (exp.GetType() == typeof(IOException))
                {
                    MessageBox.Show(exp.Message, "Destination file already exists!");
                }
            }
        }
        /// <summary>
        /// When the test button is clicked, plays the selected file. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(this.openFileDialog.FileName))
                {
                    MainForm.self.OperationPanel.NAudioRecorder.AudioPlayer.Play(this.openFileDialog.FileName);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "NAudio playing error!");
            }
        }
    }
}
