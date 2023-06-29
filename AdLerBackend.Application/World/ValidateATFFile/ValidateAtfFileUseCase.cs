using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace AdLerBackend.Application.World.ValidateATFFile;

public class ValidateAtfFileUseCase : IRequestHandler<ValidateATFFileCommand>
{
    static ValidateAtfFileUseCase()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "AdLerBackend.Application.Common.Schemas.schema0_4.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();

        Schema = JSchema.Parse(result);
    }

    private static JSchema Schema { get; }

    public async Task Handle(ValidateATFFileCommand request, CancellationToken cancellationToken)
    {
        var streamReader = new StreamReader(request.ATFFileStream);
        var jsonReader = new JsonTextReader(streamReader);
        var json = await JToken.LoadAsync(jsonReader, cancellationToken);

        var isValid = json.IsValid(Schema, out IList<string> errorMessages);
        if (isValid) return;
        var validationFailures
            = errorMessages.Select(errorMessage => new ValidationFailure(string.Empty, errorMessage)).ToList();

        throw new ValidationException(validationFailures);
    }
}