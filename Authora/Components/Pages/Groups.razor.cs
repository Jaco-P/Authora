using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Authora.Components.Pages
{
    public partial class Groups
    {
        [Inject] IGroupService GroupService { get; set; }

        private List<Group> _groups = new();
        private Group _newGroup = new();
        private string? _successMessage;

        protected override async Task OnInitializedAsync()
        {
            _groups = await GroupService.GetAllAsync();
        }

        private async Task AddGroup()
        {
            await GroupService.AddAsync(_newGroup);
            _newGroup = new();
            _groups = await GroupService.GetAllAsync();
            _successMessage = "Group added successfully.";
        }

        private async Task DeleteGroup(Guid groupId)
        {
            bool confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this group?");
            if (!confirmed)
                return;

            await GroupService.DeleteAsync(groupId);
            _groups = await GroupService.GetAllAsync();
            _successMessage = "Group deleted successfully.";
        }
    }
}
