using Authora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authora.Application.Interfaces
{
    public interface IGroupService
    {
        Task<List<Group>> GetAllAsync();
        Task AddAsync(Group group);
        Task DeleteAsync(Guid groupId);
    }
}
