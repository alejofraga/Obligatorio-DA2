using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Homes;

public interface IMemberRepository : IRepository<Member>
{
    new Member GetOrDefault(Expression<Func<Member, bool>> predicate);
}
