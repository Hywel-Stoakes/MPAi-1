﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;
using System.Reflection;
using System.IO;
using MPAid.Models;
using MPAid.Cores;
using MPAid.Forms;

namespace MPAid.UserControls
{
    public partial class VlcPlayer : UserControl
    {
        public VlcControl VlcControl
        {
            get { return vlcControl; }
        }
        public VlcPlayer()
        {
            InitializeComponent();
        }
        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"VlcLibs\x86\"));
            else
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"VlcLibs\x64\"));

            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (vlcControl.State)
                {
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.NothingSpecial:
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped:
                        {
                            MainForm mainForm = this.Parent.Parent.Parent.Parent.Parent.Parent as MainForm;
                            Speaker spk = mainForm.RecordingList.SpeakerComboBox.SelectedItem as Speaker;
                            Word wd = mainForm.RecordingList.WordListBox.SelectedItem as Word;
                            Recording rd = mainForm.DBModel.Recording.Local.Where(x => x.WordId == wd.WordId && x.SpeakerId == spk.SpeakerId).SingleOrDefault();
                            if (rd != null)
                            {
                                SingleFile sf = rd.Video;
                                if (sf == null) throw new Exception("No video recording!");
                                string filePath = Path.Combine(sf.Address, sf.Name);

                                vlcControl.Play(new Uri(filePath));
                                playButton.ImageIndex = 2;
                            }
                            else
                            {
                                MessageBox.Show("Invalid recording!");
                            }
                        }
                        break;
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing:
                        {
                            vlcControl.Pause();
                            playButton.ImageIndex = 0;
                        }
                        break;
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused:
                        {
                            vlcControl.Pause();
                            playButton.ImageIndex = 2;
                        }
                        break;
                    default:
                        throw new Exception("Invalid state!");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                Console.WriteLine(exp);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            vlcControl.Stop();
        }

        private void OnVlcControlStopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            playButton.ImageIndex = 0;
        }

        private void playButton_MouseEnter(object sender, EventArgs e)
        {
            playButton.ImageIndex = vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing ? 1 : 3; 
        }

        private void playButton_MouseLeave(object sender, EventArgs e)
        {
            playButton.ImageIndex = vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing ? 0 : 2;
        }

        private void stopButton_MouseEnter(object sender, EventArgs e)
        {
            stopButton.ImageIndex = 1;
        }

        private void stopButton_MouseLeave(object sender, EventArgs e)
        {
            stopButton.ImageIndex = 0;
        }
    }
}
