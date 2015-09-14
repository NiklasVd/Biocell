using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Biocell.Core
{
    public static class Debug
    {
        private readonly static List<LogMessage> logMessages;
        public static List<LogMessage> LogMessages { get { return logMessages.ToList(); } }

        static Debug()
        {
            logMessages = new List<LogMessage>(500);
        }

        public static void Log(string text,
            string sourceInfoMemberName = "", string sourceInfoFilePath = "", int sourceInfoLineNumber = -1)
        {
            LogMessage(text, LogMessageType.Normal, sourceInfoMemberName, sourceInfoFilePath, sourceInfoLineNumber);
        }
        public static void LogWarning(string text,
            string sourceInfoMemberName = "", string sourceInfoFilePath = "", int sourceInfoLineNumber = -1)
        {
            LogMessage(text, LogMessageType.Warning, sourceInfoMemberName, sourceInfoFilePath, sourceInfoLineNumber);
        }
        public static void LogError(string text,
            string sourceInfoMemberName = "", string sourceInfoFilePath = "", int sourceInfoLineNumber = -1)
        {
            LogMessage(text, LogMessageType.Error, sourceInfoMemberName, sourceInfoFilePath, sourceInfoLineNumber);
        }
        public static void LogException(Exception exception,
            string sourceInfoMemberName = "", string sourceInfoFilePath = "", int sourceInfoLineNumber = -1)
        {
            // Do you really need the caller info for exceptions? The text already contains the information needed, right?
            LogMessage(exception.ToString(), LogMessageType.Exception, sourceInfoMemberName, sourceInfoFilePath, sourceInfoLineNumber);
        }

        public static void SaveLog(string filePath)
        {
            var logMessageLines = "";
            logMessages.ForEach(l => logMessageLines += l.text + "\n"); // Is that really it? No more information?

            logMessageLines += "\n---\n";
            File.AppendAllLines(filePath, logMessageLines.Split('\n'));
        }

        private static void LogMessage(string text, LogMessageType type, string sourceInfoMemberName, string sourceInfoFilePath, int sourceInfoLineNumber)
        {
            var logMessage = new LogMessage(text, DateTime.Now, (sourceInfoMemberName != "" ? (sourceInfoMemberName + ", " + sourceInfoFilePath + ", " + sourceInfoLineNumber) : ""), type);
            logMessages.Add(logMessage);
        }
    }

    public struct LogMessage
    {
        public readonly string text;
        public readonly DateTime timestamp;
        public readonly string sourceInfo;
        public readonly LogMessageType type;

        public LogMessage(string text, DateTime timestamp, string sourceInfo, LogMessageType type)
        {
            this.text = text;
            this.timestamp = timestamp;
            this.sourceInfo = sourceInfo;
            this.type = type;
        }
    }
    public enum LogMessageType
    {
        Normal,
        Warning,
        Error,
        Exception
    }
}
