namespace Jobify.Application.Common.Extensions;

public class BaseSetting
{
    private protected readonly IApplicationDbContext _dbContext;

    public BaseSetting(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
