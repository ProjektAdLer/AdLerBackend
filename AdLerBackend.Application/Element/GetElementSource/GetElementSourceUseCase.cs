using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
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
        var learningElementModule = await _mediator.Send(new GetLearningElementCommand
        {
            WebServiceToken = request.WebServiceToken,
            WorldId = request.WorldId,
            ElementId = request.ElementId
        }, cancellationToken);
        

        switch (learningElementModule.LmsModule.ModName)
        {
            case "resource":
            case "url":
                return new GetElementSourceResponse
                {
                    // At this point, we assume, that the moodle resource has a file attached to it.
                    FilePath = learningElementModule.LmsModule.Contents![0].fileUrl + "&token=" +
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
            case "adaptivity":
                throw new Exception("The Content of the Adaptivity Element is accessible via ATF File");
            default: throw new NotImplementedException("Unknown module type" + learningElementModule.LmsModule.ModName);
        }
    }
}