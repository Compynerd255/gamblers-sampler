using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// General implementation of <see cref="ISampler"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GamblersSampler<T> : ISampler<T>
    {
        public GamblersSampler(IEnumerable<T> outcomes, double severity)
        {
            throw new NotImplementedException();
        }

        public GamblersSampler(IEnumerable<WeightedOutcome<T>> outcomes, double severity)
        {
            throw new NotImplementedException();
        }

        public GamblersSampler(SamplerExportState<T> state)
        {
            throw new NotImplementedException();
        }

        public T Next()
        {
            throw new NotImplementedException();
        }

        public SamplerExportState<T> ExportState()
        {
            throw new NotImplementedException();
        }

        public void Force(T outcome)
        {
            throw new NotImplementedException();
        }
    }
}
