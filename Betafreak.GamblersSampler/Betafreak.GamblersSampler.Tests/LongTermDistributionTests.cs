using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Betafreak.GamblersSampler.Tests
{
    [TestClass]
    public class LongTermDistributionTests
    {
        [TestMethod]
        public void FairCoin_ShowsFairness()
        {
            GamblersCoin_ShowsDistribution(.5, 500);
        }

        [TestMethod]
        public void BiasedCoin_ShowsBias()
        {
            GamblersCoin_ShowsDistribution(.8, 500);
            GamblersCoin_ShowsDistribution(.3, 500);
            GamblersCoin_ShowsDistribution(.95, 500);
        }

        private void GamblersCoin_ShowsDistribution(double p, int n)
        {
            ISampler<bool> coin = Samplers.GamblersBiasedCoin(p);
            int trueCount = 0;
            int falseCount = 0;
            for (int i = 0; i < n; i++)
            {
                if (coin.Next()) trueCount++;
                else falseCount++;
            }

            double stddev = Math.Sqrt(n * p * (1 - p));
            double stddevDistance = Math.Abs((double)trueCount - n * p) / stddev;

            Assert.IsTrue(Math.Abs(stddevDistance) < 3,
                $"Coin sampler {stddevDistance} standard deviations from the mean");
        }
    }
}
