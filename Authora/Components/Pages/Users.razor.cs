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

            try
            {
                await UserService.AddAsync(_newUser);
                _users = await UserService.GetAllAsync();
                ShowSuccessMessage($"User '{_newUser.Username}' added successfully.");
            }
            catch (Exception ex) 
            {
                while (ex.InnerException != null) 
                {
                    ex = ex.InnerException;
                }

                Console.WriteLine($"Could not add User '{_newUser.Username}'. The error was {ex.Message}");
            }
            finally
            {
                StateHasChanged();

                //Reset _newUser and Group id so that a new user can be added
                _newUser = new();
                _newUserSelectedGroupId = null;
            }
            
            
        }

        private void EditUser(User user)
        {
            try
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
            catch (Exception ex) 
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Console.WriteLine($"There was an error updating user '{_editedUser.Username}'. The error was {ex.Message}");
            }
             
        }

        private async Task SaveUser()
        {
            try
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
            }
            catch (Exception ex) 
            {
                while (ex.InnerException != null) 
                {
                    ex = ex.InnerException;
                }
                ShowSuccessMessage($"User '{_editedUser.Username}' updating failed. The error was {ex.Message}");
                Console.Write($"User '{_editedUser.Username}' updating failed. The error was {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }   
                     
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
            try
            {
                await UserService.DeleteAsync(id);
                _users = await UserService.GetAllAsync();
                ShowSuccessMessage("User deleted successfully.");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                ShowSuccessMessage($"Failed to delete user with Id '{id}'. The error was {ex.Message}");
                Console.WriteLine($"");
            }
            
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
