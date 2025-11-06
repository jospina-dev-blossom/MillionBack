using MediatR;

namespace MillionApi.Application.Commands;

public record DeletePropertyCommand(string Id) : IRequest<Unit>, IBaseRequest;
