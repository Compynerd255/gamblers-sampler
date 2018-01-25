using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    class UniformRealGamblersSampler : ISampler<double>
    {
        // TODO: Generate a sampler to distort results based on previous inputs.
        // Each of the elements of this array represents a bit in an IEEE 754
        // double-precision floating point number.

        // To get random number between 0 and 1:
        // - Generate 52 random bits in a 64-bit integer
        // - OR it with 0x3ff000000000000 for the correct exponent and sign
        // - Convert to double using BitConverter.Int64BitsToDouble
        // - Subtract 1
        short[] state = new short[52];
        public double Next()
        {
            throw new NotImplementedException();
        }

        public void Force(double outcome)
        {
            throw new NotImplementedException();
        }
    }
}
