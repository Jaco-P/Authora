using Authora.Domain.Entities;

namespace Authora.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<List<Permission>> GetAllAsync();
        Task<List<Permission>> GetByGroupIdAsync(Guid groupId);
        Task AddAsync(Permission permission);
        Task DeleteAsync(Guid id);
    }
}