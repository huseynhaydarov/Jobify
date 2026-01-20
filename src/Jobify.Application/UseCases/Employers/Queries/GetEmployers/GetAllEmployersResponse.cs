using System;

namespace Jobify.Application.UseCases.Employers.Queries.GetEmployers;

public class GetAllEmployersResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
}
