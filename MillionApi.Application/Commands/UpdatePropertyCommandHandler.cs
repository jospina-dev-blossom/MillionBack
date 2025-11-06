using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MillionApi.Application.Interfaces;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Commands;

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyResponseDto>
{
	private readonly IPropertyRepository _repository;

	private readonly IMapper _mapper;

	public UpdatePropertyCommandHandler(IPropertyRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PropertyResponseDto> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
	{
		Property property = _mapper.Map<Property>(request.PropertyDto);
		await _repository.UpdateAsync(property.Id, property);
		return _mapper.Map<PropertyResponseDto>(property);
	}
}
