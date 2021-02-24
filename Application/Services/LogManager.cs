namespace Template.Application.Services
{
	public static class LogManager
	{
		public static void Shutdown()
		{
			NLog.LogManager.Shutdown();
		}

		public static NLog.Logger GetCurrentClassLogger(string projectName, string className)
		{
			return NLog.LogManager.GetLogger($"{nameof(projectName)}.{nameof(className)}");
		}
	}
}