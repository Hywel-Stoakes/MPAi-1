using MPAi.Components;
using System.Diagnostics;

namespace MPAi.Modules
{
    /// <summary>
    /// Handles running other programs (Such as Notepad or a browser) from within this one.
    /// Also has methods to get the MPAi directory.
    /// Doesn't store state, and is accessed from many classes, so was made static.
    /// </summary>
    public static class IoController
    {
        private static string fileNotFoundText = "File not found!";
        /// <summary>
        /// Loads the user's default browser to view the specified HTML file.
        /// </summary>
        /// <param name="htmlPath">The file path of the HTML file to be viewed, as a string.</param>
        public static void ShowInBrowser(string htmlPath)
        {
            try
            {
                Process browser = new Process();
                browser.StartInfo.FileName = htmlPath;
                browser.Start();
            }
            catch
            {
                MPAiMessageBoxFactory.Show(fileNotFoundText);
            }
        }
    }
}
