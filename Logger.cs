using Microsoft.Extensions.Logging;
using System;

namespace SqlDBSecurityProvider
{
	public static class Logger
	{
		private static ILogger _logger = null;

		public static void SetLogger(ILogger logger)
		{
			_logger = logger;
		}

		public static void Debug(string msg, params object[] args)
		{
			_logger?.LogDebug(msg, args);
		}
		public static void Information(string msg, params object[] args)
		{
			_logger?.LogInformation(msg, args);
		}
		public static void Error(string msg, params object[] args)
		{
			_logger?.LogError(msg, args);
		}
		public static void Exception(Exception e, string msg, params object[] args)
		{
			_logger?.LogError(e, msg, args);
		}
	}
}
