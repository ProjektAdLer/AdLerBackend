using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace AdLerBackend.Application.World.ValidateATFFile;

public class ValidateATFFileHandler : IRequestHandler<ValidateATFFileCommand, Unit>
{
    public async Task<Unit> Handle(ValidateATFFileCommand request, CancellationToken cancellationToken)
    {
        var schemaJson = File.ReadAllText("./config/schema0_3.json");
        var schema = JSchema.Parse(schemaJson);

        var streamReader = new StreamReader(request.ATFFileStream);
        var jsonReader = new JsonTextReader(streamReader);
        var json = await JToken.LoadAsync(jsonReader);

        IList<string> errorMessages;
        var isValid = json.IsValid(schema, out errorMessages);
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