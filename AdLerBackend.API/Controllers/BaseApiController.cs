using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace AdLerBackend.API.Controllers;

[ApiController]
public class BaseApiController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
}