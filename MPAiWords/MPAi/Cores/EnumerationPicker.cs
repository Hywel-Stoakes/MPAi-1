using System;
using System.Collections.Generic;
using System.Linq;

namespace MPAi.Cores
{
    /// <summary>
    /// Override of existing IEnumberable methods, allowing custom actions to be performed on enumerable collections.
    /// </summary>
    public static class EnumerableExtension
    {
        private static int currentIndex = 0;
        /// <summary>
        /// Gets the next element in an enumerable. If the current index is the last, gets the first element in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of element in the enumerable.</typeparam>
        /// <param name="source">The enumerable to perform the operation on.</param>
        /// <returns>The next element in the enumerable, type T.</returns>
        public static T PickNext<T>(this IEnumerable<T> source)
        {
            currentIndex = currentIndex >= source.Count() ? 0 : currentIndex;
            return source.ElementAt(currentIndex++);
        }
        /// <summary>
        /// Gets a random element out of the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of element in the enumerable.</typeparam>
        /// <param name="source">The enumerable to perform the operation on.</param>
        /// <returns>The randomly selected element, type T.</returns>
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }
        /// <summary>
        /// Gets a number of random elements out of the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of element in the enumerable.</typeparam>
        /// <param name="source">The enumerable to perform the operation on.</param>
        /// <param name="count">The number of elements to get from the enumerable.</param>
        /// <returns>The randomly selected elements, as an enumerable of the same sort as the input.</returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }
        /// <summary>
        /// Randomises the order of the input enumerable.
        /// </summary>
        /// <typeparam name="T">The type of element in the enumerable.</typeparam>
        /// <param name="source">The enumerable to perform the operation on.</param>
        /// <returns>The input object, with all elements randomply shuffled.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
