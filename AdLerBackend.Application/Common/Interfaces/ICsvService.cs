using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ICsvService
{
    public Task<IEnumerable<CsvUserDto>> ReadUsersFromUserCSV(Stream stream);
}