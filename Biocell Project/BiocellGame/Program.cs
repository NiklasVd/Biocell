using Biocell.Core;
using System;

namespace BiocellGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public const string version = "1.00-a",
            debugFilePath = "debug.log";

        public static bool IsDevelopmentMode { get; private set; }
        public static bool IsTelemetryActivated { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Debug.Log("Biocell (Version " + version + ")");

            // Command line evaluation
            var commandLineArgs = Environment.GetCommandLineArgs();
            foreach (var commandLineArg in commandLineArgs)
            {
                switch (commandLineArg)
                {
                    case "-devmode":
                        IsDevelopmentMode = true;
                        Debug.Log("Activated development mode.");
                        break;

                    case "-telemetry":
                        IsTelemetryActivated = true;
                        Debug.Log("Activated telemetry behaviour. The client will send anonymous usage data and statistics to the central servers.");
                        break;

                    //default:
                    //    Debug.Log("The command line argument \"" + commandLineArg + "\" is unknown.");
                    //    break;
                }
            }

            using (var game = new Game())
            {
                game.Exiting += OnGameExiting;
                game.Run();
            }
        }

        private static void OnGameExiting(object sender, EventArgs e)
        {
            Debug.SaveLog(debugFilePath);
        }
    }
#endif
}
