using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Betafreak.GamblersSampler.Tests
{
    [TestClass]
    public class LongTermDistributionTests
    {
        [TestMethod]
        public void UniformFairCoin_ShowsFairness()
        {
            UniformCoin_ShowsDistribution(.5, 500);
        }

        [TestMethod]
        public void UniformBiasedCoin_ShowsBias()
        {
            UniformCoin_ShowsDistribution(.8, 500);
            UniformCoin_ShowsDistribution(.3, 500);
            UniformCoin_ShowsDistribution(.95, 500);
        }

        [TestMethod]
        public void GamblersFairCoin_ShowsFairness()
        {
            GamblersCoin_ShowsDistribution(.5, 500);
        }

        [TestMethod]
        public void GamblersBiasedCoin_ShowsBias()
        {
            GamblersCoin_ShowsDistribution(.8, 500);
            GamblersCoin_ShowsDistribution(.3, 500);
            GamblersCoin_ShowsDistribution(.95, 500);
        }

        private void UniformCoin_ShowsDistribution(double p, int n)
        {
            Coin_ShowsDistribution(Samplers.UniformBiasedCoin(p), p, n);
        }

        private void GamblersCoin_ShowsDistribution(double p, int n)
        {
            Coin_ShowsDistribution(Samplers.GamblersBiasedCoin(p), p, n);
        }

        private void Coin_ShowsDistribution(ISampler<bool> coin, double p, int n)
        {
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
