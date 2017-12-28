using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// Provides a set of common samplers
    /// </summary>
    public static class Samplers
    {
        /// <summary>
        /// Loads a Gambler's Sampler from the given sampler state
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static ISampler<T> FromExportState<T>(SamplerExportState<T> state)
        {
            return new GamblersSampler<T>(state);
        }

        /// <summary>
        /// Provides a biased coin that forever retains the given odds
        /// </summary>
        /// <param name="headsWeight">The probability of getting heads</param>
        /// <returns></returns>
        public static ISampler<bool> UniformBiasedCoin(double headsWeight)
        {
            return new GamblersSampler<bool>(new WeightedOutcome<bool>[]{
                new WeightedOutcome<bool>(true, headsWeight),
                new WeightedOutcome<bool>(false, 1 - headsWeight)
            }, 1);
        }

        /// <summary>
        /// Provides a biased coin subject to the Gambler's Fallacy
        /// </summary>
        /// <param name="headsWeight">The probability of getting heads</param>
        /// <returns></returns>
        public static ISampler<bool> GamblersBiasedCoin(double headsWeight)
        {
            return new GamblersSampler<bool>(new WeightedOutcome<bool>[]{
                new WeightedOutcome<bool>(true, headsWeight),
                new WeightedOutcome<bool>(false, 1 - headsWeight)
            }, 0.1);
        }

        /// <summary>
        /// Provides a sampler representing a uniform six-sided die
        /// </summary>
        /// <returns></returns>
        public static ISampler<int> UniformD6()
        {
            return new GamblersSampler<int>(Range(1, 6), 1);
        }

        /// <summary>
        /// Provides a six-sided die subject to the Gambler's Fallacy
        /// </summary>
        /// <returns></returns>
        public static ISampler<int> GamblersD6()
        {
            return new GamblersSampler<int>(Range(1, 6), 0.1);
        }

        /// <summary>
        /// Provides a uniform twenty-sided die
        /// </summary>
        /// <returns></returns>
        public static ISampler<int> UniformD20()
        {
            return new GamblersSampler<int>(Range(1, 20), 1);
        }

        /// <summary>
        /// Provides a twenty-sided die subject to the Gambler's Fallacy
        /// </summary>
        /// <returns></returns>
        public static ISampler<int> GamblersD20()
        {
            return new GamblersSampler<int>(Range(1, 20), 0.1);
        }

        /// <summary>
        /// Generates an inclusive range of the two indicated numbers
        /// </summary>
        private static IEnumerable<int> Range(int first, int last)
        {
            for (int i = first; i <= last; i++)
            {
                yield return i;
            }
        }
    }
}
