﻿using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;

public record GetLearningElementCommand : CommandWithToken<AdLerLmsElementAggregation>
{
    public bool CanBeLocked = false;
    public int WorldId { get; init; }
    public int ElementId { get; init; }
}