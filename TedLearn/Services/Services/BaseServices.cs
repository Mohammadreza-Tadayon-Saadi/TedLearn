using Data.Context;

namespace Services.Services;

public class BaseServices
{
    #region ConstructorInjection

    protected readonly TedLearnContext _context;
    public BaseServices(TedLearnContext context)
    {
        _context = context;
    }

    #endregion

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public int SaveChanges() => _context.SaveChanges();
}
