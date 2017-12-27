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
            ISampler<int> uniform = Samplers.UniformD20();
            Console.WriteLine("Uniform Sample:");
            Simulate80PercentXCOMShots(uniform);

            ISampler<int> gamblers = Samplers.GamblersD20();
            Console.WriteLine("Gambler's Sample:");
            Simulate80PercentXCOMShots(gamblers);

            Console.ReadLine();
            // Simulate a set of 20 
        }

        static void Simulate80PercentXCOMShots(ISampler<int> d20)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.Write(d20.Next() > 16 ? '*' : '_');
                }
            }
            Console.WriteLine();
        }
    }
}
