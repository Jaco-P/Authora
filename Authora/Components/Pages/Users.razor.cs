using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Authora.Components.Pages
{
    public partial class Users : ComponentBase
    {

        [Inject] IUserService UserService { get; set; } = default!;

        [Inject] IJSRuntime JSRuntime { get; set; } = default!;

        private List<User> _users = new();
        private User _newUser = new();
        private User _editedUser = new();
        private Guid? _editingUserId = null;
        private string? _successMessage;

        protected override async Task OnInitializedAsync()
        {
            _users = await UserService.GetAllAsync();
        }

        private async Task HandleValidSubmit()
        {

            try
            {
                _newUser.Id = Guid.NewGuid();
                await UserService.AddAsync(_newUser);
                _users = await UserService.GetAllAsync();
                _successMessage = $"User '{_newUser.Username}' added successfully.";
                _newUser = new();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Console.WriteLine($"Erro saving user. The error was {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
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
        }

        private async Task SaveUser()
        {
            await UserService.UpdateAsync(_editedUser);
            _editingUserId = null;
            _users = await UserService.GetAllAsync();
            _successMessage = "User updated successfully.";
        }

        private void CancelEdit()
        {
            _editingUserId = null;
        }

        private async Task DeleteUser(Guid id)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this user?");
            if (!confirmed)
                return;

            await UserService.DeleteAsync(id);
            _users = await UserService.GetAllAsync();
            _successMessage = "User deleted successfully.";
        }
    }
}
