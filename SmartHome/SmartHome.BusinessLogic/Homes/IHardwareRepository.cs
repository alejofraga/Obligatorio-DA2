using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Homes;

public interface IHardwareRepository : IRepository<Hardware>
{
    new Hardware GetOrDefault(Expression<Func<Hardware, bool>> predicate);
}
