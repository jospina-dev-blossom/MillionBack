using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MillionApi.Application.Interfaces;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Commands;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyResponseDto>
{
	private readonly IPropertyRepository _repository;

	private readonly IMapper _mapper;

	public CreatePropertyCommandHandler(IPropertyRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PropertyResponseDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
	{
		Property property = _mapper.Map<Property>(request.PropertyDto);
		await _repository.CreateAsync(property);
		return _mapper.Map<PropertyResponseDto>(property);
	}
}
