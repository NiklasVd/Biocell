﻿using Biocell.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiocellApp
{
    static class Program
    {
        public const string version = "1.00-a",
            debugLogSaveFilePath = @"debug.log";

        static void Main()
        {
            using (var biocellGame = new BiocellGame())
            {
                Debug.Log("Biocell Game");
                Debug.Log("Version " + version + "\n");

                biocellGame.Exiting += OnBiocellGameExit;
                biocellGame.Run();
            }
        }

        private static void OnBiocellGameExit(object sender, EventArgs e)
        {
            Debug.SaveLog(debugLogSaveFilePath);
        }
    }
}