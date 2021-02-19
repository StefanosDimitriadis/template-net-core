using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Template.Application.Commands.Customers;
using Template.Application.Queries.Customers;
using Template.Shared.Customers;

namespace Template.Api.Controllers
{
	public class CustomerController : BaseController
	{
		public CustomerController(
			IMediator mediator,
			IMapper mapper)
			: base(mediator, mapper) { }

		[HttpGet("{id}")]
		public async Task<GetCustomerResponse> Get([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new GetCustomerRequest(id);
			var query = _mapper.Map<GetCustomerRequest, GetCustomerQuery>(request);
			var response = await _mediator.Send(query, cancellationToken);
			return _mapper.Map<GetCustomerQueryResponse, GetCustomerResponse>(response);
		}

		[HttpPut("{id}")]
		public async Task<UpdateCustomerResponse> Update([FromRoute] long id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
		{
			request.SetId(id);
			var command = _mapper.Map<UpdateCustomerRequest, UpdateCustomerCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<UpdateCustomerCommandResponse, UpdateCustomerResponse>(response);
		}

		[HttpPut("addmoney/{id}")]
		public async Task<AddMoneyResponse> AddMoney([FromRoute] long id, [FromBody] AddMoneyRequest request, CancellationToken cancellationToken)
		{
			request.SetCustomerId(id);
			var command = _mapper.Map<AddMoneyRequest, AddMoneyCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<AddMoneyCommandResponse, AddMoneyResponse>(response);
		}

		[HttpPost]
		public async Task<CreateCustomerResponse> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
		{
			var command = _mapper.Map<CreateCustomerRequest, CreateCustomerCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<CreateCustomerCommandResponse, CreateCustomerResponse>(response);
		}

		[HttpDelete("{id}")]
		public async Task<DeleteCustomerResponse> Delete([FromRoute] long id, CancellationToken cancellationToken)
		{
			var request = new DeleteCustomerRequest(id);
			var command = _mapper.Map<DeleteCustomerRequest, DeleteCustomerCommand>(request);
			var response = await _mediator.Send(command, cancellationToken);
			return _mapper.Map<DeleteCustomerCommandResponse, DeleteCustomerResponse>(response);
		}
	}
}