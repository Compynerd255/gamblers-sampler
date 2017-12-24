using System;
using Betafreak.GamblersSampler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Betafreak.GamblersSampler.Tests
{
    [TestClass]
    public class StateComparisonTests
    {
        [TestMethod]
        public void TestUniformRemainsSame()
        {
            var sampler = Samplers.UniformD6();
            sampler.Force(1);

            // TODO: Assert that all the probabilities are still the same
        }

        [TestMethod]
        public void TestGamblersChanges()
        {
            var sampler = Samplers.GamblersD6();
            sampler.Force(1);
            
            // TODO: Assert that the probability is now lower for 1
        }
    }
}
