using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MillionApi.Application.Interfaces;
using MillionApi.Domain.AggregateModel;
using MillionApi.Domain.Exceptions;
using MongoDB.Bson;

namespace MillionApi.Application.Queries;

public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDetailResponseDto>
{
	private readonly IPropertyRepository _repository;

	private readonly IMapper _mapper;

	public GetPropertyByIdQueryHandler(IPropertyRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PropertyDetailResponseDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(request.Id, out var _))
		{
			throw new BadRequestException("Property ID must be a valid ObjectId format.");
		}
		Property property;
		Owner owner;
		List<PropertyImage> images;
		List<PropertyTrace> traces;
		(property, owner, images, traces) = await _repository.GetByIdWithDetailsAsync(request.Id);
		if (property == null)
		{
			throw new NotFoundException("Property", request.Id);
		}
		return new PropertyDetailResponseDto
		{
			Id = property.Id,
			Name = property.Name,
			AddressProperty = property.AddressProperty,
			PriceProperty = property.PriceProperty,
			ImageUrl = property.ImageUrl,
			CodeInternal = property.CodeInternal,
			Year = property.Year,
			Owner = ((owner != null) ? _mapper.Map<OwnerDto>(owner) : null),
			Images = ((images != null) ? _mapper.Map<List<PropertyImageDto>>(images) : new List<PropertyImageDto>()),
			Traces = ((traces != null) ? _mapper.Map<List<PropertyTraceDto>>(traces) : new List<PropertyTraceDto>())
		};
	}
}
