namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfiles;

public record GetAllUserProfilesQuery(PagingParameters PagingParameters)
    : IRequest<PaginatedList<GetAllUserProfilesViewModel>>;
