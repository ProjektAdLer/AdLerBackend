using MediatR;

namespace AdLerBackend.Application.AdminPanel.CreateUsersByCsv;

public class CreateUsersByCsvUseCase : IRequestHandler<CreateUsersByCsvCommand, bool>
{
    public Task<bool> Handle(CreateUsersByCsvCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}