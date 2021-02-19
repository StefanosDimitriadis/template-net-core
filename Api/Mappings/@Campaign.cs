using AutoMapper;
using System.Collections.Generic;
using Template.Application.Commands.Campaigns;
using Template.Application.Queries.Campaigns;
using Template.Shared.Campaigns;

namespace Template.Api.Mappings
{
	public class CampaignMapping : Profile
	{
		public CampaignMapping()
		{
			CreateMap<GetCampaignRequest, GetCampaignQuery>().ConstructUsing((_request, _resolutionContext) =>
			{
				return GetCampaignQuery.Create(_request.Id);
			});
			CreateMap<GetCampaignQueryResponse, GetCampaignResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return GetCampaignResponse.CreateError(errors);
				}
				else
				{
					var campaign = _resolutionContext.Mapper.Map<Domain.Entities.Campaigns.Campaign, Shared.Campaigns.Campaign>(_response.Campaign);
					return GetCampaignResponse.Create(campaign);
				}
			});
			CreateMap<UpdateCampaignRequest, UpdateCampaignCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				var campaignSpecification = _resolutionContext.Mapper.Map<Shared.Campaigns.CampaignSpecification, Domain.Values.CampaignSpecification>(_request.CampaignSpecification);
				return UpdateCampaignCommand.Create(_request.Id, campaignSpecification);
			});
			CreateMap<UpdateCampaignCommandResponse, UpdateCampaignResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return UpdateCampaignResponse.CreateError(errors);
				}
				else
					return UpdateCampaignResponse.Create();
			});
			CreateMap<CreateCampaignRequest, CreateCampaignCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				var campaignSpecification = _resolutionContext.Mapper.Map<Shared.Campaigns.CampaignSpecification, Domain.Values.CampaignSpecification>(_request.CampaignSpecification);
				return CreateCampaignCommand.Create(campaignSpecification);
			});
			CreateMap<CreateCampaignCommandResponse, CreateCampaignResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return CreateCampaignResponse.CreateError(errors);
				}
				else
					return CreateCampaignResponse.Create();
			});
			CreateMap<DeleteCampaignRequest, DeleteCampaignCommand>().ConstructUsing((_request, _resolutionContext) =>
			{
				return DeleteCampaignCommand.Create(_request.Id);
			});
			CreateMap<DeleteCampaignCommandResponse, DeleteCampaignResponse>().ConstructUsing((_response, _resolutionContext) =>
			{
				if (_response.Errors != null)
				{
					var errors = _resolutionContext.Mapper.Map<IReadOnlyCollection<Application.Error>, IReadOnlyCollection<Shared.Error>>(_response.Errors);
					return DeleteCampaignResponse.CreateError(errors);
				}
				else
					return DeleteCampaignResponse.Create();
			});
			CreateMap<Domain.Entities.Campaigns.Campaign, Shared.Campaigns.Campaign>().ConstructUsing((_campaign, _resolutionContext) =>
			{
				var campaignSpecification = _resolutionContext.Mapper.Map<Domain.Values.CampaignSpecification, Shared.Campaigns.CampaignSpecification>(_campaign.CampaignSpecification);
				return new Campaign(_campaign.Id, campaignSpecification, _campaign.CreatedAt, _campaign.UpdatedAt, _campaign.IsDeleted);
			});
			CreateMap<Domain.Values.CampaignSpecification, Shared.Campaigns.CampaignSpecification>().ConstructUsing((_campaignSpecification, _resolutionContext) =>
			{
				return new CampaignSpecification(_campaignSpecification.BonusMoney);
			});
			CreateMap<Shared.Campaigns.CampaignSpecification, Domain.Values.CampaignSpecification>().ConstructUsing((_campaignSpecification, _resolutionContext) =>
			{
				return new Domain.Values.CampaignSpecification(_campaignSpecification.BonusMoney);
			});
		}
	}
}