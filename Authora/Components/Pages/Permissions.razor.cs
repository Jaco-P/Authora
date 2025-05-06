using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Authora.Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Authora.Components.Pages
{
    public partial class Permissions
    {
        [Inject] IGroupService GroupService { get; set; } 
        [Inject] IPermissionService PermissionService { get; set; }

        private List<Group>? _groups;
        private List<Permission>? _permissions;
        private Guid? _selectedGroupId;
        private Permission _newPermission = new();

        protected override async Task OnInitializedAsync()
        {
            _groups = await GroupService.GetAllAsync();
        }

        private async Task OnGroupChanged(ChangeEventArgs e)
        {
            if (Guid.TryParse(e.Value?.ToString(), out var groupId))
            {
                _selectedGroupId = groupId;
                _permissions = await PermissionService.GetByGroupIdAsync(groupId);
                _newPermission = new Permission { GroupId = groupId };
            }
            else
            {
                _permissions = null;
            }
        }

        private async Task AddPermission()
        {
            if (_selectedGroupId.HasValue)
            {
                _newPermission.GroupId = _selectedGroupId.Value;
                await PermissionService.AddAsync(_newPermission);
                _permissions = await PermissionService.GetByGroupIdAsync(_selectedGroupId.Value);
                _newPermission = new Permission { GroupId = _selectedGroupId.Value };
            }
        }

        private async Task DeletePermission(Guid id)
        {
            await PermissionService.DeleteAsync(id);
            if (_selectedGroupId.HasValue)
            {
                _permissions = await PermissionService.GetByGroupIdAsync(_selectedGroupId.Value);
            }
        }

    }
}
