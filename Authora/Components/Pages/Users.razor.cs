using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Authora.Components.Pages
{
    public partial class Users : ComponentBase
    {

        [Inject] IUserService UserService { get; set; } = default!;

        [Inject] IJSRuntime JSRuntime { get; set; } = default!;

        private List<User> _users = new();
        private List<Group> _allGroups = new();

        private User _newUser = new();
        private User _editedUser = new();

        private Guid? _editingUserId = null;
        private string? _successMessage;

        private EditContext? _editContext;
        private bool _editFormValid = false;

        private Dictionary<Guid, bool> _groupAssignments = new();
        private Guid? _newUserSelectedGroupId;

        protected override async Task OnInitializedAsync()
        {
            _users = await UserService.GetAllAsync();
            _allGroups = await UserService.GetAllGroupsAsync();
        }

        private async Task HandleValidSubmit()
        {
            _newUser.Id = Guid.NewGuid();

            if (_newUserSelectedGroupId.HasValue)
            {
                _newUser.UserGroups.Add(new UserGroup
                {
                    UserId = _newUser.Id,
                    GroupId = _newUserSelectedGroupId.Value
                });
            }

            await UserService.AddAsync(_newUser);
            _users = await UserService.GetAllAsync();
            ShowSuccessMessage($"User '{_newUser.Username}' added successfully.");
            StateHasChanged();            

            _newUser = new();
            _newUserSelectedGroupId = null;
        }

        private void EditUser(User user)
        {
            _editingUserId = user.Id;
            _editedUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            _editContext = new EditContext(_editedUser);
            _editContext.OnFieldChanged += (_, __) =>
            {
                _editFormValid = _editContext.Validate();
                StateHasChanged();
            };
            _editFormValid = _editContext.Validate();

            _groupAssignments = _allGroups.ToDictionary(
                g => g.Id,
                g => user.UserGroups.Any(ug => ug.GroupId == g.Id)
            );           
        }

        private async Task SaveUser()
        {
            await UserService.UpdateAsync(_editedUser);

            var selectedGroupIds = _groupAssignments
                .Where(g => g.Value)
                .Select(g => g.Key)
                .ToList();

            await UserService.AssignGroupsAsync(_editedUser.Id, selectedGroupIds);

            _editingUserId = null;
            _editFormValid = false;

            _users = await UserService.GetAllAsync();
            ShowSuccessMessage($"User '{_editedUser.Username}' updated successfully.");
            StateHasChanged();            
        }

        private void CancelEdit()
        {
            _editingUserId = null;
            _editFormValid = false;
        }

        private async Task DeleteUser(Guid id)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this user?");
            if (!confirmed)
                return;

            await UserService.DeleteAsync(id);
            _users = await UserService.GetAllAsync();
            ShowSuccessMessage("User deleted successfully.");
        }

        private void ShowSuccessMessage(string message)
        {
            _successMessage = message;
            StateHasChanged();

            _ = Task.Run(async () =>
            {
                await Task.Delay(4000);
                _successMessage = null;
                await InvokeAsync(StateHasChanged);
            });
        }
    }
}
