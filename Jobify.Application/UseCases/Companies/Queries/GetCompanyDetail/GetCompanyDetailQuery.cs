namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailQuery : IRequest<GetCompanyDetailViewModel>
{
    public Guid Id { get; set; }

    public GetCompanyDetailQuery(Guid id)
    {
        Id = id;
    }
}
