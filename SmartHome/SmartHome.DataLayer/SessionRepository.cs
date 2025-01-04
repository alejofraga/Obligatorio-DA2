using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Sessions;

namespace SmartHome.DataLayer;

public class SessionRepository(DbContext context) : Repository<Session>(context), ISessionRepository
{
    private readonly DbSet<Session> _sessions = context.Set<Session>();

    public override Session? GetOrDefault(Expression<Func<Session, bool>> predicate)
    {
        return _sessions
            .Include(s => s.User)
            .ThenInclude(u => u.Roles)
            .ThenInclude(r => r.SystemPermissions)
            .FirstOrDefault(predicate);
    }
}
