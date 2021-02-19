namespace Template.Application.Services
{
	public static class LogManager
	{
		public static void Shutdown()
		{
			NLog.LogManager.Shutdown();
		}

		public static NLog.Logger GetCurrentClassLogger()
		{
			return NLog.LogManager.GetCurrentClassLogger();
		}
	}
}