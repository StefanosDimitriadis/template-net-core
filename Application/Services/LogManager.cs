namespace Template.Application.Services
{
	public static class LogManager
	{
		public static void Shutdown()
		{
			Serilog.Log.CloseAndFlush();
		}

		public static Serilog.ILogger GetCurrentClassLogger<TCurrentClass>()
		{
			return Serilog.Log.Logger.ForContext<TCurrentClass>();
		}
	}
}