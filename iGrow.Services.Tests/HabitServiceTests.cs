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
    using iGrow.Web.ViewModels.Habit;

    using static iGrow.GCommon.ApplicationConstants;

    [TestFixture]
    public class HabitServiceTests
    {
        private static readonly string UserId = Guid.NewGuid().ToString();
        private static readonly string HabitId = Guid.NewGuid().ToString();

        private static HabitFormViewModel CreateValidFormModel() => new HabitFormViewModel
        {
            Title = "Morning Run",
            StartDate = "01/01/2026",
            EndDate = "12/31/2026",
            Priority = 1,
            Note = "Run every morning",
            IsCompleted = false,
            RecurringTypeId = 1,
            AmountId = 1,
            Metric = 5,
            Unit = "km",
            CategoryId = 1
        };

        private static Habit CreateValidHabit() => new Habit
        {
            Id = Guid.Parse(HabitId),
            Title = "Morning Run",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 12, 31),
            Priority = 1,
            Note = "Run every morning",
            IsCompleted = false,
            RecurringTypeId = 1,
            AmountId = 1,
            Metric = 5,
            Unit = "km",
            CategoryId = 1,
            UserId = Guid.Parse(UserId),
            Category = new Category { Id = 1, Name = "Fitness", ImageUrl = "/images/categories/fitness.svg" },
            RecurringType = new RecurringType { Id = 1, Name = "Daily" },
            Amount = new Amount { Id = 1, Name = "Less than" }
        };

        // ── AddHabitAsync ──────────────────────────────────────────────

        [Test]
        public async Task AddHabitAsync_CallsRepositoryAndDoesNotThrow_RepositoryReturnsTrue()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.AddHabitAsync(It.IsAny<Habit>())).ReturnsAsync(true);

            var service = new HabitService(repoMock.Object);
            var model = CreateValidFormModel();

            // Act
            await service.AddHabitAsync(model, UserId);

            // Assert
            repoMock.Verify(r => r.AddHabitAsync(It.Is<Habit>(h =>
                h.Title == model.Title &&
                h.UserId == Guid.Parse(UserId))), Times.Once);
        }

        [Test]
        public void AddHabitAsync_ThrowsEntityPersistFailureException_RepositoryReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.AddHabitAsync(It.IsAny<Habit>())).ReturnsAsync(false);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.AddHabitAsync(CreateValidFormModel(), UserId));
        }

        // ── GetHabitByIdAsync ──────────────────────────────────────────

        [Test]
        public async Task GetHabitByIdAsync_Existing_ReturnsProjected()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);

            var service = new HabitService(repoMock.Object);

            // Act
            var result = await service.GetHabitByIdAsync(HabitId);

            // Assert
            Assert.That(result != null);
            Assert.That("Morning Run" == result.Title);
            Assert.That(1 == result.CategoryId);
        }

        [Test]
        public void GetHabitByIdAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.GetHabitByIdAsync(HabitId));
        }

        // ── GetHabitDetailsAsync ───────────────────────────────────────

        [Test]
        public async Task GetHabitDetailsAsync_Existing_ReturnsDetails()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);

            var service = new HabitService(repoMock.Object);

            // Act
            var result = await service.GetHabitDetailsAsync(HabitId);

            // Assert
            Assert.That(result != null);
            Assert.That("Morning Run" == result.Title);
            Assert.That("Fitness" == result.CategoryName);
            Assert.That("Daily" == result.RecurringTypeName);
            Assert.That("Less than" == result.AmountName);
        }

        [Test]
        public void GetHabitDetailsAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.GetHabitDetailsAsync(HabitId));
        }

        // ── EditHabitAsync ─────────────────────────────────────────────

        [Test]
        public async Task EditHabitAsync_ExistingAndEditSucceeds_CallsRepository()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);
            repoMock.Setup(r => r.EditHabitAsync(habit)).ReturnsAsync(true);

            var service = new HabitService(repoMock.Object);
            var model = CreateValidFormModel();
            model.Title = "Updated Title";

            // Act
            await service.EditHabitAsync(HabitId, model);

            // Assert
            repoMock.Verify(r => r.EditHabitAsync(It.Is<Habit>(h => h.Title == "Updated Title")), Times.Once);
        }

        [Test]
        public void EditHabitAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.EditHabitAsync(HabitId, CreateValidFormModel()));
        }

        [Test]
        public void EditHabitAsync_EditFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);
            repoMock.Setup(r => r.EditHabitAsync(habit)).ReturnsAsync(false);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.EditHabitAsync(HabitId, CreateValidFormModel()));
        }

        // ── GetHabitToBeDeletedAsync ───────────────────────────────────

        [Test]
        public async Task GetHabitToBeDeletedAsync_Existing_ReturnsDeleteModel()
        {
            // Arrange
            var habit = CreateValidHabit();
            habit.IsDeleted = false;
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);

            var service = new HabitService(repoMock.Object);

            // Act
            var result = await service.GetHabitToBeDeletedAsync(HabitId);

            // Assert
            Assert.That(result != null);
            Assert.That("Morning Run" == result.Title);
        }

        [Test]
        public void GetHabitToBeDeletedAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.GetHabitToBeDeletedAsync(HabitId));
        }

        // ── SoftDeleteHabitAsync ───────────────────────────────────────

        [Test]
        public async Task SoftDeleteHabitAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);
            repoMock.Setup(r => r.SoftDeleteHabitAsync(habit)).ReturnsAsync(true);

            var service = new HabitService(repoMock.Object);

            // Act
            await service.SoftDeleteHabitAsync(HabitId);

            // Assert
            repoMock.Verify(r => r.SoftDeleteHabitAsync(It.Is<Habit>(h => h.Id == Guid.Parse(HabitId))), Times.Once);
        }

        [Test]
        public void SoftDeleteHabitAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.SoftDeleteHabitAsync(HabitId));
        }

        [Test]
        public void SoftDeleteHabitAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);
            repoMock.Setup(r => r.SoftDeleteHabitAsync(habit)).ReturnsAsync(false);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.SoftDeleteHabitAsync(HabitId));
        }

        // ── HardDeleteHabitAsync ───────────────────────────────────────

        [Test]
        public async Task HardDeleteHabitAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);
            repoMock.Setup(r => r.HardDeleteHabitAsync(habit)).ReturnsAsync(true);

            var service = new HabitService(repoMock.Object);

            // Act
            await service.HardDeleteHabitAsync(HabitId);

            // Assert
            repoMock.Verify(r => r.HardDeleteHabitAsync(It.Is<Habit>(h => h.Id == Guid.Parse(HabitId))), Times.Once);
        }

        [Test]
        public void HardDeleteHabitAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.HardDeleteHabitAsync(HabitId));
        }

        [Test]
        public void HardDeleteHabitAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);
            repoMock.Setup(r => r.HardDeleteHabitAsync(habit)).ReturnsAsync(false);

            var service = new HabitService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(
                async () => await service.HardDeleteHabitAsync(HabitId));
        }

        // ── IsUserCreatorAsync ─────────────────────────────────────────

        [Test]
        public async Task IsUserCreatorAsync_UserIsCreator_ReturnsTrue()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);

            var service = new HabitService(repoMock.Object);

            // Act
            bool result = await service.IsUserCreatorAsync(HabitId, UserId);

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task IsUserCreatorAsync_UserIsNotCreator_ReturnsFalse()
        {
            // Arrange
            var habit = CreateValidHabit();
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync(habit);

            var service = new HabitService(repoMock.Object);

            // Act
            bool result = await service.IsUserCreatorAsync(HabitId, Guid.NewGuid().ToString());

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task IsUserCreatorAsync_HabitNotFound_ReturnsFalse()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetHabitByIdAsync(HabitId)).ReturnsAsync((Habit?)null);

            var service = new HabitService(repoMock.Object);

            // Act
            bool result = await service.IsUserCreatorAsync(HabitId, UserId);

            // Assert
            Assert.That(!result);
        }

        // ── GetHabitsCountAsync ────────────────────────────────────────

        [Test]
        public async Task GetHabitsCountAsync_ReturnsCount()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.CountAsync(UserId, It.IsAny<Expression<Func<Habit, bool>>?>())).ReturnsAsync(5);

            var service = new HabitService(repoMock.Object);

            // Act
            int result = await service.GetHabitsCountAsync(UserId);

            // Assert
            Assert.That(5 == result);
        }

        [Test]
        public async Task GetHabitsCountAsync_WithSearchQuery_ReturnsFilteredCount()
        {
            // Arrange
            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.CountAsync(UserId, It.IsAny<Expression<Func<Habit, bool>>?>())).ReturnsAsync(2);

            var service = new HabitService(repoMock.Object);

            // Act
            int result = await service.GetHabitsCountAsync(UserId, "run");

            // Assert
            Assert.That(2 == result);
        }

        // ── GetAllHabitsAsync ──────────────────────────────────────────

        [Test]
        public async Task GetAllHabitsAsync_ReturnsProjectedList()
        {
            // Arrange
            var habits = new List<Habit> { CreateValidHabit() };

            var repoMock = new Mock<IHabitRepository>();
            repoMock.Setup(r => r.GetAllHabitsNoTrackingByUserIdWithCategoryAndRecurringTypeAndAmountAsync(
                    UserId,
                    It.IsAny<Expression<Func<Habit, bool>>?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(habits);

            var service = new HabitService(repoMock.Object);

            // Act
            var result = (await service.GetAllHabitsAsync(UserId)).ToList();

            // Assert
            Assert.That(1 == result.Count);
            Assert.That("Morning Run" == result[0].Title);
            Assert.That("Fitness" == result[0].CategoryName);
        }
    }
}