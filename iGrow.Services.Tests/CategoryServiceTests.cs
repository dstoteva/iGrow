namespace iGrow.Services.Tests
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using Moq;

    [TestFixture]
    public class CategoryServiceTests
    {
        [Test]
        public async Task ItemExistsByNameAsync_ItemExists_ReturnsTrue()
        {
            // Arrange
            string name = "Health";
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(name)).ReturnsAsync(true);

            var service = new CategoryService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(name);

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task ItemExistsByNameAsync_ItemDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string name = "Sample category";
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(name)).ReturnsAsync(false);

            var service = new CategoryService(repoMock.Object);

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
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);

            var service = new CategoryService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(name);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task AddCategoryAsync_CallsRepositoryAndDoesNotThrow_RepositoryReturnsTrue()
        {
            // Arrange
            string name = "Sample category";
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.AddCategoryAsync(It.IsAny<Category>())).ReturnsAsync(true);

            var service = new CategoryService(repoMock.Object);

            // Act
            await service.AddCategoryAsync(name, null);

            // Assert
            repoMock.Verify(r => r.AddCategoryAsync(It.Is<Category>(a => a.Name == name)), Times.Once);
        }

        [Test]
        public void AddCategoryAsync_ThrowsEntityPersistFailureException_RepositoryReturnsFalse()
        {
            // Arrange
            string name = "Greater than";
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.AddCategoryAsync(It.IsAny<Category>())).ReturnsAsync(false);

            var service = new CategoryService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await service.AddCategoryAsync(name, null));
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsProjectedList()
        {
            // Arrange
            var items = new List<Category>
        {
            new Category { Id = 1, Name = "Small" },
            new Category { Id = 2, Name = "Large" }
        };

            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetAllCategoriesNoTrackingAsync()).ReturnsAsync(items);

            var service = new CategoryService(repoMock.Object);

            // Act
            var result = (await service.GetAllCategoriesAsync()).ToList();

            // Assert
            Assert.That(2 == result.Count);
            Assert.That(1 == result[0].Id);
            Assert.That("Small" == result[0].Name);
            Assert.That(2 == result[1].Id);
            Assert.That("Large" == result[1].Name);
        }

        [Test]
        public async Task GetCategoryByIdAsync_Existing_ReturnsProjected()
        {
            // Arrange
            int id = 10;
            var item = new Category { Id = id, Name = "Medium" };

            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync(item);

            var service = new CategoryService(repoMock.Object);

            // Act
            var result = await service.GetCategoryByIdAsync(id);

            // Assert
            Assert.That(result != null);
            Assert.That(id == result!.Id);
            Assert.That("Medium" == result.Name);
        }

        [Test]
        public void GetCategoryByIdAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int id = 99;
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync((Category?)null);

            var service = new CategoryService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await service.GetCategoryByIdAsync(id));
        }

        [Test]
        public async Task DeleteCategoryAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            int id = 5;
            var item = new Category { Id = id, Name = "ToDelete" };

            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync(item);
            repoMock.Setup(r => r.DeleteCategoryAsync(item)).ReturnsAsync(true);

            var service = new CategoryService(repoMock.Object);

            // Act
            await service.DeleteCategoryAsync(id);

            // Assert
            repoMock.Verify(r => r.DeleteCategoryAsync(It.Is<Category>(a => a.Id == id && a.Name == "ToDelete")), Times.Once);
        }

        [Test]
        public void DeleteCategoryAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int id = 123;
            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync((Category?)null);

            var service = new CategoryService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await service.DeleteCategoryAsync(id));
        }

        [Test]
        public void DeleteCategoryAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            int id = 7;
            var item = new Category { Id = id, Name = "WillFail" };

            var repoMock = new Mock<ICategoryRepository>();
            repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync(item);
            repoMock.Setup(r => r.DeleteCategoryAsync(item)).ReturnsAsync(false);

            var service = new CategoryService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await service.DeleteCategoryAsync(id));
        }
    }
}
