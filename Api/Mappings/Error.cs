﻿using AutoMapper;

namespace Template.Api.Mappings
{
	internal class ErrorMapping : Profile
	{
		public ErrorMapping()
		{
			CreateMap<Application.Error, Shared.Error>().ConstructUsing((_error, _resolutionContext) =>
			{
				return new Shared.Error(_error.Message);
			});
		}
	}
}