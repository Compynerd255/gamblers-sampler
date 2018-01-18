using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// An object that creates a particular <see cref="ISampler{T}"/>
    /// in an exact distortion state, allowing the state of the sampler
    /// to be saved to disk between executions.
    /// </summary>
    /// <remarks>
    /// Any implementation of this class should also be serializable.
    /// It is usually best if this class is a member class of the serializer,
    /// allowing it to access private member variables.
    /// </remarks>
    /// <typeparam name="SamplerType">The type of sampler to be re-created from this class</typeparam>
    public interface ISamplerExportState<SamplerType, T> where SamplerType : ISampler<T>
    {
        // TODO: Also see if the underlying random sequencer
        // can also be saved with this class. This will allow
        // a seeded sequencer to maintain its sequence
        // between executions.
        // It is possible that you will have to write your own
        // uniform sampler - fortunately, it only has to generate
        // doubles for it to work.

        SamplerType RecreateSampler();
    }
}
