using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Authora.Infrastructure.Data;

namespace Authora.Infrastructure.Services
{

    public class UserService : IUserService
    {
        private readonly AuthoraDbContext _context;

        public UserService(AuthoraDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                //.Include(u => u.UserGroups)
                //.ThenInclude(ug => ug.Group)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var tracked = await _context.Users.FindAsync(user.Id);
            if (tracked != null)
            {
                _context.Entry(tracked).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

    }
}



