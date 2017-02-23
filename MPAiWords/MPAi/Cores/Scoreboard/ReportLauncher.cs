using MPAi.Modules;
using System.IO;
using System.Web.UI;

namespace MPAi.Cores.Scoreboard
{
    /// <summary>
    /// Class that generates an HTML report based on data from a MPAiSpeakScoreBoard object.
    /// </summary>
    static class ReportLauncher
    {
        private static void ensureUserDirectoryExists()
        {
            if (!Directory.Exists(Path.Combine(DirectoryManagement.ScoreboardReportFolder, UserManagement.CurrentUser.getName())))
            {
                Directory.CreateDirectory(Path.Combine(DirectoryManagement.ScoreboardReportFolder, UserManagement.CurrentUser.getName()));
            }
        }
        /// <summary>
        /// The address within the local repository of the generated Speak report.
        /// </summary>
        public static string MPAiSpeakScoreReportHTMLAddress
        {
            get
            {
                ensureUserDirectoryExists();
                return Path.Combine(DirectoryManagement.ScoreboardReportFolder, UserManagement.CurrentUser.getName(), "MPAiSpeakReport.html");
            }
        }

        /// <summary>
        /// The address within the local repository of the generated Sound report.
        /// </summary>
        public static string MPAiSoundScoreReportHTMLAddress
        {
            get
            {
                ensureUserDirectoryExists();
                return Path.Combine(DirectoryManagement.ScoreboardReportFolder, UserManagement.CurrentUser.getName(), "MPAiSoundReport.html");
            }
        }
        /// <summary>
        /// The address within the local repository of the generated CSS File.
        /// </summary>
        private static string OriginalScoreboardReportCSSAddress
        {
            get
            {
                return Path.Combine(AppDataPath.Path, "Resources", "CSSFiles", "Scoreboard.css");

            }
        }
        /// <summary>
        /// The address within the local repository of the generated CSS File.
        /// </summary>
        public static string ScoreboardReportCSSAddress
        {
            get
            {
                ensureUserDirectoryExists();
                return Path.Combine(DirectoryManagement.ScoreboardReportFolder, UserManagement.CurrentUser.getName(), "Scoreboard.css");
            }
        }

        /// <summary>
        /// Shows the MPAiSpeakScoreBoard, if it exists.
        /// </summary>
        public static void ShowMPAiSpeakScoreReport()
        {
            if (File.Exists(MPAiSpeakScoreReportHTMLAddress))
            {
                IoController.ShowInBrowser(MPAiSpeakScoreReportHTMLAddress);
            }
        }

        /// <summary>
        /// Shows the MPAiSoundScoreBoard, if it exists.
        /// </summary
        public static void ShowMPAiSoundScoreReport()
        {
            if (File.Exists(MPAiSoundScoreReportHTMLAddress))
            {
                IoController.ShowInBrowser(MPAiSoundScoreReportHTMLAddress);
            }

        }

        /// <summary>
        /// Generates the CSS File for the report, if it does not already exist.
        /// </summary>
        private static void generateScoreboardCSS()
        {
            File.Copy(OriginalScoreboardReportCSSAddress, ScoreboardReportCSSAddress);
        }

