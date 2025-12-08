namespace Jobify.Application.UseCases.UserProfiles.Queries.GetUserProfileDetail;

public record GetUserProfileDetailQuery(Guid Id) : IRequest<GetUserProfileDetailVievModel>;
