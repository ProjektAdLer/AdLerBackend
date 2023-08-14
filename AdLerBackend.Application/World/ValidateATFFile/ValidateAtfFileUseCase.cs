using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace AdLerBackend.Application.World.ValidateATFFile;

public class ValidateAtfFileUseCase : IRequestHandler<ValidateATFFileCommand, Unit>
{
    static ValidateAtfFileUseCase()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "AdLerBackend.Application.Common.Schemas.schema0_5.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();

        Schema = JSchema.Parse(result);
    }

    private static JSchema Schema { get; }

    public async Task<Unit> Handle(ValidateATFFileCommand request, CancellationToken cancellationToken)
    {
        var streamReader = new StreamReader(request.ATFFileStream);
        var jsonReader = new JsonTextReader(streamReader);
        var json = await JToken.LoadAsync(jsonReader);

        IList<string> errorMessages;
        var isValid = json.IsValid(Schema, out errorMessages);
        if (!isValid)
        {
            var validationFailures = new List<ValidationFailure>();
            foreach (var errorMessage in errorMessages)
            {
                var validationFailure = new ValidationFailure(string.Empty, errorMessage);
                validationFailures.Add(validationFailure);
            }

            throw new ValidationException(validationFailures);
        }

        return Unit.Value;
    }
}