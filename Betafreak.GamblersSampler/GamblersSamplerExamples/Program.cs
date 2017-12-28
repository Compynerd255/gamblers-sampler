using Betafreak.GamblersSampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblersSamplerExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            ISampler<bool> uniformCoin = Samplers.UniformBiasedCoin(.6);
            Console.WriteLine("Uniform Sample:");
            SimulateCoinFlips(uniformCoin);

            ISampler<bool> gamblersCoin = Samplers.GamblersBiasedCoin(.6);
            Console.WriteLine("Gambler's Sample:");
            SimulateCoinFlips(gamblersCoin);

            ISampler<int> uniformD20 = Samplers.UniformD20();
            Console.WriteLine("Uniform Sample:");
            Simulate95PercentXCOMShots(uniformD20);

            ISampler<int> gamblersD20 = Samplers.GamblersD20();
            Console.WriteLine("Gambler's Sample:");
            Simulate95PercentXCOMShots(gamblersD20);

            Console.ReadLine();
        }

        static void SimulateCoinFlips(ISampler<bool> coin)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write(coin.Next() ? 'H' : 'T');
                }
            }
            Console.WriteLine();
        }

        static void Simulate95PercentXCOMShots(ISampler<int> d20)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write(d20.Next() > 1 ? '*' : '_');
                }
            }
            Console.WriteLine();
        }
    }
}
