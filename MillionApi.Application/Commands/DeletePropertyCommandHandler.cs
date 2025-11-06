using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MillionApi.Application.Interfaces;

namespace MillionApi.Application.Commands;

public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, Unit>
{
	private readonly IPropertyRepository _repository;

	public DeletePropertyCommandHandler(IPropertyRepository repository)
	{
		_repository = repository;
	}

	public async Task<Unit> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
	{
		await _repository.DeleteAsync(request.Id);
		return Unit.Value;
	}
}
