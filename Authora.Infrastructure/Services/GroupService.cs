using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Authora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authora.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly AuthoraDbContext _context;

        public GroupService(AuthoraDbContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group?> GetByIdAsync(Guid id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task AddAsync(Group group)
        {
            group.Id = Guid.NewGuid();
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group is not null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }
    }
}

