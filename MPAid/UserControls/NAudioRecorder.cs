﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.IO;
using System.Diagnostics;
using MPAid.Models;
using MPAid.Cores;

namespace MPAid.UserControls
{
    public partial class NAudioRecorder : UserControl
    {
        private IWaveIn waveIn;
        private WaveOutEvent waveOut;
        private WaveFileWriter writer;
        private WaveFileReader reader;
        private string outputFileName;
        private string outputFolder;
        private string tempFilename;
        private string tempFolder;
        private HTKEngine RecEngine;
        public NAudioRecorder()
        {
            InitializeComponent();

            LoadWasapiDevices();

            waveOut = new WaveOutEvent();
            RecEngine = new HTKEngine();
        }

        private void LoadWasapiDevices()
        {
            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();

            audioDeviceComboBox.DataSource = devices;
            audioDeviceComboBox.DisplayMember = "FriendlyName";
        }

        public void CreateDirectory()
        {
            MainForm mainForm = MainForm.self;
            outputFolder = mainForm.configContent.RecordingFolderAddr.FolderAddr;
            tempFolder = Path.Combine(Path.GetTempPath(), "MPAidTemp");
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(tempFolder);
        }

        public void DataBinding()
        {
            DirectoryInfo info = new DirectoryInfo(MainForm.self.configContent.RecordingFolderAddr.FolderAddr);
            RECListBox.Items.AddRange(info.GetFiles().Where(x => x.Extension != ".mfc").Select(x => x.Name).ToArray());
        }
        private void FinalizeWaveFile(Stream s)
        {
            if (s != null)
            {
                s.Dispose();
                s = null;
            }
        }

        private void SetControlStates(bool isRecording)
        {
            recordButton.Enabled = !isRecording;
            fromFileButton.Enabled = !isRecording;
            stopButton.Enabled = isRecording;
        }

        private double CalculateScore()
        {
            MainForm mainForm = Parent.Parent.Parent.Parent.Parent.Parent as MainForm;
            String word = (mainForm.RecordingList.WordListBox.SelectedItem as Word).Name;
            int total = RECListBox.Items.Count;
            double score = -1;
            if (total > 0)
            {
                int i = 0;

                foreach (var item in RECListBox.Items)
                    if (new Examiner(item.ToString(), word).wordsMatch())
                        i += 1;
                score = i / total;
            }
            return score;
        }

        private void StopRecording()
        {
            if (waveIn != null) waveIn.StopRecording();
            FinalizeWaveFile(writer);
        }

        private void StopPlay()
        {
            if (waveOut != null) waveOut.Stop();
            FinalizeWaveFile(reader);
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (InvokeRequired)
            {
                //Debug.WriteLine("Data Available");
                BeginInvoke(new EventHandler<WaveInEventArgs>(OnDataAvailable), sender, e);
            }
            else
            {
                //Debug.WriteLine("Flushing Data Available");
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                int secondsRecorded = (int)(writer.Length / writer.WaveFormat.AverageBytesPerSecond);
                if (secondsRecorded >= 30)
                {
                    StopRecording();
                }
                else
                {
                    recordingProgressBar.Value = secondsRecorded;
                }
            }
        }

        private void Resample()
        {
            try
            {
                using (var reader = new WaveFileReader(Path.Combine(tempFolder, tempFilename)))
                {
                    var outFormat = new WaveFormat(16000, reader.WaveFormat.Channels);
                    using (var resampler = new MediaFoundationResampler(reader, outFormat))
                    {
                        // resampler.ResamplerQuality = 60;
                        WaveFileWriter.CreateWaveFile(Path.Combine(outputFolder, outputFileName), resampler);
                    }
                }
                File.Delete(Path.Combine(tempFolder, tempFilename));
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<StoppedEventArgs>(OnRecordingStopped), sender, e);
            }
            else
            {
                Resample();
                recordingProgressBar.Value = 0;
                if (e.Exception != null)
                {
                    MessageBox.Show(String.Format("A problem was encountered during recording {0}",
                                                  e.Exception.Message));
                }

                int newItemIndex = RECListBox.Items.Add(outputFileName);
                RECListBox.SelectedIndex = newItemIndex;
                SetControlStates(false);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (RECListBox.SelectedItem != null)
            {
                try
                {
                    stopButton_Click(sender, e);
                    File.Delete(Path.Combine(outputFolder, (string)RECListBox.SelectedItem));
                    RECListBox.Items.Remove(RECListBox.SelectedItem);
                    if (RECListBox.Items.Count > 0)
                    {
                        RECListBox.SelectedIndex = 0;
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                    MessageBox.Show("Could not delete recording");
                }
            }
        }

        private void RECListBox_DoubleClick(object sender, EventArgs e)
        {
            if (RECListBox.SelectedItem != null)
            {
                reader = new WaveFileReader(Path.Combine(outputFolder, (string)RECListBox.SelectedItem));
                if (waveOut.PlaybackState != PlaybackState.Playing)
                {
                    waveOut.Init(reader);
                    waveOut.Play();
                }
            }
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            var device = (MMDevice)audioDeviceComboBox.SelectedItem;
            device.AudioEndpointVolume.Mute = false;
            //use wasapi by default
            waveIn = new WasapiCapture(device);
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;

            tempFilename = String.Format("{0}-{1:yyy/MM/dd/HH/mm/ss}.wav", MainForm.self.AllUsers.getCurrentUser().getName(), DateTime.Now);
            //initially, outputname is the same as tempfilename
            outputFileName = tempFilename;
            writer = new WaveFileWriter(Path.Combine(tempFolder, tempFilename), waveIn.WaveFormat);
            waveIn.StartRecording();
            SetControlStates(true);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopRecording();
            StopPlay();
        }

        private void fromFileButton_Click(object sender, EventArgs e)
        {
            Process.Start(outputFolder);
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (RECListBox.SelectedItem != null)
                {
                    RecEngine.Recognize(Path.Combine(outputFolder, (string)RECListBox.SelectedItem));
                }
                
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
            }

            correctnessLabel.Text = "Correctness = " + CalculateScore().ToString();
        }

