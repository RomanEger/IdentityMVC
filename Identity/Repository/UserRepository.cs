using System.Linq.Expressions;
using Identity.Models.Entities;
using Identity.Services;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repository;

public class UserRepository : IRepository<User>
{
    private ApplicationContext _context;
    
    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<User?> Get(Expression<Func<User, bool>> func) => await _context.Users.FirstOrDefaultAsync(func) ?? null;

    public async Task Add(User entity)
    {
        entity.Password = PasswordHash.EncodePasswordToBase64(entity.Password);
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    } 
}