using MPAi.DatabaseModel;
using System;
using System.IO;
using System.Linq;

namespace MPAi.Cores
{
    /// <summary>
    /// Class that handles extracting data about a recording file from it's file name.
    /// </summary>
    class NameParser
    {
        /// <summary>
        /// Property for accessing the file containing the recording.
        /// Getters and setters combine the Address variable (the path to the file) and the FullName variable (the filename, with extension).
        /// Setter takes the path and file name, as a string.
        /// </summary>
        public string SingleFile
        {
            get { return Address + @"\" + FullName; }
            set
            {
                try
                {
                    Address = Path.GetDirectoryName(value);
                    FullName = Path.GetFileName(value);
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                }
            }
        }

        public void SingleFileInitialise(string singleFile)
        {
            try
            {
                Address = Path.GetDirectoryName(singleFile);
                FullNameInitialise(Path.GetFileName(singleFile));
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        /// <summary>
        /// Property for accessing the file name and extension of a file containing a recording.
        /// Getters and setters combine the FileName variable (File name without extension) and the Ext variable (File extension).
        /// Setter takes the file name and extension as a string.
        /// </summary>
        public string FullName
        {
            get
            {
                return FileName + Ext;
            }
            set
            {
                try
                {
                    FileName = Path.GetFileNameWithoutExtension(value);
                    Ext = Path.GetExtension(value);
                }
                catch(Exception exp)
                {
                    Console.WriteLine(exp);
                }   
            }
        }

        public void FullNameInitialise(string fullname)
        {
            try
            {
                FileNameInitialise(Path.GetFileNameWithoutExtension(fullname));
                Ext = Path.GetExtension(fullname);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        /// <summary>
        /// Property for accessing the file name of a file containing a recording. 
        /// This follows a particular format.
        /// Getters and Setters combine the speaker, category, and word from the recording, with the the label of the recording.
        /// Setter takes a string made of 4 values, separated by a hyphen (-): Speaker, Category, Word, and Label.
        /// </summary>
        public string FileName
        {
            get { return Recording + @"-" + Label; }
            set
            {
                try
                {

                    Console.WriteLine(value);
                    if (!IsFileNameCorrect(value)) { throw new Exception("Invalid name format!"); }
                    string[] parts = value.Split('-');
                    Speaker = parts[0];
                    Category = parts[1];
                    Word = parts[2];
                    Label = parts[3];
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void FileNameInitialise( string fileName)
        {
            try
            {
                Console.WriteLine(fileName);
                string[] parts = fileName.Split('-');
                Speaker = parts[0];
                Category = parts[1];
                Word = parts[2];
                Label = parts[3];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static bool IsFileNameCorrect(string fileName)
        {
            string[] parts = fileName.Split('-');

            MPAiModel DBModel = MPAiModel.InitializeDBModel();

            if (parts.Length < 4)
            {
                Console.WriteLine("Not enough Dashes");
                return false;
            }

            if (!DBModel.IsSpeakerString(parts[0]))
            {
                Console.WriteLine("Speaker invalid");

                return false;
            }

            if (!DBModel.IsCategoryString(parts[1]))
            {
                return false;
            }

            if (!DBModel.IsWordString(parts[2]))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Property for accessing the recording details of a file containing a recording.
        /// Combines the Speaker, Category, and Word values, separating them with hyphens (-).
        /// </summary>
        public string Recording
        {
            get { return Speaker + "-" + Category + "-" + Word; }
        }
        /// <summary>
        /// Property for accessing the Address (path to the file) of a file containing a recording.
        /// Setter takes a string representing the enclosing folder, and it's path.
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        /// Property for accessing the Speaker of a recording.
        /// Setter takes a string representing the name of the speaker.
        /// </summary>
        public string Speaker
        {
            get { return speaker; }
            set { speaker = value; }
        }
        /// <summary>
        /// Property for accessing the category of a recording.
        /// Setter takes a string representing the category of the recording.
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        /// <summary>
        /// Property for accessing the Word spoken in a recording. 
        /// Setter takes a string representing the word in the recording.
        /// </summary>
        public string Word
        {
            get { return word; }
            set { word = value; }
        }
        /// <summary>
        /// Property for accessing the Label of a recording.
        /// Setter takes a string representing the label.
        /// </summary>
        public string Label
        {
            get { return label; }
            set { label = value; }
        }
        /// <summary>
        /// Property for accessing the extension of a file containing a recording.
        /// Setter takes a string representing the extension.
        /// Note that this value must contain a leading period - ".wav", not "wav".
        /// </summary>
        public string Ext
        {
            get { return ext; }
            set { ext = value; }
        }
        /// <summary>
        /// Property for checking the format of a file containing a recording. 
        /// If the table of accepted formats for audio or video contains the file extension of this recording, it returns "audio" or "video", as appropriate.
        /// </summary>
        public string MediaFormat
        {
            get
            {
                if (category.Equals("vocaltract"))
                {
                    return "vocaltract";
                }
                else if (audioExtsTable.Contains(ext))
                {
                    return "audio";
                }
                else if (videoExtsTable.Contains(ext))
                {
                    return "video";
                }
                return "unknown";
            }
        }
        /// <summary>
        /// Array of accepted video formats.
        /// </summary>
        private string[] videoExtsTable =
        {
            ".mp4",
            ".avi"
        };
        /// <summary>
        /// Array of accepted audio formats.
        /// </summary>
        private string[] audioExtsTable =
        {
            ".wav",
            ".mp3"
        };
        // Variables used by all properties.
        private string address;
        private string speaker;
        private string category;
        private string word;
        private string label;
        private string ext;
    }
}
