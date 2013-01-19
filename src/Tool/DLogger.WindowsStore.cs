using System.Collections.Generic;

namespace NDatabase.Tool
{
    /// <summary>
    ///   Simple logging class
    /// </summary>
    internal static class DLogger
    {
        private static readonly IList<ILogger> Loggers = new List<ILogger>();

        internal static void Register(ILogger logger)
        {
            Loggers.Add(logger);
        }

        internal static void Warning(object @object)
        {
            foreach (var logger in Loggers)
            {
                logger.Info(string.Concat("Warning", ": "));
                logger.Warning(@object == null
                                      ? "null"
                                      : @object.ToString());
            }
        }

        internal static void Debug(object @object)
        {
            foreach (var logger in Loggers)
            {
                logger.Info(string.Concat("Debug", ": "));
                logger.Debug(@object == null
                                      ? "null"
                                      : @object.ToString());
            }
        }

        internal static void Info(object @object)
        {
            foreach (var logger in Loggers)
            {
                logger.Info(string.Concat("Info", ": "));
                logger.Info(@object == null
                                      ? "null"
                                      : @object.ToString());
            }
        }

        internal static void Error(object @object)
        {
            foreach (var logger in Loggers)
            {
                logger.Info(string.Concat("Error", ": "));
                logger.Error(@object == null
                                      ? "null"
                                      : @object.ToString());
            }
        }
    }
}
