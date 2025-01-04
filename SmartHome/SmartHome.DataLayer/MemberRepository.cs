using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.DataLayer;

public class MemberRepository(DbContext context) : Repository<Member>(context), IMemberRepository
{
    private readonly DbSet<Member> _members = context.Set<Member>();

    public override Member? GetOrDefault(Expression<Func<Member, bool>> predicate)
    {
        return _members
            .Include(m => m.Home)
            .Include(m => m.User)
            .Include(m => m.HomePermissions)
            .Include(m => m.NotiActions)
            .FirstOrDefault(predicate);
    }

    public override List<Member> GetAll(Expression<Func<Member, bool>> predicate)
    {
        return _members
            .Include(m => m.Home)
            .Include(m => m.User)
            .Include(m => m.HomePermissions)
            .Include(m => m.NotiActions)
            .Where(predicate)
            .ToList();
    }
}
