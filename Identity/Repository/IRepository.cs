using System.Linq.Expressions;
using Identity.Models.Entities;

namespace Identity.Repository;

public interface IRepository<T>
{
    Task<T?> Get(Expression<Func<User, bool>> func);
    Task Add(T entity);
}