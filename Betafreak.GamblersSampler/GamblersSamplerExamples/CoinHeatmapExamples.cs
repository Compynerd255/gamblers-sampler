using Betafreak.GamblersSampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblersSamplerExamples
{
    static class CoinHeatmapExamples
    {
        public static void IllustrateCoinFlipRuns()
        {
            ISampler<bool> uniformCoin = Samplers.UniformFairCoin();
            Console.WriteLine("Uniform Sample:");
            IllustrateCoinFlipRunsOnSampler(uniformCoin);

            ISampler<bool> gamblersCoin = Samplers.GamblersFairCoin();
            Console.WriteLine("Gambler's Sample:");
            IllustrateCoinFlipRunsOnSampler(gamblersCoin);
        }

        static void IllustrateCoinFlipRunsOnSampler(ISampler<bool> coin)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            ConsoleColor[] colors =
            {
                ConsoleColor.White,
                ConsoleColor.Gray,
                ConsoleColor.Green,
                ConsoleColor.DarkGreen,
                ConsoleColor.DarkMagenta,
                ConsoleColor.DarkRed,
                ConsoleColor.Red
            };
            bool lastRunValue = coin.Next();
            int lastRunLength = 0;
            for (int i = 0; i < 80; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    bool value = coin.Next();
                    if (lastRunValue == value)
                    {
                        lastRunLength++;
                    }
                    else
                    {
                        lastRunLength = 0;
                    }
                    lastRunValue = value;
                    Console.ForegroundColor = colors[Math.Min(colors.Length - 1, lastRunLength)];
                    Console.Write(value ? 'H' : 'T');
                }
            }
            Console.ForegroundColor = oldColor;
            Console.WriteLine();
        }
    }
}
