using MPAi.Modules;
using System;
using System.IO;

namespace MPAi.Cores
{
    static class DirectoryManagement
    {
        private static string scoreboardReportFolder;
        private static string videoFolder;
        private static string formantFolder;
        private static string recordingFolder;

        private static readonly string settingsFile = Path.Combine(AppDataPath.Path, "SystemPaths.dat");

        public static string ScoreboardReportFolder
        {
            get
            {
                if (scoreboardReportFolder == null)
                {
                    scoreboardReportFolder = Path.Combine(AppDataPath.Path, "ScoreboardReports");
                    Directory.CreateDirectory(scoreboardReportFolder);  // This method does nothing if the directory already exists
                }
                return scoreboardReportFolder;
            }
            set
            {
                scoreboardReportFolder = value;
            }
        }

        public static string VideoFolder
        {
            get
            {
                if (videoFolder == null)
                {
                    videoFolder = Path.Combine(AppDataPath.Path, "Video");
                    Directory.CreateDirectory(videoFolder); // This method does nothing if the directory already exists.
                }
                return videoFolder;
            }
            set
            {
                videoFolder = value;
            }
        }

        public static string FormantFolder
        {
            get
            {
                if (formantFolder == null)
                {
                    formantFolder = Path.Combine(AppDataPath.Path, "Formant");
                    Directory.CreateDirectory(formantFolder); // This method does nothing if the directory already exists.
                }
                return formantFolder;
            }
            set
            {
                formantFolder = value;
            }
        }

        public static string RecordingFolder
        {
            get
            {
                if (recordingFolder == null)
                {
                    recordingFolder = Path.Combine(AppDataPath.Path, "Recording");
                    Directory.CreateDirectory(recordingFolder); // This method does nothing if the directory already exists.
                }
                return recordingFolder;
            }
            set
            {
                recordingFolder = value;
            }
        }

        /// <summary>
        /// Called to set up file paths, or read from the user's settings if they have been configured prior.
        /// </summary>
        public static void Initialise()
        {
            if (File.Exists(settingsFile))
            {
                // Read out of the file
                ReadPaths();
            }
            else
            {
                // Write defaults to file
                WritePaths();
            }
        }

        /// <summary>
        /// Opens the settings file and populates the fields with the values saved during the last session.
        /// </summary>
        public static void ReadPaths()
        {
            try
            {
                using (FileStream fs = new FileStream(settingsFile, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        if (new FileInfo(settingsFile).Length != 0) // If the file wasn't just created
                        {
                            ScoreboardReportFolder = reader.ReadString();
                            VideoFolder = reader.ReadString();
                            FormantFolder = reader.ReadString();
                            RecordingFolder = reader.ReadString();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Writes all values to the settings file, and creates it if it does not already exist.
        /// </summary>
        public static void WritePaths()
        {
            try
            {
                using (FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        // This will write default values if they haven't already been set.
                        writer.Write(ScoreboardReportFolder);
                        writer.Write(VideoFolder);
                        writer.Write(FormantFolder);
                        writer.Write(RecordingFolder);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
    }
}