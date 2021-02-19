using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Commands.Bonuses;
using Template.Application.Queries.Bonuses;
using Template.Shared.Bonuses;

namespace Template.Api.Controllers
{
	public class BonusController : BaseController
	{
		public BonusController(
			IMediator mediator,
			IMapper mapper)
			: base(mediator, mapper) { }

		[HttpGet("{id}")]
		public async Task<GetBonusResponse> Get([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new GetBonusRequest(id);
			var query = _mapper.Map<GetBonusRequest, GetBonusQuery>(request);
			var response = await _mediator.Send(query, cancellationToken);
			return _mapper.Map<GetBonusQueryResponse, GetBonusResponse>(response);
		}

		[HttpPut("{id}")]
		public async Task<AwardBonusResponse> Award([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new AwardBonusRequest(id);
			var command = _mapper.Map<AwardBonusRequest, AwardBonusCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<AwardBonusCommandResponse, AwardBonusResponse>(response);
		}

		[HttpPost]
		public async Task<CreateBonusResponse> Create([FromBody] CreateBonusRequest request, CancellationToken cancellationToken)
		{
			var command = _mapper.Map<CreateBonusRequest, CreateBonusCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<CreateBonusCommandResponse, CreateBonusResponse>(response);
		}

		[HttpDelete("{id}")]
		public async Task<DeleteBonusResponse> Delete([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new DeleteBonusRequest(id);
			var command = _mapper.Map<DeleteBonusRequest, DeleteBonusCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<DeleteBonusCommandResponse, DeleteBonusResponse>(response);
		}
	}
}