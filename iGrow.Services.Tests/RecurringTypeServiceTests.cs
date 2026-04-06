namespace iGrow.Services.Tests
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using Moq;

    [TestFixture]
    public class RecurringTypeServiceTests
    {
        [Test]
        public async Task ItemExistsByNameAsync_ItemExists_ReturnsTrue()
        {
            // Arrange
            string name = "Daily";
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(name)).ReturnsAsync(true);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(name);

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task ItemExistsByNameAsync_ItemDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string name = "Sample recurring type";
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(name)).ReturnsAsync(false);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(name);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task ItemExistsByNameAsync_ItemNameIsEmptyString_ReturnsFalse()
        {
            // Arrange
            string name = string.Empty;
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(name);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task AddRecurringTypeAsync_CallsRepositoryAndDoesNotThrow_RepositoryReturnsTrue()
        {
            // Arrange
            string name = "Sample recurring type";
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.AddRecurringTypeAsync(It.IsAny<RecurringType>())).ReturnsAsync(true);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            await service.AddRecurringTypeAsync(name);

            // Assert
            repoMock.Verify(r => r.AddRecurringTypeAsync(It.Is<RecurringType>(a => a.Name == name)), Times.Once);
        }

        [Test]
        public void AddRecurringTypeAsync_ThrowsEntityPersistFailureException_RepositoryReturnsFalse()
        {
            // Arrange
            string name = "Greater than";
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.AddRecurringTypeAsync(It.IsAny<RecurringType>())).ReturnsAsync(false);

            var service = new RecurringTypeService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await service.AddRecurringTypeAsync(name));
        }

        [Test]
        public async Task GetAllRecurringTypesAsync_ReturnsProjectedList()
        {
            // Arrange
            var items = new List<RecurringType>
        {
            new RecurringType { Id = 1, Name = "Small" },
            new RecurringType { Id = 2, Name = "Large" }
        };

            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.GetAllRecurringTypesNoTrackingAsync()).ReturnsAsync(items);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            var result = (await service.GetAllRecurringTypesAsync()).ToList();

            // Assert
            Assert.That(2 == result.Count);
            Assert.That(1 == result[0].Id);
            Assert.That("Small" == result[0].Name);
            Assert.That(2 == result[1].Id);
            Assert.That("Large" == result[1].Name);
        }

        [Test]
        public async Task GetRecurringTypeByIdAsync_Existing_ReturnsProjected()
        {
            // Arrange
            int id = 10;
            var item = new RecurringType { Id = id, Name = "Medium" };

            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.GetRecurringTypeByIdAsync(id)).ReturnsAsync(item);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            var result = await service.GetRecurringTypeByIdAsync(id);

            // Assert
            Assert.That(result != null);
            Assert.That(id == result!.Id);
            Assert.That("Medium" == result.Name);
        }

        [Test]
        public void GetRecurringTypeByIdAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int id = 99;
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.GetRecurringTypeByIdAsync(id)).ReturnsAsync((RecurringType?)null);

            var service = new RecurringTypeService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await service.GetRecurringTypeByIdAsync(id));
        }

        [Test]
        public async Task DeleteRecurringTypeAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            int id = 5;
            var item = new RecurringType { Id = id, Name = "ToDelete" };

            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.GetRecurringTypeByIdAsync(id)).ReturnsAsync(item);
            repoMock.Setup(r => r.DeleteRecurringTypeAsync(item)).ReturnsAsync(true);

            var service = new RecurringTypeService(repoMock.Object);

            // Act
            await service.DeleteRecurringTypeAsync(id);

            // Assert
            repoMock.Verify(r => r.DeleteRecurringTypeAsync(It.Is<RecurringType>(a => a.Id == id && a.Name == "ToDelete")), Times.Once);
        }

        [Test]
        public void DeleteRecurringTypeAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int id = 123;
            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.GetRecurringTypeByIdAsync(id)).ReturnsAsync((RecurringType?)null);

            var service = new RecurringTypeService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await service.DeleteRecurringTypeAsync(id));
        }

        [Test]
        public void DeleteRecurringTypeAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            int id = 7;
            var item = new RecurringType { Id = id, Name = "WillFail" };

            var repoMock = new Mock<IRecurringTypeRepository>();
            repoMock.Setup(r => r.GetRecurringTypeByIdAsync(id)).ReturnsAsync(item);
            repoMock.Setup(r => r.DeleteRecurringTypeAsync(item)).ReturnsAsync(false);

            var service = new RecurringTypeService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await service.DeleteRecurringTypeAsync(id));
        }
        }
}
