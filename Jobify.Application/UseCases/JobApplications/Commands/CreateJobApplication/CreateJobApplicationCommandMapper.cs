namespace Jobify.Application.UseCases.JobApplications.Commands.CreateJobApplication;

public class CreateJobApplicationCommandMapper : Profile
{
    public CreateJobApplicationCommandMapper()
    {
        CreateMap<CreateJobApplicationCommand, JobApplication>();
    }
}
