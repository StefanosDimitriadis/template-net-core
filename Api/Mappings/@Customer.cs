using AutoMapper;
using System.Collections.Generic;
using Template.Application.Commands.Customers;
using Template.Application.Queries.Customers;
using Template.Shared.Customers;

namespace Template.Api.Mappings
{
	internal class CustomerMapping : Profile
	{
		public CustomerMapping()
		{
			CreateMap<GetCustomerRequest, GetCustomerQuery>().ConstructUsing((_request, _resolutionContext) =>
			{
				return GetCustomerQuery.Create(_request.Id);
			});
			CreateMap<GetCustomerQueryResponse, GetCustomerResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return GetCustomerResponse.CreateError(errors);
				}
				else
				{
					var customer = _resolutionContext.Mapper.Map<Domain.Entities.Customers.Customer, Shared.Customers.Customer>(_response.Customer);
					return GetCustomerResponse.Create(customer);
				}
			});
			CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return UpdateCustomerCommand.Create(_request.Id, _request.Email, _request.DateOfBirth);
			});
			CreateMap<UpdateCustomerCommandResponse, UpdateCustomerResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return UpdateCustomerResponse.CreateError(errors);
				}
				else
					return UpdateCustomerResponse.Create();
			});
			CreateMap<AddMoneyRequest, AddMoneyCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return AddMoneyCommand.Create(_request.CustomerId, _request.Money);
			});
			CreateMap<AddMoneyCommandResponse, AddMoneyResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return AddMoneyResponse.CreateError(errors);
				}
				else
					return AddMoneyResponse.Create();
			});
			CreateMap<CreateCustomerRequest, CreateCustomerCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return CreateCustomerCommand.Create(_request.Name, _request.Email, _request.DateOfBirth);
			});
			CreateMap<CreateCustomerCommandResponse, CreateCustomerResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return CreateCustomerResponse.CreateError(errors);
				}
				else
					return CreateCustomerResponse.Create();
			});
			CreateMap<DeleteCustomerRequest, DeleteCustomerCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return DeleteCustomerCommand.Create(_request.Id);
			});
			CreateMap<DeleteCustomerCommandResponse, DeleteCustomerResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return DeleteCustomerResponse.CreateError(errors);
				}
				else
					return DeleteCustomerResponse.Create();
			});
			CreateMap<Domain.Entities.Customers.Customer, Shared.Customers.Customer>().ConstructUsing((_customer, _resolutionContext) =>
			{
				return new Customer(_customer.Id, _customer.Name, _customer.Email, _customer.DateOfBirth, _customer.Money, _customer.CreatedAt, _customer.UpdatedAt, _customer.IsDeleted);
			});
		}
	}
}