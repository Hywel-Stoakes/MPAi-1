using System.Collections.Generic;

namespace MPAi.Modules
{
    /// <summary>
    /// GenderType represents the gender type of the speaker - whether they are masculine or feminine.
    /// </summary>
    public enum GenderType
    {
        FEMININE, MASCULINE
    }

    /// <summary>
    /// LanguageType represents the language type of the speaker - whether they are a native or modern speaker of Maori.
    /// </summary>
    public enum LanguageType
    {
        NATIVE, MODERN
    }

    /// <summary>
    /// VoiceType is a class that wraps GenderType and LanguageType to hold the overall voice type of the speaker.
    /// It is used by MPAiUser to represent the voice type of the current user.
    /// </summary>
    public class VoiceType
    {
        private GenderType gender;

        /// <summary>
        /// Wrapper property for the GenderType of the voice type.
        /// </summary>
        public GenderType Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        /// <summary>
        /// Wrapper property for the LanguageType of the voice type.
        /// </summary>
        private LanguageType language;
        public LanguageType Language
        {
            get
            {
                return language;
            }

            set
            {
                language = value;
            }
        }

        /// <summary>
        /// Constructor for VoiceType, which instatiates the gender and language with the designated values.
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="language"></param>
        public VoiceType(GenderType gender, LanguageType language)
        {
            this.language = language;
            this.gender = gender;
        }

        /// <summary>
        /// Returns the appropriate enum for the inputted string; null if string is not in dictionary.
        /// </summary>
        /// <param name="voiceString">The string to convert into VoiceType</param>
        public static VoiceType getVoiceTypeFromString(string voiceString)
        {
            switch(voiceString)
            {
                case "MASCULINE_NATIVE":
                    return new VoiceType(GenderType.MASCULINE, LanguageType.NATIVE);
                case "MASCULINE_MODERN":
                    return new VoiceType(GenderType.MASCULINE, LanguageType.MODERN);
                case "FEMININE_NATIVE":
                    return new VoiceType(GenderType.FEMININE, LanguageType.NATIVE);
                case "FEMININE_MODERN":
                    return new VoiceType(GenderType.FEMININE, LanguageType.MODERN);
            }
            return null;
        }

        /// <summary>
        /// Returns the appropriate string for the inputted VoiceType. This string is used for writing settings and
        /// underlying uses of voicetype; use GetDisplayName for displaying to user.
        /// </summary>
        /// <param name="voiceString">The enum to convert into a string.</param>
        public static string getStringFromVoiceType(VoiceType voiceType)
        {
            if (voiceType.Gender.Equals(GenderType.MASCULINE) && voiceType.Language.Equals(LanguageType.NATIVE))
            {
                return "MASCULINE_NATIVE";
            }
            else if (voiceType.Gender.Equals(GenderType.FEMININE) && voiceType.Language.Equals(LanguageType.NATIVE))
            {
                return "FEMININE_NATIVE";
            }
            else if (voiceType.Gender.Equals(GenderType.MASCULINE) && voiceType.Language.Equals(LanguageType.MODERN))
            {
                return "MASCULINE_MODERN";
            }
            else if (voiceType.Gender.Equals(GenderType.FEMININE) && voiceType.Language.Equals(LanguageType.MODERN))
            {
                return "FEMININE_MODERN";
            }

            return null;
        }

        /// <summary>
        /// Returns the display name for a voice type; this should be used when displaying voice type to the user.
        /// </summary>
        /// <param name="voiceType"></param>
        /// <returns></returns>
        public static string getDisplayNameFromVoiceType(VoiceType voiceType)
        {
            if (voiceType.Gender.Equals(GenderType.MASCULINE) && voiceType.Language.Equals(LanguageType.NATIVE))
            {
                return "Masculine, Kaumatua Māori";       
            }
            else if (voiceType.Gender.Equals(GenderType.FEMININE) && voiceType.Language.Equals(LanguageType.NATIVE))
            {
                return "Feminine, Kuia Māori";
            }
            else if (voiceType.Gender.Equals(GenderType.MASCULINE) && voiceType.Language.Equals(LanguageType.MODERN))
            {
                return "Masculine, Modern Māori";
            }
            else if (voiceType.Gender.Equals(GenderType.FEMININE) && voiceType.Language.Equals(LanguageType.MODERN))
            {
                return "Feminine, Modern Māori";
            }
            return "No Voice Type";
        }
    }

    /// <summary>
    /// This static class is used to display the appropriate version of 'native' maori to the user.
    /// </summary>
    public static class DisplayVoice
    {
        /// <summary>
        /// This method returns a string, used to display the appropriate version of 'native' maori to the user.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string DisplayNative(GenderType gender)
        {
            switch (gender)
            {
                case GenderType.FEMININE:
                    return "Kuia Māori";
                case GenderType.MASCULINE:
                    return "Kaumatua Māori";
                default:
                    return null;
            }
        }
    }
}