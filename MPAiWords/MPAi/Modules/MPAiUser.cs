using MPAi.Cores.Scoreboard;
using MPAi.DatabaseModel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace MPAi.Modules
{
    /// <summary>
    /// Class representing a user in the MPAi system.
    /// </summary>
    public class MPAiUser
    {
        private string userName;
        private string passWord;
        private VoiceType voiceType;
        private MPAiSpeakScoreBoard speakScoreboard;
        private Speaker speaker;
        private bool isAdmin;
        private bool originalAdmin;

        /// <summary>
        /// Wrapper property for the user's username, allowing access from outside the class.
        /// </summary>

        [DisplayName("UserName")]
        public string UserID
        {
            get { return userName; }
            set
            {
                // prevent the user from changing the name of the admin
                if (!originalAdmin)
                {
                    userName = value;
                }
            }
        }
        /// <summary>
        /// Wrapper property for the user's password, allowing access outside of the class.
        /// </summary>
        [DisplayName("Password")]
        public string UserPswd
        {
            get { return passWord; }
            set
            {
                passWord = value;
            }
        }

        /// <summary>
        /// Wrapper property for the user's voice type, allowing access outside of the class.
        /// </summary>
        [DisplayName("VoiceType")]
        public VoiceType Voice
        {
            get { return voiceType; }
            set
            {
                voiceType = value;
            }
        }
        
        /// <summary>
        /// Wrapper property for the administrator status of the user, allowing ti to be checked from outside the class.
        /// </summary>
        [DisplayName("IsAdministrator")]
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }

            set
            {
                isAdmin = value;
            }
        }

        /// <summary>
        /// Wrapper property for the original administrator status of the user, allowing ti to be checked from outside the class.
        /// </summary>
        [DisplayName("OriginalAdmin")]
        public bool OriginalAdmin
        {
            get
            {
                return originalAdmin;
            }
        }

        public Speaker Speaker
        {
            get
            {
                setSpeakerFromVoiceType();
                return speaker;
            }

            set
            {
                speaker = value;
            }
        }
        /// <summary>
        /// Wrapper property for the user's username, allowing access from outside the class.
        /// </summary>
        [DisplayName("SpeakScoreBoard")]
        public MPAiSpeakScoreBoard SpeakScoreboard
        {
            get
            {
                loadScoreBoards();
                return speakScoreboard;
            }
        }

        private MPAiModel InitializeDBModel(MPAiModel DBModel)
        {
            DBModel.Database.Initialize(false);
            DBModel.Recording.Load();
            DBModel.Speaker.Load();
            DBModel.Category.Load();
            DBModel.Word.Load();
            DBModel.SingleFile.Load();
            return DBModel;
        }

        /// <summary>
        /// Constructor for the MPAiUser class, with a default value for voice type.
        /// </summary>
        /// <param name="name">The new user's username</param>
        /// <param name="code">The new user's password</param>
        public MPAiUser(string name, string code) :
            this(name, code, new VoiceType(GenderType.MASCULINE, LanguageType.NATIVE))
        {
            // As Peter Keegan's recordings are the only one we have for testing, users default to MASCULINE_NATIVE.
        }

        public MPAiUser(string name, string code, bool admin) :
            this(name, code, new VoiceType(GenderType.MASCULINE, LanguageType.NATIVE), admin)
        {
            // As Peter Keegan's recordings are the only one we have for testing, users default to MASCULINE_NATIVE.
        }


        /// <summary>
        /// Constructor for the MPAiUser class.
        /// </summary>
        /// <param name="name">The new user's username</param>
        /// <param name="code">The new user's password</param>
        public MPAiUser(string name, string code, VoiceType voiceType) :
            this(name, code, voiceType, false)
        {
           
        }

        public MPAiUser(string name, string code, VoiceType voiceType, bool admin):
            this(name, code, voiceType, admin, false)
        {
            
        }

        public MPAiUser(string name, string code, VoiceType voiceType, bool admin, bool originalAdmin)
        {
            if(!admin && originalAdmin)
            {
                throw new ArgumentException("Cannot create a user who is not an admin but is an original admin.");
            }
            userName = name;
            passWord = code;
            Voice = voiceType;
            this.isAdmin = admin;
            this.originalAdmin = originalAdmin;
        }

        public void setSpeakerFromVoiceType()
        {
            using (MPAiModel DBModel = new MPAiModel())
            {
                InitializeDBModel(DBModel);

                if(voiceType.Gender.Equals(GenderType.MASCULINE) && voiceType.Language.Equals(LanguageType.NATIVE))
                {
                    speaker = DBModel.Speaker.Local.Where(x => x.SpeakerId == 2).SingleOrDefault();
                }
                else if (voiceType.Gender.Equals(GenderType.FEMININE) && voiceType.Language.Equals(LanguageType.NATIVE))
                {
                    speaker = DBModel.Speaker.Local.Where(x => x.SpeakerId == 1).SingleOrDefault();
                }
                else if (voiceType.Gender.Equals(GenderType.MASCULINE) && voiceType.Language.Equals(LanguageType.MODERN))
                {
                    speaker = DBModel.Speaker.Local.Where(x => x.SpeakerId == 4).SingleOrDefault();
                }
                else if (voiceType.Gender.Equals(GenderType.FEMININE) && voiceType.Language.Equals(LanguageType.MODERN))
                {
                    speaker = DBModel.Speaker.Local.Where(x => x.SpeakerId == 3).SingleOrDefault();
                }
            }
        }

        private void loadScoreBoards()
        {
            if (speakScoreboard == null)
            {
                if (File.Exists(MPAiSpeakScoreboardLoader.SpeakScoreboardFileAddress(this)))
                {
                    speakScoreboard = MPAiSpeakScoreboardLoader.LoadScoreboard(this);
                }
                else
                {
                    speakScoreboard = new MPAiSpeakScoreBoard(this);
                }
            }
        }

        /// <summary>
        /// Changes the voice type to feminine.
        /// </summary>
        public void changeVoiceToFeminine()
        {
            Voice.Gender = GenderType.FEMININE;
        }

        /// <summary>
        /// Changes the voice type to Masculine.
        /// </summary>
        public void changeVoiceToMasculine()
        {
            Voice.Gender = GenderType.MASCULINE;
        }

        /// <summary>
        /// Changes the voice type to Native.
        /// </summary>
        public void changeVoiceToNative()
        {
            Voice.Language = LanguageType.NATIVE;
        }
        /// <summary>
        /// Changes the voice type to Modern.
        /// </summary>
        public void changeVoiceToModern()
        {
            Voice.Language = LanguageType.MODERN;
        }
        /// <summary>
        /// Checks if the input string matches this user's password. Case sensitive.
        /// </summary>
        /// <param name="code">The string to check against the password.</param>
        /// <returns>True if the password is correct, false otherwise.</returns>
        public bool codeCorrect(string code)
        {
            return (code.Equals(passWord));
        }

        /// <summary>
        /// Getter for username, Not case senstive.
        /// </summary>
        /// <returns>The lower-case username, as a string.</returns>
        public string getName()
        {
            return userName.ToLower();
        }

        public string GetCorrectlyCapitalisedName()
        {
            string[] parts = userName.Split(' ');
            string capUserName = "";

            foreach (string word in parts)
            {
                if(!String.IsNullOrEmpty(word))
                {
                    if(capUserName.Equals(""))
                    {
                        capUserName += word.First().ToString().ToUpper() + word.Substring(1);
                    }
                    else
                    {
                        capUserName += " " + word.First().ToString().ToUpper() + word.Substring(1);
                    }
                }
            }
            return capUserName;
        }

        /// <summary>
        /// Getter for the Users inputed username. Case sensitive.
        /// </summary>
        /// <returns>The case sensitive username, as a string</returns>
        public string getRawName() {
            return userName;
        }
        /// <summary>
        /// Getter for password. Case Senstive.
        /// </summary>
        /// <returns>The password, as a string.</returns>
        public string getCode()
        {
            return passWord;
        }
        /// <summary>
        /// Override for equals().
        /// Two users with the same username are considered the same user.
        /// </summary>
        /// <param name="obj">The object to be compared to the current user.</param>
        /// <returns>True if the user and the object are the same thing, false otherwise.</returns>
        public override bool Equals(System.Object obj)
        {
            if (obj is MPAiUser)
            {
                MPAiUser otherUser = (MPAiUser)obj;
                if (userName == null || passWord == null)
                {
                    return false;
                }
                return (getName() == otherUser.getName());
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Override for GetHashCode(). Functions the same.
        /// </summary>
        /// <returns>The hashcode for this object, as an int.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Override for ToString(), returning the username - this should be unique for each user.
        /// </summary>
        /// <returns>The username, as a string.</returns>
        public override string ToString()
        {
            return userName;
        }
    }
}
