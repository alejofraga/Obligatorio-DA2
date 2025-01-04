using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Sessions;

public interface ISessionRepository : IRepository<Session>
{
    new Session GetOrDefault(Expression<Func<Session, bool>> predicate);
}
