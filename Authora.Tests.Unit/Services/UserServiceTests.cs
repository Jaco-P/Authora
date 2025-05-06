using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authora.Application.Interfaces;
using Authora.Infrastructure.Services;
using Authora.Domain.Entities;
using Authora.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Authora.Tests.Unit.Services
{
    public class UserServiceTests
    {
        private async Task<AuthoraDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AuthoraDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AuthoraDbContext(options);

            // Seed data
            var group1 = new Group { Id = Guid.NewGuid(), Name = "Admins" };
            var group2 = new Group { Id = Guid.NewGuid(), Name = "Users" };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                UserGroups = new List<UserGroup>
                {
                    new() { Group = group1 },
                    new() { Group = group2 }
                }
            };

            await context.Groups.AddRangeAsync(group1, group2);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsersWithGroups()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.First().UserGroups.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var existing = context.Users.First();
            var service = new UserService(context);

            // Act
            var result = await service.GetByIdAsync(existing.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AuthoraDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AuthoraDbContext(options);
            var service = new UserService(context);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "newuser",
                Email = "new@example.com"
            };

            // Act
            await service.AddAsync(user);
            var users = await service.GetAllAsync();

            // Assert
            users.Should().ContainSingle(u => u.Username == "newuser");
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyUser()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);
            var user = context.Users.First();

            // Act
            user.Email = "updated@example.com";
            await service.UpdateAsync(user);

            // Assert
            var updated = await service.GetByIdAsync(user.Id);
            updated!.Email.Should().Be("updated@example.com");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);
            var user = context.Users.First();

            // Act
            await service.DeleteAsync(user.Id);

            // Assert
            var result = await service.GetByIdAsync(user.Id);
            result.Should().BeNull();
        }

        [Fact]
        public async Task AssignGroupsAsync_ShouldUpdateGroupMappings()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);
            var user = context.Users.First();
            var group = context.Groups.First();

            // Clear existing groups
            user.UserGroups.Clear();
            await context.SaveChangesAsync();

            // Act
            await service.AssignGroupsAsync(user.Id, new List<Guid> { group.Id });

            // Assert
            var updated = await service.GetByIdAsync(user.Id);
            updated!.UserGroups.Should().ContainSingle(g => g.GroupId == group.Id);
        }

        [Fact]
        public async Task AssignGroupsAsync_ShouldNotDuplicateGroupAssignments()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);
            var user = context.Users.First();
            var group = context.Groups.First();

            // Act
            await service.AssignGroupsAsync(user.Id, new List<Guid> { group.Id });
            await service.AssignGroupsAsync(user.Id, new List<Guid> { group.Id });

            // Assert
            var updated = await service.GetByIdAsync(user.Id);
            updated!.UserGroups.Count(ug => ug.GroupId == group.Id).Should().Be(1);
        }

        [Fact]
        public async Task AssignGroupsAsync_WithEmptyList_ShouldRemoveAllGroupMappings()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);
            var user = context.Users.First();

            // Act
            await service.AssignGroupsAsync(user.Id, new List<Guid>());

            // Assert
            var updated = await service.GetByIdAsync(user.Id);
            updated!.UserGroups.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUserAndUserGroups()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new UserService(context);
            var user = context.Users.First();

            // Act
            await service.DeleteAsync(user.Id);

            // Assert
            var result = await context.UserGroups
                .Where(ug => ug.UserId == user.Id)
                .ToListAsync();

            result.Should().BeEmpty();
        }
    }
}
