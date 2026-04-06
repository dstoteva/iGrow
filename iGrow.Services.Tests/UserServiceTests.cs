namespace iGrow.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Moq;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Web.ViewModels.Admin.User;

    [TestFixture]
    public class UserServiceTests
    {
        private static readonly Guid AdminUserId = Guid.NewGuid();
        private static readonly Guid RegularUserId = Guid.NewGuid();

        private static ApplicationUser CreateRegularUser() => new ApplicationUser
        {
            Id = RegularUserId,
            Email = "user@example.com",
            UserName = "user@example.com"
        };

        // ── GetAllManageableUsersAsync ─────────────────────────────────

        [Test]
        public async Task GetAllManageableUsersAsync_ReturnsUsersExcludingAdmin()
        {
            // Arrange
            var users = new List<ApplicationUser> { CreateRegularUser() };

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetAllUsersAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>?>()))
                .ReturnsAsync(users);
            repoMock.Setup(r => r.GetUserRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "User" });

            var service = new UserService(repoMock.Object);

            // Act
            var result = (await service.GetAllManageableUsersAsync(AdminUserId.ToString())).ToList();

            // Assert
            Assert.That(1 == result.Count);
            Assert.That(RegularUserId == result[0].Id);
            Assert.That("user@example.com" == result[0].Email);
        }

        [Test]
        public async Task GetAllManageableUsersAsync_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetAllUsersAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>?>()))
                .ReturnsAsync(new List<ApplicationUser>());

            var service = new UserService(repoMock.Object);

            // Act
            var result = (await service.GetAllManageableUsersAsync(AdminUserId.ToString())).ToList();

            // Assert
            Assert.That(0 == result.Count);
        }

        // ── AssignRoleToUserAsync ──────────────────────────────────────

        [Test]
        public async Task AssignRoleToUserAsync_ValidInput_ReturnsTrue()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.UpdateUserRoleAsync(RegularUserId, "Admin", false))
                .ReturnsAsync(true);

            var service = new UserService(repoMock.Object);

            // Act
            bool result = await service.AssignRoleToUserAsync(RegularUserId, "Admin");

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task AssignRoleToUserAsync_RepositoryReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.UpdateUserRoleAsync(RegularUserId, "Admin", false))
                .ReturnsAsync(false);

            var service = new UserService(repoMock.Object);

            // Act
            bool result = await service.AssignRoleToUserAsync(RegularUserId, "Admin");

            // Assert
            Assert.That(!result);
        }

        [Test]
        public void AssignRoleToUserAsync_EmptyGuid_ThrowsEntityInputDataFormatException()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var service = new UserService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityInputDataFormatException>(
                async () => await service.AssignRoleToUserAsync(Guid.Empty, "Admin"));
        }

        [Test]
        public void AssignRoleToUserAsync_EmptyRole_ThrowsEntityInputDataFormatException()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var service = new UserService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityInputDataFormatException>(
                async () => await service.AssignRoleToUserAsync(RegularUserId, string.Empty));
        }

        [Test]
        public void AssignRoleToUserAsync_WhitespaceRole_ThrowsEntityInputDataFormatException()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var service = new UserService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityInputDataFormatException>(
                async () => await service.AssignRoleToUserAsync(RegularUserId, "   "));
        }

        // ── RemoveRoleFromUserAsync ────────────────────────────────────

        [Test]
        public async Task RemoveRoleFromUserAsync_ValidInput_ReturnsTrue()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.UpdateUserRoleAsync(RegularUserId, "Admin", true))
                .ReturnsAsync(true);

            var service = new UserService(repoMock.Object);

            // Act
            bool result = await service.RemoveRoleFromUserAsync(RegularUserId, "Admin");

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task RemoveRoleFromUserAsync_RepositoryReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.UpdateUserRoleAsync(RegularUserId, "Admin", true))
                .ReturnsAsync(false);

            var service = new UserService(repoMock.Object);

            // Act
            bool result = await service.RemoveRoleFromUserAsync(RegularUserId, "Admin");

            // Assert
            Assert.That(!result);
        }

        [Test]
        public void RemoveRoleFromUserAsync_EmptyGuid_ThrowsEntityInputDataFormatException()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var service = new UserService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityInputDataFormatException>(
                async () => await service.RemoveRoleFromUserAsync(Guid.Empty, "Admin"));
        }

        [Test]
        public void RemoveRoleFromUserAsync_EmptyRole_ThrowsEntityInputDataFormatException()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var service = new UserService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityInputDataFormatException>(
                async () => await service.RemoveRoleFromUserAsync(RegularUserId, string.Empty));
        }

        // ── DeleteUserAsync ────────────────────────────────────────────

        [Test]
        public async Task DeleteUserAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.DeleteUserAsync(RegularUserId)).ReturnsAsync(true);

            var service = new UserService(repoMock.Object);

            // Act
            bool result = await service.DeleteUserAsync(RegularUserId);

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task DeleteUserAsync_RepositoryReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.DeleteUserAsync(RegularUserId)).ReturnsAsync(false);

            var service = new UserService(repoMock.Object);

            // Act
            bool result = await service.DeleteUserAsync(RegularUserId);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public void DeleteUserAsync_EmptyGuid_ThrowsEntityInputDataFormatException()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            var service = new UserService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityInputDataFormatException>(
                async () => await service.DeleteUserAsync(Guid.Empty));
        }
    }
}