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

            if (_groups.Any())
            {
                SelectedGroupId = _groups.First().Id;
            }
        }

        private async Task OnGroupChanged()
        {
            if (_selectedGroupId.HasValue)
            {
                _permissions = await PermissionService.GetByGroupIdAsync(_selectedGroupId.Value);
                _newPermission = new Permission
                {
                    GroupId = _selectedGroupId.Value
                };
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
                await OnGroupChanged();
            }
        }

        private async Task DeletePermission(Guid permissionId)
        {
            await PermissionService.DeleteAsync(permissionId);
            await OnGroupChanged();
        }

        // Keep this to handle binding side effect from InputSelect
        private Guid? SelectedGroupId
        {
            get => _selectedGroupId;
            set
            {
                if (_selectedGroupId != value)
                {
                    _selectedGroupId = value;
                    _ = OnGroupChanged();
                }
            }
        }
    }
}
