using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.AdminPanel.CreateUsersByCsv;

public class CreateUsersByCsvUseCase(ICsvService csvService, ILMS lmsService)
    : IRequestHandler<CreateUsersByCsvCommand, bool>
{
    public async Task<bool> Handle(CreateUsersByCsvCommand request, CancellationToken cancellationToken)
    {
        var userList = await csvService.ReadUsersFromUserCSV(request.UserFileStream);
        throw new NotImplementedException();
    }
}