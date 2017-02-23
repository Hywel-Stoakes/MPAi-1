using MPAi.Components;
using MPAi.Cores;
using MPAi.Cores.Scoreboard;
using MPAi.DatabaseModel;
using MPAi.Forms.Popups;
using MPAi.Modules;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MPAi.Forms
{

    public partial class SpeechRecognitionTest : Form, MainFormInterface
    {
        // Strings kept in fields to make text easier to change.
        private string optionsLess = "Less...";
        private string optionsMore = "More...";
        private string playText = "Play";
        private string stopText = "Stop";
        private string recordText = "Record";

        private string formatErrorText = "A problem was encountered during recording {0}";
        private string couldntDeleteRecordingText = "Could not delete recording";
        private string noCurrentFileText = "No current file";
        private string recordingNotSelectedText = "Please select a recording.";
        private string warningText = "Warning";
        private string noAudioDeviceText = "No audio device plugged in.";

        private string outputFileName;
        private string outputFolder;
        private string tempFilename;
        private string tempFolder;

        private IWaveIn waveIn;
        private WaveFileWriter writer;

        private HTKEngine RecEngine = new HTKEngine();
        private MPAiSpeakScoreBoardSession session;
        private NAudioPlayer audioPlayer = new NAudioPlayer();

        private int bottomHeight;

        private bool appClosing = true;

        bool dataEvent = false;

        public NAudioPlayer AudioPlayer
        {
            get { return audioPlayer; }
        }

        /// <summary>
        /// Default constructor. Sets up combo boxes, and creates required directories.
        /// </summary>
        public SpeechRecognitionTest()
        {
            InitializeComponent();
            LoadWasapiDevices();
            CreateDirectory();
            DataBinding();
            populateBoxes();
            bottomHeight = SpeechRecognitionTestPanel.Height - SpeechRecognitionTestPanel.SplitterDistance;
            toggleOptions();    // For development, the bottom panel is visible, but the user won't need the bottom panel most of the time.
            toggleListButtons(RecordingListBox.SelectedItems.Count > 0);
            session = UserManagement.CurrentUser.SpeakScoreboard.NewScoreBoardSession();

            ensureScoreReportButtonCorrectlyEnabled();
        }

        /// <summary>
        /// When the user changes their voice settings, take this action.
        /// </summary>
        public void userChanged()
        {
            populateBoxes();
        }

        /// <summary>
        /// Gets the words from the database.
        /// </summary>
        private void populateBoxes()
        {
            try
            {
                // Create new database context.
                using (MPAiModel DBModel = new MPAiModel())
                {
                    DBModel.Database.Initialize(false); // Added for safety; if the database has not been initialised, initialise it.

                    MPAiUser current = UserManagement.CurrentUser;
                    Console.WriteLine(VoiceType.getDisplayNameFromVoiceType(current.Voice));

                    List<Word> view = DBModel.Word.Where(x => (
                       x.Category.Name.Equals("Word")
                       && x.Recordings.Any(y => y.Speaker.SpeakerId == current.Speaker.SpeakerId)
                       )).ToList();

                    view.Sort(new VowelComparer());
                    WordComboBox.DataSource = new BindingSource() { DataSource = view };
                    WordComboBox.DisplayMember = "Name";
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// If the recordings folder doesn't already exist, creates it.
        /// Creates a temporary directory,to be used for recording purposes. 
        /// </summary>
        public void CreateDirectory()
        {
            outputFolder = DirectoryManagement.RecordingFolder;
            tempFolder = AppDataPath.Temp;
            Directory.CreateDirectory(outputFolder);
        }

        /// <summary>
        /// Gets all the audio input devices attached to the system, converts them to a list, and populates the audio device combo box.
        /// </summary>
        public void LoadWasapiDevices()
        {
            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();

            AudioInputDeviceComboBox.DataSource = devices.Count == 0 ? null : devices;
            AudioInputDeviceComboBox.DisplayMember = "FriendlyName";
        }

        /// <summary>
        /// Clears the list box that holds all the recordings, and repopulates it with all valid recordings in the recordings directory.
        /// Used for initialisation and to refresh values.
        /// </summary>
        public void DataBinding()
        {
            RecordingListBox.Items.Clear();
            DirectoryInfo info = new DirectoryInfo(DirectoryManagement.RecordingFolder);
            RecordingListBox.Items.AddRange(info.GetFiles().Where(
                x => x.Extension != ".mfc"  // Not an mfc file
                && x.Name.EndsWith(".wav")  // Is a .wav file
                && !x.Name.ToCharArray().Any(c => c > 127)   // Contains no unicode characters
            ).Select(x => x.Name).ToArray());
            toggleListButtons(RecordingListBox.Items.Count > 0);
        }

        /// <summary>
        /// Event to handle audio buffering and updating of the progress bar.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (InvokeRequired) // If it is necessary to invoke this method on a separate thread
            {
                BeginInvoke(new EventHandler<WaveInEventArgs>(OnDataAvailable), sender, e); // Send this event to the relevant thread.
            }
            else
            {
                if (recordButton.Text.Equals(recordText))
                {
                    return;
                }
                writer.Write(e.Buffer, 0, e.BytesRecorded); // Record audio into a buffer
                int secondsRecorded = (int)(writer.Length / writer.WaveFormat.AverageBytesPerSecond);
                recordingProgressBar.Value = secondsRecorded * 10;  // Increase the progress bar
                if (secondsRecorded >= 10)
                {
                    StopRecording(); // If we have recorded more than 10s of audio then stop recording
                }
            }
        }

        /// <summary>
        /// Tidies up the stream after a wave file has been played or recorded.
        /// </summary>
        /// <param name="s">The stream to dispose of.</param>
        private void FinalizeWaveFile(Stream s)
        {
            if (s != null)
            {
                s.Dispose();
                s = null;
            }
        }

        /// <summary>
        /// Handles the state of the buttons based on whether or not the user is recording.
        /// </summary>
        /// <param name="isRecording"></param>
        private void SetControlStates(bool isRecording)
        {
            recordButton.Text = isRecording ? stopText : recordText;
            playButton.Enabled = !isRecording;
            analyzeButton.Enabled = !isRecording;
            recordingProgressBar.Visible = isRecording;
            recordingProgressBarLabel.Visible = !isRecording;
            SpeechRecognitionTestPanel.Panel2.Enabled = !isRecording;
        }

        /// <summary>
        /// If a file is being recorded, stop recording and tidy up the stream.
        /// </summary>
        private void StopRecording()
        {
            recordButton.Text = recordText;
            if (dataEvent)
            {
                waveIn.DataAvailable -= OnDataAvailable;
                dataEvent = false;
            }
            if (waveIn != null)
            {
                waveIn.StopRecording();
            }
            FinalizeWaveFile(writer);
        }

        /// <summary>
        /// If a file is being played, stop playback and tidy up the stream.
        /// </summary>
        private void StopPlay()
        {
            AudioPlayer.Stop();
        }

        /// <summary>
        /// Converts the temporary file created by recording into the format used by the recording files.
        /// </summary>
        private void Resample()
        {
            try
            {
                using (var reader = new WaveFileReader(Path.Combine(tempFolder, tempFilename))) // Read audio out of a temporary file in the temporary folder.
                {
                    var outFormat = new WaveFormat(16000, reader.WaveFormat.Channels);      // Define the output format of the audio
                    // Create the sampler that interprets the audio file into the format use the resampler to create the .wav file in the recordings directory.
                    using (var resampler = new MediaFoundationResampler(reader, outFormat))
                    {
                        WaveFileWriter.CreateWaveFile(Path.Combine(outputFolder, outputFileName), resampler);
                    }
                }
                File.Delete(Path.Combine(tempFolder, tempFilename));    // Delete the temporary file.
            }
            catch(IOException exp)
            {
                // If there is an error recording, finalise everything, but don't update the recording.
                if (dataEvent)
                {
                    waveIn.DataAvailable -= OnDataAvailable;
                    dataEvent = false;
                }
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                }
                FinalizeWaveFile(writer);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Functionality to tidy up when recording has stopped.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (InvokeRequired) // If it is necessary to invoke this on a different thread
            {
                BeginInvoke(new EventHandler<StoppedEventArgs>(OnRecordingStopped), sender, e); // Send this event to the relevant thread
            }
            else
            {
                Resample();
                recordingProgressBarLabel.Text = outputFileName;
                int newItemIndex = RecordingListBox.Items.Add(outputFileName);    // Add the new audio file to the list box
                RecordingListBox.SelectedIndex = newItemIndex;    // And select it
                SetControlStates(false);    // Toggle the record and stop buttons
                if (e.Exception != null)
                {
                    DeleteFile();   // Remove the now erroneous file.
                    MPAiMessageBoxFactory.Show(string.Format(formatErrorText, e.Exception.Message));
                }
            }
        }

        /// <summary>
        /// Functionality for the "More" and "Less" buttons.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void optionsButton_Click(object sender, EventArgs e)
        {
            toggleOptions();
        }

        /// <summary>
        /// Shows/hides the options panel, changes button text, and resizes the form as appropriate.
        /// </summary>
        private void toggleOptions()
        {
            if (SpeechRecognitionTestPanel.Panel2Collapsed)
            {
                Height += bottomHeight;
                MinimumSize = new Size(MinimumSize.Width, 600);
                optionsButton.Text = optionsLess;
            }
            else
            {
                MinimumSize = new Size(MinimumSize.Width, 300);
                bottomHeight = SpeechRecognitionTestPanel.Height - SpeechRecognitionTestPanel.SplitterDistance;
                Height -= bottomHeight;
                optionsButton.Text = optionsMore;
            }
            SpeechRecognitionTestPanel.Panel2Collapsed = !SpeechRecognitionTestPanel.Panel2Collapsed;
        }

        /// <summary>
        /// Functionality for the refresh devices button.
        /// Reloads any attached devices into the combo box, and refreshes it's value.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void AudioInputDeviceButton_Click(object sender, EventArgs e)
        {
            LoadWasapiDevices();
            DataBinding();
        }

        /// <summary>
        /// Functionality for the delete button.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteFile();
        }

        /// <summary>
        /// Deletes the file selected in the list box from the user's computer.
        /// </summary>
        void DeleteFile()
        {
            if (RecordingListBox.SelectedItem != null)
            {
                try
                {
                    StopRecording();
                    StopPlay();
                    File.Delete(Path.Combine(outputFolder, (string)RecordingListBox.SelectedItem));
                    if (recordingProgressBarLabel.Text.Equals((string)RecordingListBox.SelectedItem))
                    {
                        recordingProgressBarLabel.Text = noCurrentFileText;
                    }
                    RecordingListBox.Items.Remove(RecordingListBox.SelectedItem);
                    if (RecordingListBox.Items.Count > 0)
                    {
                        RecordingListBox.SelectedIndex = RecordingListBox.Items.Count - 1;
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                    MPAiMessageBoxFactory.Show(couldntDeleteRecordingText);
                }
            }
            else
            {
                MPAiMessageBoxFactory.Show(recordingNotSelectedText);
            }
            // If no items remain, disable buttons relating to them.
            if (RecordingListBox.Items.Count < 1)
            {
                toggleListButtons(false);
            }
        }

        /// <summary>
        /// Functionality for the Analyse button.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void analyzeButton_Click(object sender, EventArgs e)
        {
            analyse();
            ensureScoreReportButtonCorrectlyEnabled();
        }

        /// <summary>
        /// Passes the selected item to the speech recognition software.
        /// </summary>
        void analyse()
        {
            try
            {
                if (recordingProgressBarLabel.Text.ToCharArray().Any(c => c > 127))
                {
                    // Then the file name contains a unicode character
                    MPAiMessageBoxFactory.Show("MPAi cannot support filenames with unicode characters. Please rename " + recordingProgressBarLabel.Text + " and try again.");
                    return;
                }
                if (!recordingProgressBarLabel.Text.Equals(noCurrentFileText))
                {

                    string target = ((WordComboBox.SelectedItem as Word) == null) ? string.Empty : (WordComboBox.SelectedItem as Word).Name;
                    Dictionary<string, string> result = RecEngine.Recognize(Path.Combine(outputFolder, recordingProgressBarLabel.Text)).ToDictionary(x => x.Key, x => x.Value);
                    if (result.Count > 0)
                    {
                        MPAiSpeakScoreBoardItem item;
                        AnalysisScreen analysisScreen;
                        item = new MPAiSpeakScoreBoardItem(target, result.First().Value, PronuciationAdvisor.Advise(result.First().Key, target, result.First().Value), recordingProgressBarLabel.Text);

                        if (!UserManagement.CurrentUser.SpeakScoreboard.IsRecordingAlreadyAnalysed(recordingProgressBarLabel.Text))
                        {
                            session.Content.Add(item);
                            analysisScreen = new AnalysisScreen(item.Similarity, item.Analysis);
                        }
                        else
                        {
                            analysisScreen = new AnalysisScreen(item.Similarity, item.Analysis, true);
                        }  
                        analysisScreen.ShowDialog(this);
                    }
                    else
                    {
                        MPAiMessageBoxFactory.Show("There was a error while analysing this recording.\nHTK Engine did not return a match between the recording and a word.\nIf this problem persist, reinstall MPAi");
                    }
                }
            }
            catch (Exception exp)
            {
#if DEBUG
                MPAiMessageBoxFactory.Show(exp.Message, warningText, MPAiMessageBoxButtons.OK);
#endif
            }
        }

        private void ensureScoreReportButtonCorrectlyEnabled()
        {
            if (UserManagement.CurrentUser.SpeakScoreboard.IsEmpty())
            {
                ScoreReportButton.Enabled = false;
            }
            else
            {
                ScoreReportButton.Enabled = true;
            }

        }

        /// <summary>
        /// Functionality for the Show Report button.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void ScoreReportButton_Click(object sender, EventArgs e)
        {
            generateReport();
        }

        /// <summary>
        /// Uses the Report Launcher to create the scoreboard, and launches it in a browser for the user.
        /// </summary>
        void generateReport()
        {
            ReportLauncher.GenerateMPAiSpeakScoreHTML(UserManagement.CurrentUser.SpeakScoreboard);
            if (File.Exists(ReportLauncher.MPAiSpeakScoreReportHTMLAddress))
            {
                ReportLauncher.ShowMPAiSpeakScoreReport();
            }
        }

        /// <summary>
        /// Functionality for play button. Plays the selected audio file.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void playButton_Click(object sender, EventArgs e)
        {
            if (playButton.Text.Equals(playText))
            {
                AudioPlayer.Play(Path.Combine(outputFolder, recordingProgressBarLabel.Text));
                AudioPlayer.WaveOut.PlaybackStopped += playButton_Click;    // Subscribe to playback stopped.
                playButton.Text = stopText;
            }
            else    // It must equal "Stop" if not "Play"
            {
                AudioPlayer.WaveOut.PlaybackStopped -= playButton_Click;    // Unsubscribe from playback stopped.
                StopPlay();
                playButton.Text = playText;
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void backButton_Click(object sender, EventArgs e)
        {
            new MPAiSpeakMainMenu().Show();
            closeThis();
        }

        /// <summary>
        /// Closes the form, but not the application.
        /// </summary>
        public void closeThis()
        {
            appClosing = false; // Tell the FormClosing event not to end the program.
            Close();
        }

        /// <summary>
        /// Updates the value in the analysis label.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void SelectButton_Click(object sender, EventArgs e)
        {
            recordingProgressBarLabel.Text = RecordingListBox.GetItemText(RecordingListBox.SelectedItem);
        }

        /// <summary>
        /// Disables or enables the buttons relating to the list of recordings.
        /// </summary>
        /// <param name="enable">Whether to enable (true) or disable (false) the buttons.</param>
        private void toggleListButtons(bool enable)
        {
            SelectButton.Enabled = enable;
            DeleteButton.Enabled = enable;
        }

        /// <summary>
        /// Disables or enables the buttons relating to the current recording.
        /// </summary>
        /// <param name="enable">Whether to enable (true) or disable (false) the buttons.</param>
        private void toggleRecordingButtons(bool enable)
        {
            playButton.Enabled = enable;
            analyzeButton.Enabled = enable;
        }

        /// <summary>
        /// Handles changing buttons based on the curent recording.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void recordingProgressBarLabel_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(recordingProgressBarLabel.Text))
            {
                recordingProgressBarLabel.Text = noCurrentFileText;
            }
            toggleRecordingButtons(!recordingProgressBarLabel.Text.Equals(noCurrentFileText));
        }

        /// <summary>
        /// Handles functionality of the record/stop button, calling appropriate methods and changing it's text.
        /// As there is no global isRecording field, and such a field would interfere with existing code, the button text is used as the recording status.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void recordButton_Click(object sender, EventArgs e)
        {
            if (recordButton.Text.Equals(recordText))
            {
                record();
            }
            else
            {
                StopRecording();
                StopPlay();
            }
        }

        /// <summary>
        /// Sets up the audio device, and the file to record into, adds listeners to the events, starts recording, and toggles the buttons.
        /// </summary>
        private void record()
        {
            try
            {
                var device = (MMDevice)AudioInputDeviceComboBox.SelectedItem;
                if (!(device == null))
                {

                    recordButton.Text = stopText;
                    recordingProgressBar.Value = 0;

                    device.AudioEndpointVolume.Mute = false;
                    // Use wasapi by default
                    waveIn = new WasapiCapture(device);
                    waveIn.DataAvailable += OnDataAvailable;
                    dataEvent = true;   // Track whether an event is subscribed to. 
                    waveIn.RecordingStopped += OnRecordingStopped;

                    tempFilename = String.Format("{0}-{1:yyy-MM-dd-HH-mm-ss}.wav", UserManagement.CurrentUser.getName(), DateTime.Now);
                    // Initially, outputname is the same as tempfilename
                    outputFileName = tempFilename;
                    writer = new WaveFileWriter(Path.Combine(tempFolder, tempFilename), waveIn.WaveFormat);
                    waveIn.StartRecording();
                    SetControlStates(true);
                }
                else
                {
                    recordButton.Text = recordText;
                    MPAiMessageBoxFactory.Show(noAudioDeviceText, warningText, MPAiMessageBoxButtons.OK);
                }
            }
            catch (Exception exp)
            {
#if DEBUG
                MPAiMessageBoxFactory.Show(exp.Message, warningText, MPAiMessageBoxButtons.OK);
#endif
            }
        }

        /// <summary>
        /// Functionality for Add button.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            add();

        }

        /// <summary>
        /// Opens a file picker, then for each file picked by the user, prompts them to rename the file, and places it in the recording list.
        /// </summary>
        private void add()
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string f in openFileDialog.FileNames)
                    {
                        if (f.ToCharArray().Any(c => c > 127))
                        {
                            // Then the file name contains a unicode character
                            MPAiMessageBoxFactory.Show("MPAi cannot support filenames with unicode characters. Please rename " + f + " and try again.");
                            continue;
                        }
                        else
                        {
                            string[] fArray = f.Split(Path.DirectorySeparatorChar); // Split on file separator 
                            string fShort = fArray[fArray.GetLength(0) - 1];        // Get the last index, which corresponds to just the file name
                            File.Copy(f, Path.Combine(outputFolder, fShort));       // Copy file into recordings folder.
                        }
                    }
                    DataBinding();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.StackTrace);

            }
        }

        /// <summary>
        /// If no recording is selected, disable the buttons that depend on the selected recording.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void RecordingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            toggleListButtons(RecordingListBox.SelectedItems.Count > 0);
        }

        /// <summary>
        /// Fires when the form closes. 
        /// If the user pressed the back button, the next form will be loaded. 
        /// If the user closed the form in some other way, the app will temrinate.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void SpeechRecognitionTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopPlay(); // The audio player runs threads in the background. To prevent them running after the form closes, call stop on close.

            if (appClosing)
            {
                UserManagement.WriteSettings();
                DirectoryManagement.WritePaths();
                Application.Exit();
            }
        }
    }
}
