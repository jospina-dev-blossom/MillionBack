using MediatR;

namespace MillionApi.Application.Commands;

public record UpdatePropertyCommand(UpdatePropertyRequestDto PropertyDto) : IRequest<PropertyResponseDto>, IBaseRequest;
