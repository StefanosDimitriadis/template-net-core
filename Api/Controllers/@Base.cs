﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseController : ControllerBase
	{
		protected readonly IMediator _mediator;
		protected readonly IMapper _mapper;

		protected BaseController(
			IMediator mediator,
			IMapper mapper)
		{
			_mediator = mediator;
			_mapper = mapper;
		}
	}
}