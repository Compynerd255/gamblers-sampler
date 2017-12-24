using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// Represents an outcome sampler with the ability to distort
    /// its probabilities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISampler<T>
    {
        /// <summary>
        /// Returns the next result from the random sampler,
        /// distorting probabilities as needed
        /// </summary>
        /// <returns></returns>
        T Next();

        /// <summary>
        /// Updates the probability distortion as if the given outcome was generated
        /// </summary>
        /// <remarks>
        /// If the given outcome is not found in the sampler,
        /// or if that outcome's current weight is 0, an exception is thrown
        /// and no change is made.
        /// </remarks>
        /// <param name="outcome"></param>
        void Force(T outcome);

        /// <summary>
        /// Exports the sampler's state to a serializable object
        /// </summary>
        /// <returns></returns>
        SamplerExportState<T> ExportState();
    }
}
