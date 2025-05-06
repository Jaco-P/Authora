using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Authora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Authora.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AuthoraDbContext _context;

        public PermissionService(AuthoraDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _context.Permissions.Include(p => p.Group).ToListAsync();
        }

        public async Task<List<Permission>> GetByGroupIdAsync(Guid groupId)
        {
            return await _context.Permissions
                .Where(p => p.GroupId == groupId)
                .ToListAsync();
        }

        public async Task AddAsync(Permission permission)
        {
            permission.Id = Guid.NewGuid();
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission is not null)
            {
                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
        }
    }
}
