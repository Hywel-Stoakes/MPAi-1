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
    public partial class VideoPlayer : Form, MainFormInterface
    {
        // Strings kept in fields to make text easier to change.
        private string optionsLess = "Less...";
        private string optionsMore = "More...";
        private string recordText = "Record";
        private string stopText = "Stop";

        private string videoText = " (Video)";
        private string vocalText = " (Vocal Tract)";

        private string noFileText = "No current file";
        private string myRecordingText = "My new recording";
        private string selectFolderString = "Select Vlc libraries folder.";

        private string invalidStateString = "Invalid State!";
        private string noVideoString = "No video recording was found for that sound.";
        private string invalidRecordingString = "Invalid recording!";
        private string formatErrorText = "A problem was encountered during recording {0}";
        private string warningText = "Warning";
        private string noAudioDeviceText = "No audio device plugged in.";
        private string wrongFileFormatError = "The audio player can only accept wave files (.wav). Please try another, or record one using the record button.";

        private string[] spinnerText = { "Repeat Forever", "Repeat 10 Times", "Repeat 9 Times", "Repeat 8 Times", "Repeat 7 Times", "Repeat 6 Times", "Repeat 5 Times", "Repeat 4 Times", "Repeat 3 Times", "Repeat Twice", "Repeat Once", "Don't Repeat" };

        private string audioFilePath = Path.Combine(AppDataPath.Temp, "VideoPlayerRecordedAudio.wav");
        private WaveFileWriter writer;
        private WasapiCapture waveIn;

        // Used to keep track of the currently playing file.
        private string filePath;

        // Delegate required for VLC Player.
        delegate void delegatePlayer(Uri file, string[] pars);
        delegate void delegateStopper();

        // The list of recordings to play.
        private List<Recording> wordsList;
        // The index of the current recording.
        private int currentRecordingIndex = 0;

        // Variables used to determine the number of times to repeat a video.
        private int repeatTimes = 0;
        private int repeatsRemaining = 0;

        /// <summary>
        /// Holds the height of the bottom panel between clicks of the options button.
        /// </summary>
        private readonly int bottomHeight;

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

        public VideoPlayer()
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
            VowelComboBox.Items.Clear();
            soundListCurrentListBox.Items.Clear();
            try
            {
                // Create new database context.
                using (MPAiModel DBModel = new MPAiModel())
                {
                    DBModel.Database.Initialize(false); // Added for safety; if the database has not been initialised, initialise it.

                    MPAiUser current = UserManagement.CurrentUser;

                    List<Recording> videoView = DBModel.Recording.Where(x => (
                       (x.Word.Category.Name.Equals("vowel"))   // If the category is vowel, and
                       && (current.Speaker.Name.Equals(x.Speaker.Name))    // The speaker's gender matches the current user's gender, and
                       && ((x.Video != null)   // There is a video of that speaker, or
                       || (x.VocalTract != null))   // The recording has a vocaltract attached. (They are gender neutral, albeit with a male voice.)
                       )).ToList();

                    wordsList = videoView;   // Take this action before display names are changed

                    // Lists of Recording objects, but only their name needs to be displayed to the user.
                    soundListAllListBox.DisplayMember = "Name";
                    VowelComboBox.DisplayMember = "Name";
                    soundListCurrentListBox.DisplayMember = "Name";

                    // Set the values in all the lists used by the program.
                    foreach (Recording rd in videoView)
                    {
                        // Adjust the display names of the recordings in the list, so they are human readable.
                        if (rd.Video != null)
                        {
                            rd.Name = DBModel.Word.SingleOrDefault(x => x.WordId == rd.WordId).Name + videoText;
                        }
                        else if (rd.VocalTract != null)
                        {
                            rd.Name = DBModel.Word.SingleOrDefault(x => x.WordId == rd.WordId).Name + vocalText;
                        }
                        soundListCurrentListBox.Items.Add(rd);
                        VowelComboBox.Items.Add(rd);
                    }

                    soundListAllListBox.DataSource = new BindingSource() { DataSource = videoView };

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
        private void VowelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRecordingIndex = VowelComboBox.SelectedIndex;
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
                playVideo();
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
                MinimumSize = new Size(MinimumSize.Width, 625);
                optionsButton.Text = optionsLess;
                VideoPlayerPanel.Panel2Collapsed = !VideoPlayerPanel.Panel2Collapsed;
            }
            else
            {
                VideoPlayerPanel.Panel2Collapsed = !VideoPlayerPanel.Panel2Collapsed;
                MinimumSize = new Size(MinimumSize.Width, 300);
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
            new MPAiSoundMainMenu().Show();
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
                if (Path.GetExtension(openFileDialog.FileName).Equals(".wav"))
                {
                    File.Copy(openFileDialog.FileName, audioFilePath, true);
                    recordingProgressBarLabel.Text = openFileDialog.FileName;
                    removeButton.Enabled = true;
                }
                else
                {
                    // If they select a file that is not a wav file, stop them and retry.
                    MPAiMessageBoxFactory.Show(wrongFileFormatError);
                    addFromFileButton_Click(sender, e);
                }
            }
            // If the user presses cancel, return.
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
                            playVideo();
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
        /// Plays or pauses the video, depending on the VLC player's current state.
        /// </summary>
        private void playVideo()
        {
            using (MPAiModel DBModel = new MPAiModel())
            {
                // The word list only holds proxy objects, as it's context has closed. A database query is needed to get it's recordings.
                Recording rd = DBModel.Recording.Find(wordsList[currentRecordingIndex].RecordingId);
                Speaker spk = UserManagement.CurrentUser.Speaker;   // Get the speaker from user settings.

                if (rd != null) // If the recording exists
                {
                    SingleFile sf = null;
                    if (rd.Video != null)
                    {
                        sf = rd.Video;
                    }
                    else if (rd.VocalTract != null)
                    {
                        sf = rd.VocalTract;
                    }
                    if (sf == null)
                    {
                        asyncStop();
                        MPAiMessageBoxFactory.Show(noVideoString);
                        return;
                    }
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
                playRecording = true;
            }
            else
            {
                playRecording = false;
            }
            // Get a new instance of the delegate
            delegatePlayer VLCDelegate = new delegatePlayer(vlcControl.Play);
            // Call play asynchronously.
            VLCDelegate.BeginInvoke(new Uri(filePath), new string[] { }, null, null);
            
        }

        private void asyncPlay(string audioFilePath)
        {
            playRecording = false; //About to play user recording so next recording should be from the database. so set playrecording to false.
            delegatePlayer VLCDelegate = new delegatePlayer(vlcControl.Play);
            // Call play asynchronously.
            VLCDelegate.BeginInvoke(new Uri(audioFilePath), new string[] { }, null, null);
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
            // The only way to loop playback is to have a delegate call play asynchronously. 
            if (RepeatTimes == 11)
            {
                if ((!recordingProgressBarLabel.Text.Equals(noFileText)) && (playRecording))
                {
                    asyncPlay(audioFilePath);
                }
                else
                {
                    asyncPlay();
                }
                    
            }
            else if (repeatsRemaining > 0)
            {

                if ((!recordingProgressBarLabel.Text.Equals(noFileText)) && (playRecording))
                {
                    asyncPlay(audioFilePath);
                }
                else
                {
                    asyncPlay();
                    repeatsRemaining--;
                }
                
            }
            else if((repeatsRemaining == 0) && ((!recordingProgressBarLabel.Text.Equals(noFileText)) && (playRecording)))
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
                    try {
                        if (currentRecordingIndex < wordsList.Count - 1)
                        {
                            currentRecordingIndex++;
                            // Run this command on the GUI thread
                            Invoke((MethodInvoker)delegate {
                                VowelComboBox.SelectedIndex++;
                            });
                        }
                        else    // Move back to beginning if the user reaches the end of the list.
                        {
                            currentRecordingIndex = 0;
                            Invoke((MethodInvoker)delegate {
                                VowelComboBox.SelectedIndex = 0;
                            });
                        }
                        playVideo();
                    }
                    catch (IndexOutOfRangeException exp)
                    {
                        // This can be reached in some cases where the Invoke takes too long.
                        VowelComboBox.SelectedIndex = currentRecordingIndex;
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
                VowelComboBox.SelectedIndex++;
            }
            else    // Move back to beginning if the user reaches the end of the list.
            {
                currentRecordingIndex = 0;
                VowelComboBox.SelectedIndex = 0;
            }

            // If the video is playing, automatically play the next one.
            if (vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing))
            {
                playVideo();
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
                VowelComboBox.SelectedIndex--;
            }
            else    // Move to the end if the user reaches the beginning of the list.
            {
                currentRecordingIndex = wordsList.Count - 1;
                VowelComboBox.SelectedIndex = wordsList.Count - 1;
            }
            // If the video is playing, automatically play the next one.
            if (vlcControl.State.Equals(Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing))
            {
                playVideo();
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
            List<Recording> tempList = new List<Recording>(); 
            foreach (Recording rd in soundListCurrentListBox.SelectedItems)
            {
                tempList.Add(rd);
            }

            // Remove the word from all the lists used by the screen.
            foreach (Recording rd in tempList)   
            {
                soundListCurrentListBox.Items.Remove(rd);
                VowelComboBox.Items.Remove(rd);
                wordsList.Remove(rd);
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
            VowelComboBox.Items.Clear();
            wordsList.Clear();

            // Add all words to each list used by the screen. These will always be in he correct order, as the All box doesn't change.
            soundListCurrentListBox.Items.AddRange(soundListAllListBox.Items);
            foreach (Recording rd in soundListAllListBox.Items)     
            {
                VowelComboBox.Items.Add(rd);
                wordsList.Add(rd);
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

            List<Recording> tempList = new List<Recording>();
            // Add all words tha should be added to each list used by the screen. These will always be in he correct order, as the All box doesn't change.
            foreach (Recording rd in soundListAllListBox.Items)
            {
                if (soundListCurrentListBox.Items.Contains(rd) || soundListAllListBox.SelectedItems.Contains(rd))   
                {
                    tempList.Add(rd);
                }
            }

            // It is more efficient to clear the lists and add new items than to sort the existing lists.
            VowelComboBox.Items.Clear();
            soundListCurrentListBox.Items.Clear();
            foreach (Recording rd in tempList)
            {
                VowelComboBox.Items.Add(rd);
                soundListCurrentListBox.Items.Add(rd);
            }
            wordsList = tempList;

            selectItemInComboBox();
        }

        /// <summary>
        /// VowelComboBox should always have a value selected. 
        /// This method should be called when the selected item may have been deleted, and it selects index 0.
        /// </summary>
        private void selectItemInComboBox()
        {
            VowelComboBox.SelectedIndex = 0;
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
            VowelComboBox.SelectedIndex = 0;    // Reset the combobox.
            if (!recordingProgressBarLabel.Text.Equals(noFileText))
            {
                removeButton_Click(sender, e);  // Remove any audio file that may have been added.
            }
            repeatTrackBar.Value = 0;   // Reset repeats to 0.
            playNextCheckBox.Checked = false;   // Reset play next to false.
        }

        /// <summary>
        /// Fires when the form closes. 
        /// If the user pressed the back button, the next form will be loaded. 
        /// If the user closed the form in some other way, the app will temrinate.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void VideoPlayer_FormClosing(object sender, FormClosingEventArgs e)
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

