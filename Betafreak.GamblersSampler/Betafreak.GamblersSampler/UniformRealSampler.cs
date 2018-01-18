using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    class UniformRealSampler : IExportableSampler<double, int>
    {
        public double Next()
        {
            throw new NotImplementedException();
        }

        public void Force(double outcome)
        {
            // do nothing (one sample doesn't change the next)
        }

        public int ExportState()
        {
            // just return the current state of the seed object
            throw new NotImplementedException();
        }
    }
}
