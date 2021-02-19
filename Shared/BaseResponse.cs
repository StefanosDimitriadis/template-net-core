using NSwag.Annotations;
using System.Collections.Generic;

namespace Template.Shared
{
	[OpenApiIgnore]
	public abstract class BaseResponse
	{
		public abstract IReadOnlyCollection<Error> Errors { get; protected set; }

		protected BaseResponse() { }

		protected BaseResponse(IReadOnlyCollection<Error> errors)
		{
			Errors = errors;
		}
	}
}