using System.Globalization;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdLerBackend.Infrastructure.Services;

public class CsvService : ICsvService
{
    public Task<IEnumerable<CsvUserDto>> ReadUsersFromUserCSV(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<CsvUserMap>();
        var students = csv.GetRecords<CsvUserDto>().ToList();

        return Task.FromResult<IEnumerable<CsvUserDto>>(students);
    }
}

public sealed class CsvUserMap : ClassMap<CsvUserDto>
{
    public CsvUserMap()
    {
        Map(m => m.Username).Name("username");
        Map(m => m.Password).Name("password");
        Map(m => m.Firstname).Name("firstname");
        Map(m => m.Lastname).Name("lastname");

        // Map all columns called course1, course2 ... into the list
        Map(m => m.Courses).Convert(args =>
        {
            var row = args.Row; // IReaderRow
            var headers = row.Context.Reader.HeaderRecord;

            var list = headers
                .Where(h => h.StartsWith("course", StringComparison.OrdinalIgnoreCase))
                .Select(h => row.GetField(h))
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToList();

            return list;
        });
    }
}