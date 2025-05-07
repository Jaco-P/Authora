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
                try
                {
                    _permissions = await PermissionService.GetByGroupIdAsync(_selectedGroupId.Value);
                    _newPermission = new Permission
                    {
                        GroupId = _selectedGroupId.Value
                    };
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    Console.WriteLine($"Could not retrieve Group. The error was: {ex.Message}");
                }
                
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
                try
                {
                    _newPermission.GroupId = _selectedGroupId.Value;
                    await PermissionService.AddAsync(_newPermission);
                    await OnGroupChanged();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    Console.WriteLine($"Could not Add Permission. The error was: {ex.Message}");
                }
            }
        }

        private async Task DeletePermission(Guid permissionId)
        {
            try
            {
                await PermissionService.DeleteAsync(permissionId);
                await OnGroupChanged();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Console.WriteLine($"Could not delete permission. The error was {ex.Message}");
            }
            
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