        /// <summary>
        /// Generates an HTML score report based on an input MPAiSpeak scoreboard.
        /// </summary>
        /// <param name="scoreboard">The scoreboard to generate an HTML report of.</param>
        public static void GenerateMPAiSpeakScoreHTML(MPAiSpeakScoreBoard scoreboard)
        {
            if(scoreboard.IsEmpty())
            {
                return;
            }

            scoreboard.SaveScoreBoardToFile();

            if(!File.Exists(ScoreboardReportCSSAddress))
            {
                generateScoreboardCSS();
            }
            using (FileStream fs = new FileStream(MPAiSpeakScoreReportHTMLAddress, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        htw.RenderBeginTag(HtmlTextWriterTag.Html);
                        // Table settings
                        htw.RenderBeginTag(HtmlTextWriterTag.Head);
                        htw.AddAttribute("charset", "UTF-8");
                        htw.RenderBeginTag(HtmlTextWriterTag.Meta);
                        htw.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                        htw.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                        htw.AddAttribute(HtmlTextWriterAttribute.Href, "Scoreboard.css");
                        htw.RenderBeginTag(HtmlTextWriterTag.Link);
                        htw.RenderEndTag();
                        htw.RenderEndTag();
                        htw.RenderEndTag();
                        //Scoreboard Title
                        htw.RenderBeginTag(HtmlTextWriterTag.Body);
                        htw.AddAttribute(HtmlTextWriterAttribute.Class, "title");
                        htw.RenderBeginTag(HtmlTextWriterTag.Div);
                        htw.RenderBeginTag(HtmlTextWriterTag.H3);
                        htw.Write(scoreboard.User.GetCorrectlyCapitalisedName() + "'s MPAi Speak Pronunciation Scoreboard");
                        htw.RenderEndTag();
                        htw.RenderEndTag();

                        foreach(MPAiSpeakScoreBoardSession session in scoreboard.Sessions)
                        {
                            if (session.IsEmpty())
                            {
                                continue;
                            }
                            //Table Title
                            htw.AddAttribute(HtmlTextWriterAttribute.Class, "table-title");
                            htw.RenderBeginTag(HtmlTextWriterTag.Div);
                            htw.RenderBeginTag(HtmlTextWriterTag.H3);
                            htw.Write(session.DateAndTime.ToString("dd MMMM yyyy, h:mm tt"));
                            htw.RenderEndTag();
                            htw.RenderEndTag();
                            // Header row of the table
                            htw.AddAttribute(HtmlTextWriterAttribute.Class, "table-fill");
                            htw.RenderBeginTag(HtmlTextWriterTag.Table);
                            htw.RenderBeginTag(HtmlTextWriterTag.Tr);
                            htw.RenderBeginTag(HtmlTextWriterTag.Th);
                            htw.Write("Expecting Word");
                            htw.RenderEndTag();
                            htw.RenderBeginTag(HtmlTextWriterTag.Th);
                            htw.Write("Recognized Word");
                            htw.RenderEndTag();
                            htw.RenderBeginTag(HtmlTextWriterTag.Th);
                            htw.Write("Similarity Score");
                            htw.RenderEndTag();
                            htw.RenderBeginTag(HtmlTextWriterTag.Th);
                            htw.Write("Analysis Tips");
                            htw.RenderEndTag();
                            htw.RenderEndTag();
                            // Table rows
                            foreach (MPAiSpeakScoreBoardItem item in session.Content)
                            {
                                htw.RenderBeginTag(HtmlTextWriterTag.Tr);
                                htw.RenderBeginTag(HtmlTextWriterTag.Td);
                                htw.Write(item.ExpectedText);
                                htw.RenderEndTag();
                                htw.RenderBeginTag(HtmlTextWriterTag.Td);
                                htw.Write(item.RecognisedText);
                                htw.RenderEndTag();
                                htw.RenderBeginTag(HtmlTextWriterTag.Td);
                                htw.Write(item.Similarity.ToString("0.0%"));
                                htw.RenderEndTag();
                                htw.RenderBeginTag(HtmlTextWriterTag.Td);
                                htw.Write(item.Analysis);
                                htw.RenderEndTag();
                                htw.RenderEndTag();
                            }
                            // Correctness score
                            float correctness = session.OverallCorrectnessPercentage;
                            if (correctness >= 0.8)
                            {
                                htw.AddAttribute(HtmlTextWriterAttribute.Class, "good-colour");
                            }
                            else if (correctness >= 0.5)
                            {
                                htw.AddAttribute(HtmlTextWriterAttribute.Class, "medium-colour");
                            }
                            else
                            {
                                htw.AddAttribute(HtmlTextWriterAttribute.Class, "bad-colour");
                            }
                            htw.RenderBeginTag(HtmlTextWriterTag.Tr);
                            htw.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
                            htw.RenderBeginTag(HtmlTextWriterTag.Td);
                            htw.Write("Pronunciation is " + correctness.ToString("0.0%") + " Correct");
                            htw.RenderEndTag();
                            htw.RenderEndTag();
                            htw.RenderEndTag();

                        }
                        htw.RenderEndTag();
                        htw.RenderEndTag();
                    }
                }
            }
        }
    }
}
