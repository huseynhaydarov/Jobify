using System;

namespace Jobify.Application.UseCases.JobListings.Events;

public record JobListingChangedEvent
{
    public Guid Id { get; set; }
    public ActionType Action { get; set; }
}

public enum ActionType
{
    Added,
    Deleted,
    Updated
}
