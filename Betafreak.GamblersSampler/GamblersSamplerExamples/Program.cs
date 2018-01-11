﻿using Betafreak.GamblersSampler;
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
            CoinHeatmapExamples.IllustrateCoinFlipRuns();
            XCOMShotExamples.SimulateXCOMShots(80);

            Console.ReadLine();
        }
    }
}
