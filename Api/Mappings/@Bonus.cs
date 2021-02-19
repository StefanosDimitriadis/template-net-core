using AutoMapper;
using System.Collections.Generic;
using Template.Application.Commands.Bonuses;
using Template.Application.Queries.Bonuses;
using Template.Shared.Bonuses;

namespace Template.Api.Mappings
{
	public class BonusMapping : Profile
	{
		public BonusMapping()
		{
			CreateMap<GetBonusRequest, GetBonusQuery>().ConstructUsing((_request, _resolutionContext) =>
			{
				return GetBonusQuery.Create(_request.Id);
			});
			CreateMap<GetBonusQueryResponse, GetBonusResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return GetBonusResponse.CreateError(errors);
				}
				else
				{
					var bonus = _resolutionContext.Mapper.Map<Domain.Entities.Bonuses.Bonus, Shared.Bonuses.Bonus>(_response.Bonus);
					return GetBonusResponse.Create(bonus);
				}
			});
			CreateMap<AwardBonusRequest, AwardBonusCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return AwardBonusCommand.Create(_request.Id);
			});
			CreateMap<AwardBonusCommandResponse, AwardBonusResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return AwardBonusResponse.CreateError(errors);
				}
				else
					return AwardBonusResponse.Create();
			});
			CreateMap<CreateBonusRequest, CreateBonusCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return CreateBonusCommand.Create(_request.CampaignId, _request.CustomerId);
			});
			CreateMap<CreateBonusCommandResponse, CreateBonusResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return CreateBonusResponse.CreateError(errors);
				}
				else
					return CreateBonusResponse.Create();
			});
			CreateMap<DeleteBonusRequest, DeleteBonusCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return DeleteBonusCommand.Create(_request.Id);
			});
			CreateMap<DeleteBonusCommandResponse, DeleteBonusResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return DeleteBonusResponse.CreateError(errors);
				}
				else
					return DeleteBonusResponse.Create();
			});
			CreateMap<Domain.Entities.Bonuses.Bonus, Shared.Bonuses.Bonus>().ConstructUsing((_bonus, _resolutionContext) =>
			{
				return new Bonus(_bonus.Id, _bonus.CampaignId, _bonus.CustomerId, _bonus.HasBeenAwarded, _bonus.CreatedAt, _bonus.UpdatedAt, _bonus.IsDeleted);
			});
		}
	}
}