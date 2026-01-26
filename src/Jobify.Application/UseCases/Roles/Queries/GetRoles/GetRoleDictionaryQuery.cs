using System.Collections.Generic;
using MediatR;

namespace Jobify.Application.UseCases.Roles.Queries.GetRoles;

public record GetRoleDictionaryQuery : IRequest<List<GetRoleDictionaryResponse>>;
