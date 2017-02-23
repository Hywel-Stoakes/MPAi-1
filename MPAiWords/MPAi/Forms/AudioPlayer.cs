using MPAi.Components;
using MPAi.Cores;
using MPAi.DatabaseModel;
using MPAi.Modules;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace MPAi.Forms
{
    public partial class AudioPlayer : Form, MainFormInterface
    {
        // Strings kept in fields to make text easier to change.
        private string optionsLess = "Less...";
        private string optionsMore = "More...";
        private string recordText = "Record";
        private string stopText = "Stop";

        private string noFileText = "No current file";
        private string myRecordingText = "My new recording";
        private string selectFolderString = "Select Vlc libraries folder.";

        private string invalidStateString = "Invalid State!";
        private string invalidRecordingString = "Invalid recording!";
        private string formatErrorText = "A problem was encountered during recording {0}";
        private string warningText = "Warning";
        private string noAudioDeviceText = "No audio device plugged in.";

        private string[] spinnerText = { "Repeat Forever", "Repeat 10 Times", "Repeat 9 Times", "Repeat 8 Times", "Repeat 7 Times", "Repeat 6 Times", "Repeat 5 Times", "Repeat 4 Times", "Repeat 3 Times", "Repeat Twice", "Repeat Once", "Don't Repeat" };

        private string audioFilePath = Path.Combine(AppDataPath.Temp, "AudioPlayerRecordedAudio.wav");
        private WaveFileWriter writer;
        private WasapiCapture waveIn;

        // Used to keep track of the currently playing file.
        private string filePath;

        // Delegate required for VLC Player.
        delegate void delegatePlayer(Uri file, string[] pars);
        delegate void delegateStopper();

        // The list of recordings to play.
        private List<Word> wordsList;
        // The index of the current recording.
        private int currentRecordingIndex = 0;

        // Variables used to determine the number of times to repeat a video.
        private int repeatTimes = 0;
        private int repeatsRemaining = 0;

        /// <summary>
        /// Holds the height of the bottom panel between clicks of the options button.
        /// </summary>
        private readonly int bottomHeight;

        // Used to determine if the entire application is closing, or just the form.
        private bool appClosing = true;
        private bool playRecording;
        private bool onDataAvailableSubscribed = false;

        /// <summary>
        /// Wrapper propety for repeatTimes, also prevents too many repeats by updating repeatsRemaining.
        /// </summary>
        public int RepeatTimes
        {
            get
            {
                return repeatTimes;
            }

            set
            {
                repeatTimes = value;
                // If the control is not playing, update repeats remaining. Implicitly, if the control is playing, let it finish it's current repeat cycle.
                if (!(vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing) || vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Opening) || vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused) || vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Buffering)))
                {
                    repeatsRemaining = value;
                }
            }
        }

        public AudioPlayer()
        {
            InitializeComponent();
            populateBoxes();
            setUpSpinner();
            LoadWasapiDevices();

            bottomHeight = VideoPlayerPanel.Height - VideoPlayerPanel.SplitterDistance;
            toggleOptions();    // For development, the bottom panel is visible, but the user won't need the bottom panel most of the time.
        }

        /// <summary>
        /// When the user changes their voice settings, take this action.
        /// </summary>
        public void userChanged()
        {
            populateBoxes();
        }

        delegate void SetProgressCallback(int value);

        /// <summary>
        /// Threadsafe way to invoke progress updates
        /// </summary>
        /// <param name="value"></param>
        private void SetProgress(int value)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProgressCallback d = new SetProgressCallback(SetProgress);
                try
                {
                    this.Invoke(d, new object[] { value });
                } catch (ObjectDisposedException)
                {
                    //Do Nothing
                }
            }
            else
            {
                while(vlcControl.GetCurrentMedia() == null) { }

                if(vlcControl.GetCurrentMedia().Duration.TotalMilliseconds < 10000)
                {
                    if (value >= 100)
                    {
                        this.progressBar1.Value = 100;
                        this.progressBar1.Value = 99;
                        this.progressBar1.Value = 100;
                    }
                    else if (value <= 0)
                    {
                        this.progressBar1.Value = 0;
                    }
                    else
                    {
                        this.progressBar1.Value = value + 1;
                        this.progressBar1.Value = value;
                    }
                } else
                {
                    this.progressBar1.Value = value;
                }
            }
        }

        /// <summary>
        /// Set up repeat spinner programatically. Visual Studio tends to delete values when set up in the designer, and some values can't be set in form properties.
        /// </summary>
        private void setUpSpinner()
        {
            repeatSpinner.SelectedIndex = repeatSpinner.Items.Count - 1;
            repeatSpinner.Items.AddRange(spinnerText);
            repeatSpinner.SelectedIndex = repeatSpinner.Items.Count - 1;
            repeatSpinner.SelectedItemChanged += repeatSpinner_SelectedItemChanged;
        }

        /// <summary>
        /// Gets the words from the database.
        /// </summary>
        private void populateBoxes()
        {
            // Stop playback and clear the boxes, to prevent errors.
            asyncStop();
            WordComboBox.Items.Clear();
            soundListCurrentListBox.Items.Clear();
            try
            {
                // Create new database context.
                using (MPAiModel DBModel = new MPAiModel())
                {
                    DBModel.Database.Initialize(false); // Added for safety; if the database has not been initialised, initialise it.

                    MPAiUser current = UserManagement.CurrentUser;

                    List<Word> view = DBModel.Word.Where(x => (
                       x.Category.Name.Equals("Word")
                       && x.Recordings.Any(y => y.Speaker.SpeakerId == current.Speaker.SpeakerId)  // Until the Menubar is finished, this won't work. Comment this line out to test.
                       )).ToList();

                    // Can't sort a control's Items field, so we sort a list and add values.
                    view.Sort(new VowelComparer());

                    // Lists of Word objects, but only their name needs to be displayed to the user.
                    soundListAllListBox.DisplayMember = "Name";
                    WordComboBox.DisplayMember = "Name";
                    soundListCurrentListBox.DisplayMember = "Name";

                    // Set the values in all the lists used by the program.
                    soundListAllListBox.DataSource = new BindingSource() { DataSource = view };
                    foreach (Word wd in view)
                    {
                        soundListCurrentListBox.Items.Add(wd);
                        WordComboBox.Items.Add(wd);
                    }
                    wordsList = view;

                    selectItemInComboBox();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Changing the index of the combo box will cause a different word to be played by the audio player.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void WordComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRecordingIndex = WordComboBox.SelectedIndex;
            // If the video is partway through playing, stop it.
            if (vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
                || vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Opening)
                || vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Buffering)
                || vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused))
            {
                asyncStop();
            }
            if (!vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped))   // Unless the form has just loaded, play audio.
            {
                playAudio();
            }
        }

        /// <summary>
        /// Handles the functionality for the options button.
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
            if (VideoPlayerPanel.Panel2Collapsed)
            {
                Height += bottomHeight - 3;
                MinimumSize = new Size(MinimumSize.Width, 450);
                optionsButton.Text = optionsLess;
                VideoPlayerPanel.Panel2Collapsed = !VideoPlayerPanel.Panel2Collapsed;
            }
            else
            {
                VideoPlayerPanel.Panel2Collapsed = !VideoPlayerPanel.Panel2Collapsed;
                MinimumSize = new Size(MinimumSize.Width, 225);
                Height -= bottomHeight - 3;
                optionsButton.Text = optionsMore;
            }
        }

        /// <summary>
        /// Handles the functionality for the back button.
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
            asyncStop();
            StopRecording();
            Close();
        }

        /// <summary>
        /// When the track bar's value changes, also change the spinner to match.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void repeatTrackBar_ValueChanged(object sender, EventArgs e)
        {
            RepeatTimes = repeatTrackBar.Value;
            repeatSpinner.SelectedIndex = (repeatSpinner.Items.Count - 1) - RepeatTimes;
        }

        /// <summary>
        /// When the spinner changes, also change the track bar to match.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void repeatSpinner_SelectedItemChanged(object sender, EventArgs e)
        {
            RepeatTimes = (repeatSpinner.Items.Count - 1) - repeatSpinner.SelectedIndex;
            repeatTrackBar.Value = RepeatTimes;
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
        /// Functionality for the refresh devices button.
        /// Reloads any attached devices into the combo box, and refreshes it's value.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void AudioInputDeviceButton_Click(object sender, EventArgs e)
        {
            LoadWasapiDevices();
        }

        /// <summary>
        /// Calls a dialog box for the user to select a file to overlay.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void addFromFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.Copy(openFileDialog.FileName, audioFilePath, true);
                recordingProgressBarLabel.Text = openFileDialog.FileName;
                removeButton.Enabled = true;
            }
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
            }
        }

        /// <summary>
        /// If a file is being recorded, stop recording and tidy up the stream.
        /// </summary>
        private void StopRecording()
        {
            if (onDataAvailableSubscribed)
            {
                waveIn.DataAvailable -= OnDataAvailable;
                onDataAvailableSubscribed = false;
            }     

            recordButton.Text = recordText;
            if (waveIn != null)
            {
                waveIn.StopRecording();
            }
            FinalizeWaveFile(writer);
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
                    onDataAvailableSubscribed = true;
                    waveIn.RecordingStopped += OnRecordingStopped;
                    writer = new WaveFileWriter(audioFilePath, waveIn.WaveFormat);
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
                SetControlStates(false);    // Toggle the record and stop buttons
                recordingProgressBarLabel.Text = myRecordingText;
                if (e.Exception != null)
                {
                    MPAiMessageBoxFactory.Show(string.Format(formatErrorText, e.Exception.Message));
                    // Remove the file if it's not valid
                    writer.Close();
                    recordingProgressBarLabel.Text = noFileText;
                    File.Delete(audioFilePath);
                    removeButton.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Handles the state of the buttons based on whether or not the user is recording.
        /// </summary>
        /// <param name="isRecording">Boolean value representing whether or not the screen is recording.</param>
        private void SetControlStates(bool isRecording)
        {
            recordButton.Text = isRecording ? stopText : recordText;
            recordingProgressBar.Visible = isRecording;
            recordingProgressBarLabel.Visible = !isRecording;
            addFromFileButton.Enabled = !isRecording;
            removeButton.Enabled = !isRecording;
        }

        /// <summary>
        /// Removes the current audio file from storage.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void removeButton_Click(object sender, EventArgs e)
        {
            asyncStop();
            recordingProgressBarLabel.Text = noFileText;
            File.Delete(audioFilePath);
            removeButton.Enabled = false;
        }

        /// <summary>
        /// Fires when the control needs to have it's library directory assigned to it.
        /// Navigates to where the directories should be, and sets them, or asks the user to find the directory if it's not there.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();  // Get the currently running project
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)  // If the computer is running an x86 architecture, use the x86 folder.
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDataPath.Path, @"VlcLibs\vlcx86\"));
            else        // otherwise use the x64 folder.
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDataPath.Path, @"VlcLibs\vlcx64\"));

            if (!e.VlcLibDirectory.Exists)      // If a folder is missing
            {
                var folderBrowserDialog = new FolderBrowserDialog();       // Raise a browser window and let the user find it.
                folderBrowserDialog.Description = selectFolderString;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        /// <summary>
        /// Handles the functionality of the play/pause button, which differs based on the state of the player.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void playButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (vlcControl.State)
                {
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.NothingSpecial:    // Occurs when control is finished loading. Same functionality as stopped.
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended:             // Occurs when control has finished playing a video. Same funcionaility as stopped.
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped:           // Occurs when the player has been stopped. No video is loaded in.
                        {
                            playAudio();
                        }
                        break;
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing:   // If playing, pause and update the button.
                        {
                            vlcControl.Pause();
                            playButton.ImageIndex = 1;
                        }
                        break;
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused:    // If paused, play and update the button.
                        {
                            vlcControl.Play();  // asyncPlay is not used here, as it starts playback from the beginning.
                            playButton.ImageIndex = 3;
                        }
                        break;
                    case Vlc.DotNet.Core.Interops.Signatures.MediaStates.Error:
                        {
                            MPAiMessageBoxFactory.Show(invalidStateString);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exp)
            {
                MPAiMessageBoxFactory.Show(exp.Message);
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Plays or pauses the audio, depending on the VLC player's current state.
        /// </summary>
        private void playAudio()
        {

            using (MPAiModel DBModel = MPAiModel.InitializeDBModel())
            {
                Word wd = wordsList[currentRecordingIndex];
                Speaker spk = UserManagement.CurrentUser.Speaker;  // Get the speaker from user settings.
                Console.WriteLine(UserManagement.CurrentUser.Speaker.Name + " " + VoiceType.getDisplayNameFromVoiceType(UserManagement.CurrentUser.Voice));
                Recording rd = DBModel.Recording.Local.Where(x => x.WordId == wd.WordId && x.SpeakerId == spk.SpeakerId).SingleOrDefault();

                if (rd != null)
                {
                    ICollection<SingleFile> audios = rd.Audios;
                    if (audios == null || audios.Count == 0) throw new Exception("No audio recording!");
                    SingleFile sf = audios.PickNext();
                    filePath = Path.Combine(sf.Address, sf.Name);

                    asyncPlay();
                    playButton.ImageIndex = 3;
                }
                else
                {
                    MPAiMessageBoxFactory.Show(invalidRecordingString);
                }
            }
        }

        /// <summary>
        /// The VLCPlayer is not threadsafe, so it is much easier to invoke it with delegates.
        /// This should be used to play the video; it also handles the audio overlay.
        /// </summary>
        private void asyncPlay()
        {
            if (!recordingProgressBarLabel.Text.Equals(noFileText))
            {
                playRecording = true; //About to play database recording so next recording should be from the user audio. so set playrecording to false.
            }
            else
            {
                playRecording = false; //No user recording so the next recording should be from the database.
            }
            // Get a new instance of the delegate
            delegatePlayer VLCDelegate = new delegatePlayer(this.Play);
            // Call play asynchronously.
            VLCDelegate.BeginInvoke(new Uri(filePath), new string[] { }, null, null);
        }

        private void asyncPlay(string audioFilePath)
        {
            playRecording = false; //About to play user recording so next recording should be from the database. so set playrecording to false.
            delegatePlayer VLCDelegate = new delegatePlayer(this.Play);
            // Call play asynchronously.
            VLCDelegate.BeginInvoke(new Uri(audioFilePath), new string[] { }, null, null);
        }

        private void Play(System.Uri file, string[] pars)
        {
            vlcControl.Play(file, pars);
            SetProgress(0);
        }

        /// <summary>
        /// The VLCPlayer is not threadsafe, so it is much easier to invoke it with delegates.
        /// This should be used to stop the video; it also stops audio overlay playback.
        /// </summary>
        private void asyncStop()
        {
            // Get a new instance of the delegate
            delegateStopper VLCDelegate = new delegateStopper(vlcControl.Stop);
            // Call stop asynchronously.
            VLCDelegate.BeginInvoke(null, null);
        }

        /// <summary>
        /// When the video is stopped, make the pause button into the play button, by swapping the images.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void OnVlcControlStopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            playButton.ImageIndex = 1;
        }

        /// <summary>
        /// When the mouse hovers over the play button, it highlights.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void playButton_MouseEnter(object sender, EventArgs e)
        {
            playButton.ImageIndex = vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing ? 0 : 2;
        }

        /// <summary>
        /// When the mouse leaves the play button, stop it from highlighting.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void playButton_MouseLeave(object sender, EventArgs e)
        {
            playButton.ImageIndex = vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing ? 1 : 3;
        }

        /// <summary>
        /// When the mouse hovers over the back button, it highlights.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void backwardButton_MouseEnter(object sender, EventArgs e)
        {
            backwardButton.ImageIndex = 0;
        }

        /// <summary>
        /// When the mouse leaves the back button, stop it from highlighting.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void backwardButton_MouseLeave(object sender, EventArgs e)
        {
            backwardButton.ImageIndex = 1;
        }

        /// <summary>
        /// When the mouse hovers over the forward button, it highlights.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void forwardButton_MouseEnter(object sender, EventArgs e)
        {
            forwardButton.ImageIndex = 0;
        }

        /// <summary>
        /// When the mouse leaves the forward button, stop it from highlighting.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void forwardButton_MouseLeave(object sender, EventArgs e)
        {
            forwardButton.ImageIndex = 1;
        }

        /// <summary>
        /// When the video finishes playing, revert the play button to it's original state, and repeat the video if the user has set it to repeat.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void vlcControl_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {

            //Recording reached the end, either repeat recording, play user recording, play recording for next word, or stop.
            SetProgress(100);

            // The only way to loop playback is to have a delegate call play asynchronously. 

            //Infinite loop.
            if (RepeatTimes == 11)
            {
                //Checks if it needs to play the user recording or the database recording.
                if (playRecording && !recordingProgressBarLabel.Text.Equals(noFileText))
                {
                    // Plays user recording.
                    asyncPlay(audioFilePath);
                }
                else
                {
                    //Plays database recording.
                    asyncPlay();
                }
            }
            // n repeats.
            else if (repeatsRemaining > 0)
            {
                //Checks if it needs to play the user recording or the database recording.
                if (playRecording && !recordingProgressBarLabel.Text.Equals(noFileText))
                {
                    playRecording = false;
                    asyncPlay(audioFilePath); // Plays the file at the specific
                }
                else
                {
                    Console.WriteLine(repeatsRemaining);
                    if (!(repeatsRemaining == 0)) {
                    Console.Write("DataBase -> ");

                    repeatsRemaining--;
                    playRecording = true;
                    asyncPlay();
                    }
                }
            }
            else if ((repeatsRemaining == 0) && ((!recordingProgressBarLabel.Text.Equals(noFileText)) && playRecording))
            {
                playRecording = false;
                asyncPlay(audioFilePath); 
            }
            else
            {
                repeatsRemaining = RepeatTimes;
                // Autoplay functionality
                if (playNextCheckBox.Checked)
                {
                    try
                    {
                        if (currentRecordingIndex < wordsList.Count - 1)
                        {
                            currentRecordingIndex++;
                            // Run this command on the GUI thread
                            Invoke((MethodInvoker)delegate {
                                WordComboBox.SelectedIndex++;
                            });
                        }
                        else    // Move back to beginning if the user reaches the end of the list.
                        {
                            currentRecordingIndex = 0;
                            Invoke((MethodInvoker)delegate {
                                WordComboBox.SelectedIndex = 0;
                            });
                        }
                        playAudio();
                    }
                    catch (IndexOutOfRangeException exp)
                    {
                        // This can be reached in some cases where the Invoke takes too long.
                        WordComboBox.SelectedIndex = currentRecordingIndex;
                    }
                }
                else
                {
                    playButton.ImageIndex = 1;
                }
            }
        }

      

        /// <summary>
        /// Move to the next sound when the next button is clicked.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void forwardButton_Click(object sender, EventArgs e)
        {
            if (currentRecordingIndex < wordsList.Count - 1)
            {
                currentRecordingIndex++;
                WordComboBox.SelectedIndex++;
            }
            else    // Move back to beginning if the user reaches the end of the list.
            {
                currentRecordingIndex = 0;
                WordComboBox.SelectedIndex = 0;
            }

            // If the video is playing, automatically play the next one.
            if (vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing))
            {
                playAudio();
            }
            else
            {
                asyncStop();  // So the old image doesn't stay on screen.
            }

        }

        /// <summary>
        /// Move to the previous sound when the previous button is clicked.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void backwardButton_Click(object sender, EventArgs e)
        {
            if (currentRecordingIndex > 0)
            {
                currentRecordingIndex--;
                WordComboBox.SelectedIndex--;
            }
            else    // Move to the end if the user reaches the beginning of the list.
            {
                currentRecordingIndex = wordsList.Count - 1;
                WordComboBox.SelectedIndex = wordsList.Count - 1;
            }
            // If the video is playing, automatically play the next one.
            if (vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing))
            {
                playAudio();
            }
            else
            {
                asyncStop();  // So the old image doesn't stay on screen.
            }
        }

        /// <summary>
        /// Removes the selected word from the Sound List Current List Box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void soundListRemoveButton_Click(object sender, EventArgs e)
        {
            asyncStop();    // Stop playback - changing lists can cause unusual behaviour.

            // Can't remove from a list as we iterate through it, so add the items to remove to a list, then delete them.
            List<Word> tempList = new List<Word>();
            foreach (Word wd in soundListCurrentListBox.SelectedItems)
            {
                tempList.Add(wd);
            }

            // Remove the word from all the lists used by the screen.
            foreach (Word wd in tempList)
            {
                soundListCurrentListBox.Items.Remove(wd);
                WordComboBox.Items.Remove(wd);
                wordsList.Remove(wd);
            }

            selectItemInComboBox();
        }

        /// <summary>
        /// Adds all the available words to the Sound List Current List Box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void soundListAddAllButton_Click(object sender, EventArgs e)
        {
            asyncStop();        // Stop playback - changing lists can cause unusual behaviour.

            // Clear all lists - this is more efficient than sorting them.
            soundListCurrentListBox.Items.Clear();
            WordComboBox.Items.Clear();
            wordsList.Clear();

            // Add all words to each list used by the screen. These will always be in he correct order, as the All box doesn't change.
            soundListCurrentListBox.Items.AddRange(soundListAllListBox.Items);
            foreach (Word wd in soundListAllListBox.Items)
            {
                WordComboBox.Items.Add(wd);
                wordsList.Add(wd);
            }

            selectItemInComboBox();
        }

        /// <summary>
        /// Adds all the available words to the Sound List Current List Box.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void soundListAddButton_Click(object sender, EventArgs e)
        {
            asyncStop();        // Stop playback - changing lists can cause unusual behaviour.

            List<Word> tempList = new List<Word>();
            // Add all words tha should be added to each list used by the screen. These will always be in he correct order, as the All box doesn't change.
            foreach (Word wd in soundListAllListBox.Items)
            {
                if (soundListCurrentListBox.Items.Contains(wd) || soundListAllListBox.SelectedItems.Contains(wd))
                {
                    tempList.Add(wd);
                }
            }

            // It is more efficient to clear the lists and add new items than to sort the existing lists.
            WordComboBox.Items.Clear();
            soundListCurrentListBox.Items.Clear();
            foreach (Word wd in tempList)
            {
                WordComboBox.Items.Add(wd);
                soundListCurrentListBox.Items.Add(wd);
            }
            wordsList = tempList;

            selectItemInComboBox();
        }

        /// <summary>
        /// WordComboBox should always have a value selected. 
        /// This method should be called when the selected item may have been deleted, and it selects index 0.
        /// </summary>
        private void selectItemInComboBox()
        {
            WordComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Restores all options to their default settings.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            asyncStop();    // Stop playback.
            soundListAddAllButton_Click(sender, e); // Add all words back to the lists.
            WordComboBox.SelectedIndex = 0;    // Reset the combobox.
            if (!recordingProgressBarLabel.Text.Equals(noFileText))
            {
                removeButton_Click(sender, e);  // Remove any audio file that may have been added.
            }
            repeatTrackBar.Value = 0;   // Reset repeats to 0.
            playNextCheckBox.Checked = false;   // Reset play next to false.
        }

        private void vlcControl_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            SetProgress((int) (vlcControl.Position * 100));
        }

        /// <summary>
        /// Fires when the form closes. 
        /// If the user pressed the back button, the next form will be loaded. 
        /// If the user closed the form in some other way, the app will temrinate.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void AudioPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (appClosing)
            {
                UserManagement.WriteSettings();
                DirectoryManagement.WritePaths();
                Application.Exit();
            }
        }
    }
}
