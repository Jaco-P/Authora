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
            try
            {
                await GroupService.AddAsync(_newGroup);
                _newGroup = new();
                _groups = await GroupService.GetAllAsync();
                _successMessage = "Group added successfully.";
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                _successMessage = "Failed to add Group.";
                Console.Write($"Could not add group. The error was: {ex.Message}");
            }


        }

        private async Task DeleteGroup(Guid groupId)
        {
            bool confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this group?");
            if (!confirmed)
                return;

            try
            {
                await GroupService.DeleteAsync(groupId);
                _groups = await GroupService.GetAllAsync();
                _successMessage = "Group deleted successfully.";
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                Console.WriteLine($"Group could not be deleted. The error was : {ex.Message}");
                _successMessage = "Failed to delete Group.";
            }
        }
    }
}
