using MediatR;

namespace MillionApi.Application.Commands;

public record CreatePropertyCommand(CreatePropertyRequestDto PropertyDto) : IRequest<PropertyResponseDto>, IBaseRequest;
