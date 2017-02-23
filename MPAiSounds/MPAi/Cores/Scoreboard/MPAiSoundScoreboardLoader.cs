using MPAi.Modules;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPAi.Cores.Scoreboard
{
    static class MPAiSoundScoreboardLoader
    {
        /// <summary>
        /// Private method that checks whether the user directory exists, and if it does not, creates it.
        /// </summary>
        /// <param name="user"></param>
        private static void ensureUserDirectoryExists(MPAiUser user)
        {
            if (!Directory.Exists(Path.Combine(DirectoryManagement.ScoreboardReportFolder, user.getName())))
            {
                Directory.CreateDirectory(Path.Combine(DirectoryManagement.ScoreboardReportFolder, user.getName()));
            }
        }

        /// <summary>
        /// Retruns the appropriate filepath string for the specified user's scoreboard file.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string SoundScoreboardFileAddress(MPAiUser user)
        {
            ensureUserDirectoryExists(user);
            return Path.Combine(DirectoryManagement.ScoreboardReportFolder, user.getName(), "MPAiSoundScoreboard.txt");

        }
        
        /// <summary>
        /// This method saved the specified scoreboard to the appropriate user's scoreboard file. it writes the data of the scoreboard
        /// using a similar notation to html, where the different tags refer to different fields of the scoreboard hierarchy. Items
        /// are nested inside sessions, which are in turn nested inside a scoreboard.
        /// </summary>
        /// <param name="scoreboard"></param>
        public static void SaveScoreboard(MPAiSoundScoreBoard scoreboard)
        {
            if (scoreboard.IsEmpty())
            {
                return;
            }
            if (File.Exists(SoundScoreboardFileAddress(scoreboard.User)))
            {
                File.Delete(SoundScoreboardFileAddress(scoreboard.User));
            }
            using (FileStream fs = new FileStream(SoundScoreboardFileAddress(scoreboard.User), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("<Scoreboard>");
                    foreach (MPAiSoundScoreBoardSession session in scoreboard.Sessions)
                    {
                        if (session.IsEmpty())
                        {
                            continue;
                        }
                        sw.WriteLine("<Session>");
                        sw.WriteLine("<Date>");
                        sw.WriteLine(session.DateAndTime);
                        sw.WriteLine("</Date>");
                        sw.WriteLine("<OverallCorrectnessPercentage>");
                        sw.WriteLine(session.OverallCorrectnessPercentage);
                        sw.WriteLine("</OverallCorrectnessPercentage>");
                        sw.WriteLine("<Content>");
                        foreach (MPAiSoundScoreBoardItem item in session.Content)
                        {
                            sw.WriteLine("<Vowel>");
                            sw.WriteLine(item.Vowel);
                            sw.WriteLine("</Vowel>");
                            sw.WriteLine("<CorrectnessPercentage>");
                            sw.WriteLine(item.CorrectnessPercentage);
                            sw.WriteLine("</CorrectnessPercentage>");
                        }
                        sw.WriteLine("</Content>");
                        sw.WriteLine("</Session>");
                    }
                    sw.WriteLine("</Scoreboard>");
                }
            }
            File.SetAttributes(SoundScoreboardFileAddress(scoreboard.User), File.GetAttributes(SoundScoreboardFileAddress(scoreboard.User)) | FileAttributes.Hidden);
        }

        /// <summary>
        /// This method loads the scoreboard for the specified user. It does this by reading the html-style file created by SaveScoreboard.
        /// The nested nature of the file is why this method is also deeply nested.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static MPAiSoundScoreBoard LoadScoreboard(MPAiUser user)
        {
            MPAiSoundScoreBoard scoreboard = new MPAiSoundScoreBoard(user);

            if (File.Exists(SoundScoreboardFileAddress(scoreboard.User)))
            {
                using (FileStream fs = new FileStream(SoundScoreboardFileAddress(scoreboard.User), FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line;
                        line = sr.ReadLine();
                        //MPAiMessageBoxFactory.Show(line + ": <Scoreboard> expected");
                        if (line.Equals("<Scoreboard>"))
                        {
                            //MPAiMessageBoxFactory.Show("Success, entered <Scoreboard>");
                            while (!line.Equals("</Scoreboard>"))
                            {
                                line = sr.ReadLine();
                                while (line.Equals("<Session>"))
                                {
                                    line = sr.ReadLine();
                                    while (!line.Equals("</Session>"))
                                    {
                                        DateTime dateAndTime = new DateTime(); ;
                                        if (line.Equals("<Date>"))
                                        {
                                            line = sr.ReadLine();
                                            while (!line.Equals("</Date>"))
                                            {
                                                dateAndTime = new DateTime();
                                                if (!DateTime.TryParse(line, out dateAndTime))
                                                {
                                                    throw new FileLoadException("Date could not be read");
                                                }
                                                line = sr.ReadLine();
                                            }
                                            line = sr.ReadLine();
                                        }

                                        float overallCorrectnessPercentage = -1;
                                        if (line.Equals("<OverallCorrectnessPercentage>"))
                                        {
                                            line = sr.ReadLine();
                                            while (!line.Equals("</OverallCorrectnessPercentage>"))
                                            {
                                                if (!float.TryParse(line, out overallCorrectnessPercentage))
                                                {
                                                    throw new FileLoadException("Overall Correctness Percentage could not be read");
                                                }
                                                line = sr.ReadLine();
                                            }
                                            line = sr.ReadLine();
                                        }

                                        List<MPAiSoundScoreBoardItem> content = new List<MPAiSoundScoreBoardItem>();
                                        if (line.Equals("<Content>"))
                                        {
                                            line = sr.ReadLine();
                                            while (!line.Equals("</Content>"))
                                            {
                                                string vowel = "";
                                                float correctnessPercentage = -1;

                                                if (line.Equals("<Vowel>"))
                                                {
                                                    bool firstline = true;
                                                    line = sr.ReadLine();
                                                    while (!line.Equals("</Vowel>"))
                                                    {
                                                        if (firstline)
                                                        {
                                                            firstline = false;
                                                            vowel += line;
                                                        }
                                                        else
                                                        {
                                                            vowel += String.Format(@"{0}", Environment.NewLine) + line;
                                                        }
                                                        line = sr.ReadLine();
                                                    }
                                                    line = sr.ReadLine();
                                                }

                                                if (line.Equals("<CorrectnessPercentage>"))
                                                {
                                                    line = sr.ReadLine();
                                                    while (!line.Equals("</CorrectnessPercentage>"))
                                                    {
                                                        if (!float.TryParse(line, out correctnessPercentage)) 
                                                        {
                                                            throw new FileLoadException("Correctness Percentage could not be read");
                                                        }    
                                                        line = sr.ReadLine();
                                                    }
                                                    line = sr.ReadLine();
                                                }
                                                content.Add(new MPAiSoundScoreBoardItem(vowel, correctnessPercentage));
                                            }
                                            line = sr.ReadLine();
                                        }
                                        MPAiSoundScoreBoardSession session = scoreboard.NewScoreBoardSession(dateAndTime, content);
                                        session.OverallCorrectnessPercentage = overallCorrectnessPercentage;
                                    }
                                    line = sr.ReadLine();
                                }
                            }
                        }
                    }
                }
            }
            return scoreboard;
        }
    }

}
