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
            ISampler<bool> uniform = Samplers.UniformBiasedCoin(.6);
            Console.WriteLine("Uniform Sample:");
            SimulateCoinFlips(uniform);

            ISampler<bool> gamblers = Samplers.GamblersBiasedCoin(.6);
            Console.WriteLine("Gambler's Sample:");
            SimulateCoinFlips(gamblers);

            Console.ReadLine();
            // Simulate a set of 20 
        }

        static void SimulateCoinFlips(ISampler<bool> coin)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write(coin.Next() ? 'H' : 'T');
                }
            }
            Console.WriteLine();
        }

        static void Simulate80PercentXCOMShots(ISampler<int> d20)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write(d20.Next() < 16 ? '*' : '_');
                }
            }
            Console.WriteLine();
        }
    }
}
