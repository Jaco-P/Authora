using Authora.Application.Interfaces;
using Authora.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Authora.Components.Pages
{
    public partial class Users : ComponentBase
    {

        [Inject] IUserService UserService { get; set; } = default!;

        private List<User>? _users;
        private User _newUser = new User();
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
                _successMessage = $"User '{_newUser.Username}' added successfully.";
                _newUser = new();

            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Console.WriteLine($"Could not add user. The error was {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }


        }
    }
}