        private void showReportButton_Click(object sender, EventArgs e)
        {
            MainForm mainForm = Parent.Parent.Parent.Parent.Parent.Parent as MainForm;

            String word = (mainForm.RecordingList.WordListBox.SelectedItem as Word).Name;
            HtmlConfig hConfig = new HtmlConfig(mainForm.configContent.ReportFolderAddr.FolderAddr)
            {
                myWord = word,
                correctnessValue = CalculateScore().ToString()
            };

            if ((RECListBox.Items != null) && (RECListBox.Items.Count > 0))
            {
                string[] wordArray = new string[RECListBox.Items.Count];
                RECListBox.Items.CopyTo(wordArray, 0);
                hConfig.listRecognized = wordArray.ToList();
            }


            HtmlGenerator htmlWriter = new HtmlGenerator(hConfig);
            htmlWriter.Run();

            // Show the HTML file in system browser
            String reportPath = hConfig.GetHtmlFullPath();
            if (File.Exists(reportPath))
            {
                Process browser = new Process();
                browser.StartInfo.FileName = reportPath;
                browser.Start();
            }
            else
                showReportButton.Enabled = false;
        }

        private void AudioRecognize()
        {
            MainForm mainForm = Parent.Parent.Parent.Parent.Parent.Parent as MainForm;
            String word = (mainForm.RecordingList.WordListBox.SelectedItem as Word).Name;

            PaConfig config = new PaConfig()
            {
                currentWord = word,
                AnnieDir = mainForm.configContent.AnnieFolderAddr.FolderAddr,
                batFilePath = Path.Combine(mainForm.configContent.AnnieFolderAddr.FolderAddr, "Process.bat")
            };

            if ((RECListBox.Items != null) && (RECListBox.Items.Count > 0))
            {
                string[] wordArray = new string[RECListBox.Items.Count];
                RECListBox.Items.CopyTo(wordArray, 0);
                config.audioList = wordArray.ToList();
            }

            PaEngine engine = new PaEngine(config);
            if (engine.wavFilesOK())
            {
                // The Main thread will wait until the process finishes
                engine.Start();

                MessageBox.Show("The result is " + engine.GetRecognizedWord());

                //copies the user recording files to the HTML report resource folder
                //HtmlConfig hConfig = new HtmlConfig(mainForm.configContent.ReportFolderAddr.FolderAddr);
                //File.Copy(Path.Combine(mainForm.configContent.RecordingFolderAddr.FolderAddr, outputFileName),
                //          hConfig.GetRecPath(RECListBox.Items.Count, HtmlConfig.pathType.fullUserRecPath), 
                //          true);

                //prepare to copy the sample recording file to the HTML report res folder

                //make sure the sample recording is different from each other
                //string soundToPlay = null;
                //int counter = 0;
                //do
                //{
                //    counter += 1;
                //    soundToPlay = ResMan.GetWordSound(GetAudioSource(), word.WordSoundId, true);
                //    if (soundToPlay == null)
                //        break;
                //} while ((soundToPlay == lastPlayedSound) && (counter < 255));

                //lastPlayedSound = soundToPlay;

                ////copies the sample recording files to the HTML res folder
                //ResMan.SuperCopy(soundToPlay, hConfig.GetRecPath(listREC.Count,
                //    HtmlConfig.pathType.fullSampleRecPath), true);

                //change the UI
                showReportButton.Enabled = true;
            }

            //ResetRecordings();
        }

        private void RECListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            analyzeButton.Enabled = (sender as ListBox).SelectedItem != null;
            deleteButton.Enabled = (sender as ListBox).SelectedItem != null;
        }
    }
}
