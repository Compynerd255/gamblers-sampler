using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// Holds the state of the sampler between executions.
    /// All samplers that are re-created with an instance of this
    /// object are brought to exactly the same state they were
    /// in when this object was returned from
    /// <see cref="ISampler{T}.ExportState">ExportState</see>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SamplerExportState<T>
    {
        // TODO: Also see if the underlying random sequencer
        // can also be saved with this class. This will allow
        // a seeded sequencer to maintain its sequence
        // between executions.
        // It is possible that you will have to write your own
        // uniform sampler - fortunately, it only has to generate
        // doubles for it to work.

        WeightedOutcome<T>[] CurrentWeights;
        double Severity;
    }
}
