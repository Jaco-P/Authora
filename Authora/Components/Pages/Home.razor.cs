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

        [Inject] IJSRuntime JSRuntime { get; set; } = default!;

        private List<User> _users = new List<User>();
        private List<Group> _allGroups = new List<Group>();
    }
}
