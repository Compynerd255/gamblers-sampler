using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    class UniformRealSampler : IExportableSampler<double, int>
    {
        int seed;
        Random random;

        public UniformRealSampler() : this(Environment.TickCount)
        {
            
        }

        public UniformRealSampler(int seed)
        {
            this.seed = seed;
            random = new Random(seed);
        }

        public double Next()
        {
            return random.NextDouble();
        }

        public void Force(double outcome)
        {
            // do nothing (one sample doesn't change the next)
        }

        public int ExportState()
        {
            // just return the generator's seed
            return seed;
        }
    }
}
