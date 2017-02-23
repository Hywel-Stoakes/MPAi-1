using MPAi.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPAi.Cores.Scoreboard
{
    public delegate float SimilarityAlgorithmCallBack(string str1, string str2);
    /// <summary>
    /// Class representing each item on the scoreboard.
    /// </summary>
    public class MPAiSpeakScoreBoardItem
    {
        public MPAiSpeakScoreBoardItem(string expectedText, string recognisedText, string analysis, string recordingName)
        {
            this.expectedText = expectedText;
            this.recognisedText = recognisedText;
            this.analysis = analysis;
            this.recordingName = recordingName;
            this.similarity = SimilarityScore(SimilarityAlgorithm.DamereauLevensheinDistanceAlgorithm);
        }
        /// <summary>
        /// Calls the similarity algorithm to calculate the difference between the two arguments.
        /// </summary>
        /// <param name="simi">The delegate method to use to calculate the difference between the two arguments.</param>
        /// <returns>A float representing the percentage difference.</returns>
        private float SimilarityScore(SimilarityAlgorithmCallBack simi)
        {
            return simi(recognisedText, expectedText);
        }

        /// <summary>
        /// The text the HTKEngine identified.
        /// </summary>
        private string recognisedText;
        public string RecognisedText
        {
            get { return recognisedText; }
        }
        /// <summary>
        /// The text the user input as what they were trying to say.
        /// </summary>
        private string expectedText;
        public string ExpectedText
        {
            get { return expectedText; }
        }
        /// <summary>
        /// The text describing what the user got right and wrong.
        /// </summary>
        private string analysis;
        public string Analysis
        {
            get { return analysis; }
        }

        /// <summary>
        /// The text describing what the user got right and wrong.
        /// </summary>
        private float similarity;
        public float Similarity
        {
            get
            {
                return similarity;
            }
        }

        private string recordingName;

        /// <summary>
        /// The name of the recording which was analysed to generate this scoreboard item.
        /// </summary>
        public string RecordingName
        {
            get
            {
                return recordingName;
            }
        }
    }

    /// <summary>
    /// Represents an MPAi Speak Scoreboard Session. A session is created for every time a enters the speech recognition test.
    /// A session has a date and time, indicating when it was created, a list of MPAiSpeakScoreboardItems that it contains, and an overall
    /// correctness percentage for the session.
    /// </summary>
    public class MPAiSpeakScoreBoardSession
    {
        private DateTime dateAndTime;

        /// <summary>
        /// Wrapper property for the date and time the session was created.
        /// </summary>
        public DateTime DateAndTime
        {
            get { return dateAndTime; }
        }
     
        private List<MPAiSpeakScoreBoardItem> content = new List<MPAiSpeakScoreBoardItem>();

        /// <summary>
        /// Read only wrapper property for the list of scoreboard items contained by this session.
        /// </summary>
        public List<MPAiSpeakScoreBoardItem> Content
        {
            get { return content; }
        }

        /// <summary>
        /// Calculates the overall correctness of each entry, by adding each entry's correctness and dividing by the number of entries.
        /// </summary>
        public float OverallCorrectnessPercentage
        {
            get { return CalculateScore / Content.Count; }
        }

        /// <summary>
        /// Calculates the total score of each entry on the scoreboard.
        /// </summary>
        public float CalculateScore
        {
            get
            {
                float sum = 0;
                foreach (MPAiSpeakScoreBoardItem item in Content)
                {
                    sum += item.Similarity;
                }
                return sum;
            }
        }

        /// <summary>
        /// Constructor for the session, taking the date and time as an argument.
        /// </summary>
        /// <param name="dateAndTime"></param>
        public MPAiSpeakScoreBoardSession(DateTime dateAndTime)
        {
            this.dateAndTime = dateAndTime;
        }

        /// <summary>
        /// Constructor for the session, taking the date and time as an argument, and also a list of scoreboard items to initialise the content with.
        /// </summary>
        /// <param name="dateAndTime"></param>
        /// <param name="content"></param>
        public MPAiSpeakScoreBoardSession(DateTime dateAndTime, List<MPAiSpeakScoreBoardItem> content)
        {
            this.dateAndTime = dateAndTime;
            this.content = content;
        }

        /// <summary>
        /// This method is used to add a scoreboard item to the content list, by specifying the fields of the scorebord item.
        /// </summary>
        /// <param name="expectedText"></param>
        /// <param name="recognisedText"></param>
        /// <param name="analysis"></param>
        /// <param name="recordingName"></param>
        public void AddScoreBoardItem(string expectedText, string recognisedText, string analysis, string recordingName)
        {
            Content.Add(new MPAiSpeakScoreBoardItem(expectedText, recognisedText, analysis, recordingName));
        }

        /// <summary>
        /// Returns an int indicating the ordering of two scoreboard sessions, by date.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int ComparisionByDate(MPAiSpeakScoreBoardSession x, MPAiSpeakScoreBoardSession y)
        {
            return DateTime.Compare(y.DateAndTime, x.DateAndTime);
        }

        public bool IsEmpty()
        {
            return !Content.Any();
        }
    }

    /// <summary>
    /// Represents a Scoreboard for MPAiSound. Each user has one such scoreboard, which persists between MPAi session by writing to a file, using the MPAiSpeakScoreBoardLoader class.
    /// A scoreboard is registerd to a single user, and contains a list of sessions.
    /// </summary>
    public class MPAiSpeakScoreBoard
    {
        private MPAiUser user;

        /// <summary>
        /// Read only wrapper property for the user associated with the scoreboard.
        /// </summary>
        public MPAiUser User
        {
            get { return user; }
        }

        private List<MPAiSpeakScoreBoardSession> sessions = new List<MPAiSpeakScoreBoardSession>();

        /// <summary>
        /// Read only wrapper property for the list of scoreboard items contained by this session. 
        /// </summary>
        public List<MPAiSpeakScoreBoardSession> Sessions
        {
            get
            {
                sessions.Sort(MPAiSpeakScoreBoardSession.ComparisionByDate);
                return sessions;
            }
        }

        public MPAiSpeakScoreBoard(MPAiUser user)
        {
            this.user = user;
        }

        /// <summary>
        /// Creates a new scoreboard session and returns a reference to the caller, so they can add to the session.
        /// </summary>
        /// <returns></returns>
        public MPAiSpeakScoreBoardSession NewScoreBoardSession()
        {
            MPAiSpeakScoreBoardSession session = new MPAiSpeakScoreBoardSession(DateTime.Now);
            sessions.Add(session);
            return session;
        }

        /// <summary>
        /// Creates a new scoreboard session based on the parameters and returns a reference to the caller, so they can add to the session.
        /// </summary>
        /// <param name="dateAndTime"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public MPAiSpeakScoreBoardSession NewScoreBoardSession(DateTime dateAndTime, List<MPAiSpeakScoreBoardItem> content)
        {
            MPAiSpeakScoreBoardSession session = new MPAiSpeakScoreBoardSession(dateAndTime, content);
            sessions.Add(session);
            return session;
        }

        /// <summary>
        /// Used to save the scoreboard; calls the appropriatem method in MPAiSpeakScoreboardLoader.
        /// </summary>
        public void SaveScoreBoardToFile()
        {
            MPAiSpeakScoreboardLoader.SaveScoreboard(this);
        }

        /// <summary>
        /// Returns true if the scoreboard is 'empty.' A scoreboard is empty if it either contains no sessions, or if every session it has contains no items.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            foreach (MPAiSpeakScoreBoardSession session in Sessions)
            {
                if (!session.IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns true if the supplied recording name has already been analysed in the current scoreboard.
        /// </summary>
        /// <param name="recordingName"></param>
        /// <returns></returns>
        public bool IsRecordingAlreadyAnalysed(string recordingName)
        {
            foreach (MPAiSpeakScoreBoardSession session in Sessions)
            {
                foreach (MPAiSpeakScoreBoardItem item in session.Content)
                {
                    if (recordingName.Equals(item.RecordingName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// Wrapper class for the similarity algorithm employed for the correctness value.
    /// </summary>
    public static class SimilarityAlgorithm
    {
        /// <summary>
        /// Implementation of the Damereau-Levenshein Distance Algorithm with adjacent transpositions.
        /// This calculates the difference between two strings based on the minimal number of operations to get from one to the other.
        /// </summary>
        /// <param name="str1">The first string to compare.</param>
        /// <param name="str2">The second string to compare.</param>
        /// <returns>A float representing the percentage difference between the two parameters.</returns>
        public static float DamereauLevensheinDistanceAlgorithm(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1))
            {
                if (string.IsNullOrEmpty(str2))
                    return 0;
                return str2.Length;
            }

            if (string.IsNullOrEmpty(str2))
            {
                return str1.Length;
            }

            int n = str1.Length;
            int m = str2.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (str2[j - 1] == str1[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return Math.Abs(1 - (float)d[n, m] / Math.Max(m, n));
        }
    }
}

