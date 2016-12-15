﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPAid.Cores
{
    /// <summary>
    /// Class that generates advice based on analysed recordings.
    /// </summary>
    class PronouciationAdvisor
    {
        /// <summary>
        /// Can be called from outside to generate a string of advice to present to the user.
        /// </summary>
        /// <param name="recording">The name of the recording being analysed, as a string.</param>
        /// <param name="target">The intended vowel/word, as a string.</param>
        /// <param name="recognized">The actual, identified vowel/word, as a string.</param>
        /// <returns>The advice for the user, as a string.</returns>
        public string Advise(string recording, string target, string recognized)
        {
            string advice = "Errors in lexicon.txt!";   // Handle error cases.
            if (lexicon.ReadLexicon())
            {
                if (lexicon.dictionary.Keys.Contains(recognized))
                {
                    string recognizedPronouciation = lexicon.dictionary[recognized];
                    advice = string.Format(@"Your recording '{0}' is analyzed as '{1}', having pronounciation '{2}'.{3}{4}", recording, recognized, recognizedPronouciation, Environment.NewLine, Environment.NewLine);
                    if (!recognized.Equals(target) && !string.IsNullOrEmpty(target))
                    {
                        advice += CompareWords(target, recognized);
                    }
                }
                else
                {
                    throw new Exception("The word you choose/input is not in the dictionary!");
                }

            }
            return advice;
        }
        /// <summary>
        /// Compares two words, and if they are different, points out the syllables that aren't correct.
        /// </summary>
        /// <param name="target">The correct string for comparison.</param>
        /// <param name="recognized">The user's string for comparison.</param>
        /// <returns>A string representing pronunciation advice based on the two input strings.</returns>
        private string CompareWords(string target, string recognized)
        {
            string advice = string.Empty;
            string targetPronouciation = lexicon.dictionary[target];
            advice += string.Format(@"This is in comparision to the target word '{0}', whose pronunciation is '{1}'.{2}{3}", target, targetPronouciation, Environment.NewLine, Environment.NewLine);

            List<KeyValuePair<string, string>> pronounciationList = AnalyzePronouciation(targetPronouciation, lexicon.dictionary[recognized]);
            if (pronounciationList.Count > 0)
            {
                advice += string.Format(@"This is how you pronounced the word:{0}", Environment.NewLine);
                foreach (KeyValuePair<string, string> pair in pronounciationList)
                {
                    if(pair.Key.Equals(pair.Value))
                    {
                        advice += string.Format(@"'{0}' was correctly pronounced.{1}", pair.Key, Environment.NewLine);
                    }
                    else if(pair.Value.Equals(""))
                    {
                        advice += string.Format(@"'{0}' was not said.{1}", pair.Key, Environment.NewLine);
                    }
                    else
                    {
                        advice += string.Format(@"'{0}' was mispronounced as '{1}'.{2}", pair.Key, pair.Value, Environment.NewLine);
                    }
                }
            }
            return advice;
        }
        /// <summary>
        /// Steps through each syllable of the recognised word and identifies which parts have been mispronuounced.
        /// </summary>
        /// <param name="target">The correct pronunciation.</param>
        /// <param name="recognized">The user's pronunciation.</param>
        /// <returns>A dictionary mapping the correct pronunciation to the user's pronunciation, for each syllable they got wrong.</returns>
        private List<KeyValuePair<string, string>> AnalyzePronouciation(string target, string recognized)
        {
            List<KeyValuePair<string, string>> pronounciationList = new List<KeyValuePair<string, string>>();

            string[] targetPhones = target.Split(' ');
            string[] recognizedPhones = recognized.Split(' ');

            for (int i = 0; i < targetPhones.Length; i++)
            {
                if (i < recognizedPhones.Length)
                {
                    pronounciationList.Add(new KeyValuePair<string, string>(targetPhones[i], recognizedPhones[i]));
                }
                else
                {
                    pronounciationList.Add(new KeyValuePair<string, string>(targetPhones[i], ""));
                }
            }
            return pronounciationList;
        }

        private LexiconReader lexicon = new LexiconReader();
    }
    /// <summary>
    /// A class to interpret the lexicon.txt file into a dictionary object.
    /// </summary>
    class LexiconReader
    {
        /// <summary>
        /// Publically accessible dictionary of words and their pronunciations.
        /// </summary>
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        /// <summary>
        /// Opens the lexicon.txt file and reads each element listed there into the dictionary. 
        /// </summary>
        /// <returns>True if lexicon.txt exists, is in the right place, and was read successfully, false otherwise.</returns>
        public bool ReadLexicon()
        {
            try
            {
                string filepath = Path.Combine(Properties.Settings.Default.HTKFolder, @"Dictionaries", @"lexicon.txt");
                if (File.Exists(filepath))
                {
                    dictionary.Clear();
                    using (FileStream fs = new FileStream(filepath, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                        {

                            string item;
                            while ((item = sr.ReadLine()) != null)
                            {
                                if (string.IsNullOrEmpty(item)) continue;
                                string word = item.Substring(0, item.IndexOf('[')).Trim(' ');
                                string pronouciation = item.Substring(item.IndexOf(']') + 1).Trim(' ');
                                if (!string.IsNullOrEmpty(word))
                                {
                                    if(!dictionary.ContainsKey(word)) dictionary.Add(word, pronouciation);
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
            return false;
        }
    }
}
