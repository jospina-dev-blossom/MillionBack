using MediatR;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Queries;

public record GetPropertyByIdQuery(string Id) : IRequest<PropertyDetailResponseDto>, IBaseRequest;
