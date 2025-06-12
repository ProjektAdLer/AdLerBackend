namespace AdLerBackend.Application.Common.DTOs;

public record CsvUserDto
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Firstname { get; set; }

    public required string Lastname { get; set; }
    public IList<string> Courses { get; set; } = new List<string>();
}