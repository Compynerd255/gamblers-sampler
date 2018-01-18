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
    }

    /// <summary>
    /// Represents an <see cref="ISampler{T}"/> whose state can be saved between executions
    /// </summary>
    /// <remarks>
    /// All ISampler objects should strive to implement <see cref="IExportableSampler{T, ExportT}"/>,
    /// because many applications of the sampler need to save state to create the proper
    /// intuitive perception of luck.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="ExportT"></typeparam>
    public interface IExportableSampler<T, ExportT> : ISampler<T>
    {
        /// <summary>
        /// Exports the state of the sampler to a serializable object
        /// so that it can be re-created between executions
        /// </summary>
        /// <returns></returns>
        ExportT ExportState();
    }
}
