using Biocell.Core;
using System;
using System.Linq;

namespace BiocellGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static bool IsDevelopmentMode { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Debug.Log("Biocell (Version " + Properties.Resources.Version + "-" + Properties.Resources.LifeCycle + ")");
            EvaluateCommandLineArgs(Environment.GetCommandLineArgs());

            using (var game = new Game())
            {
                game.Exiting += OnGameExit;
                game.Run();
            }
        }

        private static void OnGameExit(object sender, EventArgs e)
        {
            Debug.SaveLog(Settings.BiocellGameSettings.Default.LogFile);
        }

        private static void EvaluateCommandLineArgs(string[] args)
        {
            var executionPath = args[0];

            var argsList = args.ToList();
            argsList.RemoveAt(0); // Without the execution path item

            foreach (var arg in argsList)
            {
                switch (arg)
                {
                    case "-devmode":
                        IsDevelopmentMode = true;
                        Debug.Log("The development mode got activated.");
                        break;

                    default:
                        Debug.Log("The command line argument \"" + arg + "\" is unknown.");
                        break;
                }
            }
        }
    }
#endif
}
