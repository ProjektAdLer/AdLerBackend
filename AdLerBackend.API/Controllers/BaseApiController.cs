using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace AdLerBackend.API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected readonly IMediator Mediator;

    public BaseApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
}