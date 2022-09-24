using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.LearningElement.H5P.GetH5PFilePath;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.LearningElements.H5P;

[Route("api/LearningElements/H5P")]
public class H5PController : BaseApiController
{
    private readonly IMediator _mediator;

    public H5PController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("FilePath/Course/{courseId}/Element/{id}")]
    public async Task<ActionResult<GetH5PFilePathResponse>> GetH5PFilePath([FromHeader] string token,
        [FromRoute] int id, [FromRoute] int courseId)
    {
        return await Mediator.Send(new GetH5PFilePathCommand
        {
            ElementId = id,
            WebServiceToken = token,
            CourseId = courseId
        });
    }
}