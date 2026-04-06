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
    using iGrow.Web.ViewModels.MyTask;

    using static iGrow.GCommon.ApplicationConstants;

    [TestFixture]
    public class MyTaskServiceTests
    {
        private static readonly string UserId = Guid.NewGuid().ToString();
        private static readonly string TaskId = Guid.NewGuid().ToString();

        private static MyTaskFormViewModel CreateValidFormModel() => new MyTaskFormViewModel
        {
            Title = "Buy groceries",
            Date = "01/15/2026",
            Priority = 2,
            Note = "Milk, eggs, bread",
            IsCompleted = false,
            RecurringTypeId = 1,
            CategoryId = 1,
            UserId = UserId
        };

        private static MyTask CreateValidTask() => new MyTask
        {
            Id = Guid.Parse(TaskId),
            Title = "Buy groceries",
            Date = new DateTime(2026, 1, 15),
            Priority = 2,
            Note = "Milk, eggs, bread",
            IsCompleted = false,
            RecurringTypeId = 1,
            CategoryId = 1,
            UserId = Guid.Parse(UserId),
            Category = new Category { Id = 1, Name = "Shopping", ImageUrl = "/images/categories/shopping.svg" },
            RecurringType = new RecurringType { Id = 1, Name = "Daily" }
        };

        // ── AddTaskAsync ───────────────────────────────────────────────

        [Test]
        public async Task AddTaskAsync_CallsRepositoryAndDoesNotThrow_RepositoryReturnsTrue()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.AddTaskAsync(It.IsAny<MyTask>())).ReturnsAsync(true);

            var service = new MyTaskService(repoMock.Object);
            var model = CreateValidFormModel();

            // Act
            await service.AddTaskAsync(model, UserId);

            // Assert
            repoMock.Verify(r => r.AddTaskAsync(It.Is<MyTask>(t =>
                t.Title == model.Title &&
                t.UserId == Guid.Parse(UserId))), Times.Once);
        }

        [Test]
        public void AddTaskAsync_ThrowsEntityPersistFailureException_RepositoryReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.AddTaskAsync(It.IsAny<MyTask>())).ReturnsAsync(false);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.AddTaskAsync(CreateValidFormModel(), UserId));
        }

        // ── GetTaskByIdAsync ───────────────────────────────────────────

        [Test]
        public async Task GetTaskByIdAsync_Existing_ReturnsProjected()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);

            var service = new MyTaskService(repoMock.Object);

            // Act
            var result = await service.GetTaskByIdAsync(TaskId);

            // Assert
            Assert.That(result != null);
            Assert.That("Buy groceries" == result.Title);
            Assert.That(1 == result.CategoryId);
        }

        [Test]
        public void GetTaskByIdAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.GetTaskByIdAsync(TaskId));
        }

        // ── GetTaskDetailsAsync ────────────────────────────────────────

        [Test]
        public async Task GetTaskDetailsAsync_Existing_ReturnsDetails()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);

            var service = new MyTaskService(repoMock.Object);

            // Act
            var result = await service.GetTaskDetailsAsync(TaskId);

            // Assert
            Assert.That(result != null);
            Assert.That("Buy groceries" == result.Title);
            Assert.That("Shopping" == result.CategoryName);
            Assert.That("Daily" == result.RecurringTypeName);
        }

        [Test]
        public void GetTaskDetailsAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.GetTaskDetailsAsync(TaskId));
        }

        // ── EditTaskAsync ──────────────────────────────────────────────

        [Test]
        public async Task EditTaskAsync_ExistingAndEditSucceeds_CallsRepository()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);
            repoMock.Setup(r => r.EditTaskAsync(task)).ReturnsAsync(true);

            var service = new MyTaskService(repoMock.Object);
            var model = CreateValidFormModel();
            model.Title = "Updated Title";

            // Act
            await service.EditTaskAsync(TaskId, model);

            // Assert
            repoMock.Verify(r => r.EditTaskAsync(It.Is<MyTask>(t => t.Title == "Updated Title")), Times.Once);
        }

        [Test]
        public void EditTaskAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.EditTaskAsync(TaskId, CreateValidFormModel()));
        }

        [Test]
        public void EditTaskAsync_EditFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);
            repoMock.Setup(r => r.EditTaskAsync(task)).ReturnsAsync(false);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.EditTaskAsync(TaskId, CreateValidFormModel()));
        }

        // ── GetTaskToBeDeletedAsync ────────────────────────────────────

        [Test]
        public async Task GetTaskToBeDeletedAsync_Existing_ReturnsDeleteModel()
        {
            // Arrange
            var task = CreateValidTask();
            task.IsDeleted = false;
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);

            var service = new MyTaskService(repoMock.Object);

            // Act
            var result = await service.GetTaskToBeDeletedAsync(TaskId);

            // Assert
            Assert.That(result != null);
            Assert.That("Buy groceries" == result.Title);
        }

        [Test]
        public void GetTaskToBeDeletedAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.GetTaskToBeDeletedAsync(TaskId));
        }

        // ── SoftDeleteTaskAsync ────────────────────────────────────────

        [Test]
        public async Task SoftDeleteTaskAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);
            repoMock.Setup(r => r.SoftDeleteTaskAsync(task)).ReturnsAsync(true);

            var service = new MyTaskService(repoMock.Object);

            // Act
            await service.SoftDeleteTaskAsync(TaskId);

            // Assert
            repoMock.Verify(r => r.SoftDeleteTaskAsync(It.Is<MyTask>(t => t.Id == Guid.Parse(TaskId))), Times.Once);
        }

        [Test]
        public void SoftDeleteTaskAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.SoftDeleteTaskAsync(TaskId));
        }

        [Test]
        public void SoftDeleteTaskAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);
            repoMock.Setup(r => r.SoftDeleteTaskAsync(task)).ReturnsAsync(false);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.SoftDeleteTaskAsync(TaskId));
        }

        // ── HardDeleteTaskAsync ────────────────────────────────────────

        [Test]
        public async Task HardDeleteTaskAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);
            repoMock.Setup(r => r.HardDeleteTaskAsync(task)).ReturnsAsync(true);

            var service = new MyTaskService(repoMock.Object);

            // Act
            await service.HardDeleteTaskAsync(TaskId);

            // Assert
            repoMock.Verify(r => r.HardDeleteTaskAsync(It.Is<MyTask>(t => t.Id == Guid.Parse(TaskId))), Times.Once);
        }

        [Test]
        public void HardDeleteTaskAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.HardDeleteTaskAsync(TaskId));
        }

        [Test]
        public void HardDeleteTaskAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);
            repoMock.Setup(r => r.HardDeleteTaskAsync(task)).ReturnsAsync(false);

            var service = new MyTaskService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.HardDeleteTaskAsync(TaskId));
        }

        // ── IsUserCreatorAsync ─────────────────────────────────────────

        [Test]
        public async Task IsUserCreatorAsync_UserIsCreator_ReturnsTrue()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);

            var service = new MyTaskService(repoMock.Object);

            // Act
            bool result = await service.IsUserCreatorAsync(TaskId, UserId);

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task IsUserCreatorAsync_UserIsNotCreator_ReturnsFalse()
        {
            // Arrange
            var task = CreateValidTask();
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync(task);

            var service = new MyTaskService(repoMock.Object);

            // Act
            bool result = await service.IsUserCreatorAsync(TaskId, Guid.NewGuid().ToString());

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task IsUserCreatorAsync_TaskNotFound_ReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetTaskByIdAsync(TaskId)).ReturnsAsync((MyTask?)null);

            var service = new MyTaskService(repoMock.Object);

            // Act
            bool result = await service.IsUserCreatorAsync(TaskId, UserId);

            // Assert
            Assert.That(!result);
        }

        // ── GetTasksCountAsync ─────────────────────────────────────────

        [Test]
        public async Task GetTasksCountAsync_ReturnsCount()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.CountAsync(UserId, It.IsAny<Expression<Func<MyTask, bool>>?>())).ReturnsAsync(5);

            var service = new MyTaskService(repoMock.Object);

            // Act
            int result = await service.GetTasksCountAsync(UserId);

            // Assert
            Assert.That(5 == result);
        }

        [Test]
        public async Task GetTasksCountAsync_WithSearchQuery_ReturnsFilteredCount()
        {
            // Arrange
            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.CountAsync(UserId, It.IsAny<Expression<Func<MyTask, bool>>?>())).ReturnsAsync(2);

            var service = new MyTaskService(repoMock.Object);

            // Act
            int result = await service.GetTasksCountAsync(UserId, "groceries");

            // Assert
            Assert.That(2 == result);
        }

        // ── GetAllTasksAsync ───────────────────────────────────────────

        [Test]
        public async Task GetAllTasksAsync_ReturnsProjectedList()
        {
            // Arrange
            var tasks = new List<MyTask> { CreateValidTask() };

            var repoMock = new Mock<IMyTaskRepository>();
            repoMock.Setup(r => r.GetAllTasksNoTrackingByUserIdWithCategoryAndRecurringTypeAsync(
                    UserId,
                    It.IsAny<Expression<Func<MyTask, bool>>?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(tasks);

            var service = new MyTaskService(repoMock.Object);

            // Act
            var result = (await service.GetAllTasksAsync(UserId)).ToList();

            // Assert
            Assert.That(1 == result.Count);
            Assert.That("Buy groceries" == result[0].Title);
            Assert.That("Shopping" == result[0].CategoryName);
        }
    }
}