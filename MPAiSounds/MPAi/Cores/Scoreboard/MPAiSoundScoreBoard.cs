using MPAi.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPAi.Cores.Scoreboard
{
    /// <summary>
    /// Represents an item in the scoreboard for MPAiSound. An item has two fields; the vowel it represents, and the correctness percentage.
    /// </summary>
    public class MPAiSoundScoreBoardItem
    {
        string vowel;

        /// <summary>
        /// Wrapper property for the vowel this item shows the score for.
        /// </summary>
        public string Vowel
        {
            get { return vowel; }
        }

        float correctnessPercentage;

        /// <summary>
        /// Wrapper property for the correctness percentage of this item, represented as a float.
        /// </summary>
        public float CorrectnessPercentage
        {
            get { return correctnessPercentage; }
        }

        public MPAiSoundScoreBoardItem(string vowel, float correctnessPercentage)
        {
            this.vowel = vowel;
            this.correctnessPercentage = correctnessPercentage;
        }
    }

    /// <summary>
    /// Represents an MPAi Sound Scoreboard Session. A session is created for every time a user clicks 'analyse and go back' in the vowel target.
    /// A session has a date and time, indicating when it was created, a list of MPAiSoundScoreboardItems that it contains, and an overall
    /// correctness percentage for the session.
    /// </summary>
    public class MPAiSoundScoreBoardSession
    {
        private DateTime dateAndTime;

        /// <summary>
        /// Wrapper property for the date and time the session was created.
        /// </summary>
        public DateTime DateAndTime
        {
            get { return dateAndTime; }
        }

        private List<MPAiSoundScoreBoardItem> content = new List<MPAiSoundScoreBoardItem>();

        /// <summary>
        /// Read only wrapper property for the list of scoreboard items contained by this session.
        /// </summary>
        public List<MPAiSoundScoreBoardItem> Content
        {
            get { return content; }
        }

        float overallCorrectnessPercentage;

        /// <summary>
        /// Wrapper property for the overall correctness percentage of the session, represented by a float.
        /// </summary>
        public float OverallCorrectnessPercentage
        {
            get { return overallCorrectnessPercentage; }
            set { overallCorrectnessPercentage = value; }
        }
        /// <summary>
        /// Constructor for the session, taking the date and time as an argument.
        /// </summary>
        /// <param name="dateAndTime"></param>
        public MPAiSoundScoreBoardSession(DateTime dateAndTime)
        {
            this.dateAndTime = dateAndTime;
        }

        /// <summary>
        /// Constructor for the session, taking the date and time as an argument, and also a list of scoreboard items to initialise the content with.
        /// </summary>
        /// <param name="dateAndTime"></param>
        /// <param name="content"></param>
        public MPAiSoundScoreBoardSession(DateTime dateAndTime, List<MPAiSoundScoreBoardItem> content)
        {
            this.dateAndTime = dateAndTime;
            this.content = content;
        }

        /// <summary>
        /// This method is used to add a scoreboard item to the content list, by specifying the fields of the scorebord item.
        /// </summary>
        /// <param name="vowel"></param>
        /// <param name="correctnessPercentage"></param>
        public void AddScoreBoardItem(string vowel, float correctnessPercentage)
        {
            Content.Add(new MPAiSoundScoreBoardItem(vowel, correctnessPercentage));
        }


        /// <summary>
        /// Returns an int indicating the ordering of two scoreboard sessions, by date.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int ComparisionByDate(MPAiSoundScoreBoardSession x, MPAiSoundScoreBoardSession y)
        {
            return DateTime.Compare(y.DateAndTime, x.DateAndTime);
        }

        public bool IsEmpty()
        {
            return !Content.Any();
        }
    }

    /// <summary>
    /// Represents a Scoreboard for MPAiSound. Each user has one such scoreboard, which persists between MPAi session by writing to a file, using the MPAiSoundScoreBoardLoader class.
    /// A scoreboard is registerd to a single user, and contains a list of sessions.
    /// </summary>
    public class MPAiSoundScoreBoard
    { 
        private MPAiUser user;

        /// <summary>
        /// Read only wrapper property for the user associated with the scoreboard.
        /// </summary>
        public MPAiUser User
        {
            get { return user; }
        }
   
        private List<MPAiSoundScoreBoardSession> sessions = new List<MPAiSoundScoreBoardSession>();

        /// <summary>
        /// Read only wrapper property for the list of scoreboard items contained by this session. 
        /// </summary>
        public List<MPAiSoundScoreBoardSession> Sessions
        {               
            get         
            {           
                sessions.Sort(MPAiSoundScoreBoardSession.ComparisionByDate);
                return sessions;
            }           
        }               
                        
        public MPAiSoundScoreBoard(MPAiUser user)
        {               
            this.user = user;
        }

        /// <summary>
        /// Creates a new scoreboard session and returns a reference to the caller, so they can add to the session.
        /// </summary>
        /// <returns></returns>
        public MPAiSoundScoreBoardSession NewScoreBoardSession()
        {               
            MPAiSoundScoreBoardSession session = new MPAiSoundScoreBoardSession(DateTime.Now);
            sessions.Add(session);
            return session;
        }               
                        
        /// <summary>
        /// Creates a new scoreboard session based on the parameters and returns a reference to the caller, so they can add to the session.
        /// </summary>
        /// <param name="dateAndTime"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public MPAiSoundScoreBoardSession NewScoreBoardSession(DateTime dateAndTime, List<MPAiSoundScoreBoardItem> content)
        {               
            MPAiSoundScoreBoardSession session = new MPAiSoundScoreBoardSession(dateAndTime, content);
            sessions.Add(session);
            return session;
        }               
                        
        /// <summary>
        /// Used to save the scoreboard; calls the appropriatem method in MPAiSoundScoreboardLoader.
        /// </summary>
        public void SaveScoreBoardToFile()
        {               
            MPAiSoundScoreboardLoader.SaveScoreboard(this);
        }

        /// <summary>
        /// Returns true if the scoreboard is 'empty.' A scoreboard is empty if it either contains no sessions, or if every session it has contains no items.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            foreach (MPAiSoundScoreBoardSession session in Sessions)
            {
                if (!session.IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
