namespace Jobify.Application.Common.Extensions;

public class BaseSetting
{
    private protected readonly IMapper _mapper;
    private protected readonly IApplicationDbContext _dbContext;

    public BaseSetting(IMapper mapper,
        IApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
}
