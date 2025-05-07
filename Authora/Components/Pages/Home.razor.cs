using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Authora.Components.Pages
{
    public partial class Home
    {
        [Inject] IUserService UserService { get; set; } = default!;

        [Inject] IGroupService GroupService { get; set; } = default!;

        [Inject] IPermissionService PermissionService { get; set; } = default!;

        [Inject] IJSRuntime JSRuntime { get; set; } = default!;

        private List<User> _users = new();
        private List<Group> _groups = new();
        private List<Permission> _permissions = new();

        protected override async Task OnInitializedAsync()
        {
            _users = await UserService.GetAllAsync();
            _groups = await GroupService.GetAllAsync();
            _permissions = await PermissionService.GetAllAsync();
        }
    }
}
