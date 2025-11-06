using MediatR;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Queries;

public record GetPropertiesQuery(GetPropertiesRequest getPropertiesRequest) : IRequest<PagedResult<PropertyResponseDto>>, IBaseRequest;
