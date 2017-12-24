using System;
using System.Collections.Generic;
using System.Text;

namespace Betafreak.GamblersSampler
{
    /// <summary>
    /// Represents an ordered pair of an outcome and a probability weight
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct WeightedOutcome<T>
    {
        public T Outcome;
        public double Weight;

        public WeightedOutcome(T outcome, double weight)
        {
            this.Outcome = outcome;
            this.Weight = weight;
        }
    }
}
