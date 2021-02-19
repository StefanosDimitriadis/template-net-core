using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Commands.Campaigns;
using Template.Application.Queries.Campaigns;
using Template.Shared.Campaigns;

namespace Template.Api.Controllers
{
	public class CampaignController : BaseController
	{
		public CampaignController(
			IMediator mediator,
			IMapper mapper)
			: base(mediator, mapper) { }

		[HttpGet("{id}")]
		public async Task<GetCampaignResponse> Get([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new GetCampaignRequest(id);
			var query = _mapper.Map<GetCampaignRequest, GetCampaignQuery>(request);
			var response = await _mediator.Send(query, cancellationToken);
			return _mapper.Map<GetCampaignQueryResponse, GetCampaignResponse>(response);
		}

		[HttpPut("{id}")]
		public async Task<UpdateCampaignResponse> Update([FromRoute] long id, [FromBody] UpdateCampaignRequest request, CancellationToken cancellationToken)
		{
			request.SetId(id);
			var command = _mapper.Map<UpdateCampaignRequest, UpdateCampaignCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<UpdateCampaignCommandResponse, UpdateCampaignResponse>(response);
		}

		[HttpPost]
		public async Task<CreateCampaignResponse> Create([FromBody] CreateCampaignRequest request, CancellationToken cancellationToken)
		{
			var command = _mapper.Map<CreateCampaignRequest, CreateCampaignCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<CreateCampaignCommandResponse, CreateCampaignResponse>(response);
		}

		[HttpDelete("{id}")]
		public async Task<DeleteCampaignResponse> Delete([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new DeleteCampaignRequest(id);
			var command = _mapper.Map<DeleteCampaignRequest, DeleteCampaignCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<DeleteCampaignCommandResponse, DeleteCampaignResponse>(response);
		}
	}
}