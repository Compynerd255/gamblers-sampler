using Betafreak.GamblersSampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblersSamplerExamples
{
    static class XCOMShotExamples
    {
        public static void SimulateXCOMShots(int percentHit)
        {
            ISampler<int> uniformCoin = Samplers.UniformD20();
            Console.WriteLine("Uniform Sample:");
            SimulateXCOMShotsOnDie(uniformCoin, (100 - percentHit) / 5);

            ISampler<int> gamblersCoin = Samplers.GamblersD20();
            Console.WriteLine("Gambler's Sample:");
            SimulateXCOMShotsOnDie(gamblersCoin, (100 - percentHit) / 5);
        }


        static void SimulateXCOMShotsOnDie(ISampler<int> d20, int minHitValue)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write(d20.Next() > minHitValue ? '*' : '_');
                }
            }
            Console.WriteLine();
        }
    }
}
