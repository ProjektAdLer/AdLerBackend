using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.LearningElement.GetLearningElementSource.GetH5PFilePath;
using MediatR;

namespace AdLerBackend.Application.LearningElement.GetLearningElementSource;

public class
    GetLearningElementSourceHandler : IRequestHandler<GetLearningElementSourceCommand, GetLearningElementSourceResponse>
{
    private readonly IMediator _mediator;

    public GetLearningElementSourceHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<GetLearningElementSourceResponse> Handle(GetLearningElementSourceCommand request,
        CancellationToken cancellationToken)
    {
        var module = await _mediator.Send(new GetLearningElementLmsInformationCommand
        {
            CourseId = request.CourseId,
            ElementId = request.ElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        switch (module.LearningElementData.ModName)
        {
            case "resource":
            case "url":
                return new GetLearningElementSourceResponse
                {
                    // At this point, we assume, that the moodle resource has a file attached to it.
                    FilePath = module.LearningElementData.Contents![0].fileUrl + "&token=" +
                               request.WebServiceToken
                };
            case "h5pactivity":
                var data = await _mediator.Send(new GetH5PFilePathCommand
                {
                    CourseId = request.CourseId,
                    ElementId = request.ElementId,
                    WebServiceToken = request.WebServiceToken
                }, cancellationToken);

                return new GetLearningElementSourceResponse
                {
                    FilePath = data.FilePath
                };
                break;
            default: throw new NotImplementedException("Unknown module type" + module.LearningElementData.ModName);
        }
    }
}