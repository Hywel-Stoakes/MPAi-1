using System.IO;

namespace MPAi.Modules
{
    /// <summary>
    /// Wrapper class for the App_Data folder path, allowing it to be accessed easily.
    /// </summary>
    class AppDataPath
    {
        /// <summary>
        /// Returns the location of a temporary folder in the temp space on the user's machine.
        /// </summary>
        private readonly static string temp = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("TEMP"), "MPAiTemp", "MPAiWords");

        /// <summary>
        /// Combination of the standard MPAi directory, and a folder which will always be called App_Data.
        /// </summary>
        private readonly static string path = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("appdata"), "MPAi", "MPAiWords");

        /// <summary>
        /// Lazily instantiates the temp directory if it doesn't already exist.
        /// </summary>
        public static string Temp
        {
            get
            {
                if (!Directory.Exists(temp))
                {
                    Directory.CreateDirectory(temp);
                }
                return temp;
            }
        }

        /// <summary>
        /// Lazily instantiates the path directory if it doesn't exist.
        /// </summary>
        public static string Path
        {
            get
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}