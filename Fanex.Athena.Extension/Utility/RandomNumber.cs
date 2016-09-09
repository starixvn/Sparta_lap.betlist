// -----------------------------------------------------------------------
// <copyright file="RandomNumber.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class RandomNumber
    {
        /// <summary>
        /// Generate a seed to use when creating new "Random" objects. Will produce better random results.
        /// </summary>
        /// <returns>Random number.</returns>
        private static int GenerateRandomSeed()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                randomBytes[1] << 16 |
                randomBytes[2] << 8 |
                randomBytes[3];

            return seed;
        }

        /// <summary>
        /// Returns a random integer within a specified range. 
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Random number.</returns>
        public static int GenerateRandomInt32(int minValue, int maxValue)
        {
            Random randomClass = new Random(GenerateRandomSeed());
            return randomClass.Next(minValue, maxValue);
        }
    }
}
