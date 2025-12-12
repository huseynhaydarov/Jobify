namespace Jobify.Application.UseCases.Companies.Queries.GetCompanyDetail;

public class GetCompanyDetailMapper : Profile
{
    public GetCompanyDetailMapper()
    {
        CreateMap<Company, GetCompanyDetailResponse>();
    }
}
