using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Template.Application.Services;

namespace Template.Api.Filters
{
	internal class SettingsValidationStartupFilter : IStartupFilter
	{
		private readonly IEnumerable<IValidatable> _validatableObjects;

		public SettingsValidationStartupFilter(IEnumerable<IValidatable> validatableObjects)
		{
			_validatableObjects = validatableObjects;
		}

		public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
		{
			foreach (var validatableObject in _validatableObjects)
				validatableObject.Validate();
			return next;
		}
	}
}