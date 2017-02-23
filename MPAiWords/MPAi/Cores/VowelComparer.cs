using MPAi.DatabaseModel;
using System;
using System.Collections.Generic;
namespace MPAi.Cores
{
    /// <summary>
    /// Custom comparator for Word objects.
    /// </summary>
    public class VowelComparer : IComparer<Word>
    {
        /// <summary>
        /// Implementation of comparison method, using the element's index in the 'table' field as the grounds for comparison.
        /// </summary>
        /// <param name="x">The first value to compare.</param>
        /// <param name="y">The second value to compare.</param>
        /// <returns>1 if x is before y, -1 if x is after y, 0 if they are the same.</returns>
        public int Compare(Word x, Word y)
        {
            if (table.FindIndex(i => i == x.Name) > table.FindIndex(i => i == y.Name))          // If x is before y
            {
                return 1;
            }
            else if (table.FindIndex(i => i == x.Name) < table.FindIndex(i => i == y.Name))     // If x is after y
            {
                return -1;
            }
            else if (table.FindIndex(i => i == x.Name) == table.FindIndex(i => i == y.Name))    // If x is y
            {
                if (table.Contains(x.Name) || table.Contains(y.Name)) return 0; // If at least one of the two is in the table, they are the same.
                else return x.Name.CompareTo(y.Name);   // If one or both of the elements aren't in the table, compare them as strings.
            }
            return 0;   // Required, but unreachable.
        }
        /// <summary>
        /// List specifying the correct order for vowels to be sorted into.
        /// </summary>
        private List<String> table = new List<string>()
        {
            "o",
            "a",
            "e",
            "i",
            "u",
            "ae",
            "ai",
            "ao",
            "au",
            "ou"
        };
    }
}
