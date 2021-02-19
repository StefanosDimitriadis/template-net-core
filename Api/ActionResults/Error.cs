using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Template.Shared;

namespace Template.Api.ActionResults
{
	internal class ErrorActionResult : IActionResult
	{
		public async Task ExecuteResultAsync(ActionContext actionContext)
		{
			var errors = actionContext.ModelState.Root.Errors.Select(_modelError => new Error(_modelError.ErrorMessage));
			await actionContext.HttpContext.WriteErrorResponse(errors);
		}
	}
}