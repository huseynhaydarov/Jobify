namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public record GetCompanyDetailQuery(Guid Id) : IRequest<GetCompanyDetailResponse>;
