namespace iGrow.Services.Tests
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using Moq;

    [TestFixture]
    public class AmountServiceTests
    {
        [Test]
        public async Task ItemExistsByNameAsync_ItemExists_ReturnsTrue()
        {
            // Arrange
            string amountName = "Less than";
            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(amountName)).ReturnsAsync(true);

            var service = new AmountService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(amountName);

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task ItemExistsByNameAsync_ItemDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string amountName = "Sample amount";
            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.ItemExistsByNameAsync(amountName)).ReturnsAsync(false);

            var service = new AmountService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(amountName);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task ItemExistsByNameAsync_ItemNameIsEmptyString_ReturnsFalse()
        {
            // Arrange
            string amountName = string.Empty;
            var repoMock = new Mock<IAmountRepository>();
            // Expect the service to short-circuit or call repository; return false to be safe
            repoMock.Setup(r => r.ItemExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);

            var service = new AmountService(repoMock.Object);

            // Act
            bool result = await service.ItemExistsByNameAsync(amountName);

            // Assert
            Assert.That(!result);
        }

        [Test]
        public async Task AddAmountAsync_CallsRepositoryAndDoesNotThrow_RepositoryReturnsTrue()
        {
            // Arrange
            string name = "Sample amount";
            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.AddAmountAsync(It.IsAny<Amount>())).ReturnsAsync(true);

            var service = new AmountService(repoMock.Object);

            // Act
            await service.AddAmountAsync(name);

            // Assert - repository AddAmountAsync was called with an Amount that has the expected Name
            repoMock.Verify(r => r.AddAmountAsync(It.Is<Amount>(a => a.Name == name)), Times.Once);
        }

        [Test]
        public void AddAmountAsync_ThrowsEntityPersistFailureException_RepositoryReturnsFalse()
        {
            // Arrange
            string name = "Greater than";
            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.AddAmountAsync(It.IsAny<Amount>())).ReturnsAsync(false);

            var service = new AmountService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await service.AddAmountAsync(name));
        }

        [Test]
        public async Task GetAllAmountsAsync_ReturnsProjectedList()
        {
            // Arrange
            var amounts = new List<Amount>
            {
                new Amount { Id = 1, Name = "Small" },
                new Amount { Id = 2, Name = "Large" }
            };

            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.GetAllAmountsNoTrackingAsync()).ReturnsAsync(amounts);

            var service = new AmountService(repoMock.Object);

            // Act
            var result = (await service.GetAllAmountsAsync()).ToList();

            // Assert
            Assert.That(2 == result.Count);
            Assert.That(1 == result[0].Id);
            Assert.That("Small" == result[0].Name);
            Assert.That(2 == result[1].Id);
            Assert.That("Large" == result[1].Name);
        }

        [Test]
        public async Task GetAmountByIdAsync_Existing_ReturnsProjected()
        {
            // Arrange
            int id = 10;
            var amount = new Amount { Id = id, Name = "Medium" };

            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.GetAmountByIdAsync(id)).ReturnsAsync(amount);

            var service = new AmountService(repoMock.Object);

            // Act
            var result = await service.GetAmountByIdAsync(id);

            // Assert
            Assert.That(result != null);
            Assert.That(id == result!.Id);
            Assert.That("Medium" == result.Name);
        }

        [Test]
        public void GetAmountByIdAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int id = 99;
            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.GetAmountByIdAsync(id)).ReturnsAsync((Amount?)null);

            var service = new AmountService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await service.GetAmountByIdAsync(id));
        }

        [Test]
        public async Task DeleteAmountAsync_ExistingAndDeleteSucceeds_CallsRepository()
        {
            // Arrange
            int id = 5;
            var amount = new Amount { Id = id, Name = "ToDelete" };

            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.GetAmountByIdAsync(id)).ReturnsAsync(amount);
            repoMock.Setup(r => r.DeleteAmountAsync(amount)).ReturnsAsync(true);

            var service = new AmountService(repoMock.Object);

            // Act
            await service.DeleteAmountAsync(id);

            // Assert
            repoMock.Verify(r => r.DeleteAmountAsync(It.Is<Amount>(a => a.Id == id && a.Name == "ToDelete")), Times.Once);
        }

        [Test]
        public void DeleteAmountAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int id = 123;
            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.GetAmountByIdAsync(id)).ReturnsAsync((Amount?)null);

            var service = new AmountService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await service.DeleteAmountAsync(id));
        }

        [Test]
        public void DeleteAmountAsync_DeleteFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            int id = 7;
            var amount = new Amount { Id = id, Name = "WillFail" };

            var repoMock = new Mock<IAmountRepository>();
            repoMock.Setup(r => r.GetAmountByIdAsync(id)).ReturnsAsync(amount);
            repoMock.Setup(r => r.DeleteAmountAsync(amount)).ReturnsAsync(false);

            var service = new AmountService(repoMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await service.DeleteAmountAsync(id));
        }
    }
}
