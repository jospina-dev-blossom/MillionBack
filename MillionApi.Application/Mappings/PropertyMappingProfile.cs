using AutoMapper;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Mappings;

public class PropertyMappingProfile : Profile
{
	public PropertyMappingProfile()
	{
		CreateMap<CreatePropertyRequestDto, Property>();
		CreateMap<Property, PropertyResponseDto>();
		CreateMap<Owner, OwnerDto>();
		CreateMap<PropertyImage, PropertyImageDto>();
		CreateMap<PropertyTrace, PropertyTraceDto>();
	}
}
