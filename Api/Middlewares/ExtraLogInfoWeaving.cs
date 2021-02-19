using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Template.Api.Middlewares
{
	internal class ExtraLogInfoWeavingMiddleware
	{
		private readonly RequestDelegate _nextRequestDelegate;

		public ExtraLogInfoWeavingMiddleware(RequestDelegate nextRequestDelegate)
		{
			_nextRequestDelegate = nextRequestDelegate;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				httpContext.TraceIdentifier = Guid.NewGuid().ToString();
				await _nextRequestDelegate(httpContext);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}