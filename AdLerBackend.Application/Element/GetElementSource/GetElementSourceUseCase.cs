using AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Element.GetElementSource.GetH5PFilePath;
using MediatR;

namespace AdLerBackend.Application.Element.GetElementSource;

public class
    GetElementSourceUseCase : IRequestHandler<GetElementSourceCommand, GetElementSourceResponse>
{
    private readonly IMediator _mediator;

    public GetElementSourceUseCase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<GetElementSourceResponse> Handle(GetElementSourceCommand request,
        CancellationToken cancellationToken)
    {
        var module = await _mediator.Send(new GetElementLmsInformationCommand
        {
            WorldId = request.WorldId,
            ElementId = request.ElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        switch (module.ElementData.ModName)
        {
            case "resource":
            case "url":
                return new GetElementSourceResponse
                {
                    // At this point, we assume, that the moodle resource has a file attached to it.
                    FilePath = module.ElementData.Contents![0].fileUrl + "&token=" +
                               request.WebServiceToken
                };
            case "h5pactivity":
                var data = await _mediator.Send(new GetH5PFilePathCommand
                {
                    WorldId = request.WorldId,
                    ElementId = request.ElementId,
                    WebServiceToken = request.WebServiceToken
                }, cancellationToken);

                return new GetElementSourceResponse
                {
                    FilePath = data.FilePath
                };
            default: throw new NotImplementedException("Unknown module type" + module.ElementData.ModName);
        }
    }
}