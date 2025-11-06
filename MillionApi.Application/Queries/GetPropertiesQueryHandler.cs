using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MillionApi.Application.Interfaces;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Queries;

public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, PagedResult<PropertyResponseDto>>
{
	private readonly IPropertyRepository _repository;

	private readonly IMapper _mapper;

	public GetPropertiesQueryHandler(IPropertyRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PagedResult<PropertyResponseDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<Property> properties;
		long totalCount;
		(properties, totalCount) = await _repository.GetAsync(request.getPropertiesRequest);
		IEnumerable<PropertyResponseDto> enumerable2;
		if (!(properties?.Any() ?? false))
		{
			IEnumerable<PropertyResponseDto> enumerable = Enumerable.Empty<PropertyResponseDto>();
			enumerable2 = enumerable;
		}
		else
		{
			enumerable2 = properties.Select((Property p) => _mapper.Map<PropertyResponseDto>(p));
		}
		IEnumerable<PropertyResponseDto> mappedProperties = enumerable2;
		return new PagedResult<PropertyResponseDto>
		{
			Items = mappedProperties,
			PageNumber = request.getPropertiesRequest.PageNumber,
			PageSize = request.getPropertiesRequest.PageSize,
			TotalCount = totalCount
		};
	}
}
