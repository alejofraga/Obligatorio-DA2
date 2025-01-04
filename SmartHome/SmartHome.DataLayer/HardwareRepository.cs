using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.DataLayer;

public class HardwareRepository(DbContext context) : Repository<Hardware>(context), IHardwareRepository
{
    private readonly DbSet<Hardware> _hardwares = context.Set<Hardware>();

    public override Hardware? GetOrDefault(Expression<Func<Hardware, bool>> predicate)
    {
        return _hardwares
            .Include(h => h.Device)
            .FirstOrDefault(predicate);
    }
}
