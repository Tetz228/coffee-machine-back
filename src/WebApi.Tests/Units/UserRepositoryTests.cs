namespace WebApiTests.Units
{
    using Base;

    using FluentAssertions;

    using NUnit.Framework;

    using WebApi.Db.Models;

    /// <summary>
    ///     Тестирование репозитория для взаимодействия с пользователем.
    /// </summary>
    [TestFixture]
    public class UserRepositoryTests : BaseRepositoryTests
    {
        /// <summary>
        ///     Настройка тестирования.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        /// <summary>
        ///     Освобождение ресурсов после тестирования.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context.Dispose();
        }

        /// <summary>
        ///     Тестирование добавления пользователя.
        /// </summary>
        [Test]
        public async Task AddUserAsync_ShouldAddNewUser_ReturnAddedUser()
        {
            // Arrange.
            var creatingUser = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Act.
            await UnitOfWork.UserRepository.AddUserAsync(creatingUser);

            await UnitOfWork.SaveChangesAsync();

            var foundUser = await UnitOfWork.UserRepository.GetUserAsync(creatingUser.Id);

            // Assert.
            foundUser.Id.Should().Be(creatingUser.Id);
        }

        /// <summary>
        ///     Тестирование обновления пользователя.
        /// </summary>
        [Test]
        public async Task UpdateUser_ShouldUpdateUser_ReturnUpdatedUser()
        {
            // Arrange.
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Act.
            await UnitOfWork.UserRepository.AddUserAsync(user);

            await UnitOfWork.SaveChangesAsync();

            user.Balance = 2000;

            UnitOfWork.UserRepository.UpdateUser(user);

            await UnitOfWork.SaveChangesAsync();

            var foundUser = await UnitOfWork.UserRepository.GetUserAsync(user.Id);

            // Assert.
            foundUser.Balance.Should().Be(user.Balance);
        }

        /// <summary>
        ///     Тестирование удаления пользователя.
        /// </summary>
        [Test]
        public async Task DeleteUser_ShouldDeleteUser_ReturnNull()
        {
            // Arrange.
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Act.
            await UnitOfWork.UserRepository.AddUserAsync(user);

            await UnitOfWork.SaveChangesAsync();

            UnitOfWork.UserRepository.DeleteUser(user);

            await UnitOfWork.SaveChangesAsync();

            var foundUser = await UnitOfWork.UserRepository.GetUserAsync(user.Id);

            // Assert.
            foundUser.Should().BeNull();
        }

        /// <summary>
        ///     Тестирование получения пользователя.
        /// </summary>
        [Test]
        public async Task GetUserAsync_ShouldExistUser_ReturnUser()
        {
            // Arrange.
            var creatingUser = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Act.
            await UnitOfWork.UserRepository.AddUserAsync(creatingUser);

            await UnitOfWork.SaveChangesAsync();

            var foundUser = await UnitOfWork.UserRepository.GetUserAsync(creatingUser.Id);

            // Assert.
            foundUser.Id.Should().Be(creatingUser.Id);
        }

        /// <summary>
        ///     Тестирование получения пользователя по логину.
        /// </summary>
        [Test]
        public async Task GetUserAsync_ShouldExistLoginUser_ReturnUser()
        {
            // Arrange.
            var creatingUser = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Act.
            await UnitOfWork.UserRepository.AddUserAsync(creatingUser);

            await UnitOfWork.SaveChangesAsync();

            var foundUser = await UnitOfWork.UserRepository.GetUserAsync(creatingUser.Login);

            // Assert.
            foundUser.Login.Should().Be(creatingUser.Login);
        }

        /// <summary>
        ///     Тестирование получения пользователя по логину и паролю.
        /// </summary>
        [Test]
        public async Task GetUserAsync_ShouldExistLoginAndPasswordUser_ReturnUser()
        {
            // Arrange.
            var creatingUser = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Act.
            await UnitOfWork.UserRepository.AddUserAsync(creatingUser);

            await UnitOfWork.SaveChangesAsync();

            var foundUser = await UnitOfWork.UserRepository.GetUserAsync(creatingUser.Login);

            // Assert.
            foundUser.Login.Should().Be(creatingUser.Login);
            foundUser.Password.Should().Be(creatingUser.Password);
        }

        /// <summary>
        ///     Тестирование получения всех пользователей.
        /// </summary>
        [Test]
        public async Task GetUsers_ShouldExistUsers_ReturnUsers()
        {
            var ghlov = new User
            {
                Id = Guid.NewGuid(),
                Login = "Ghlov",
                Password = "123456789",
                Name = "Вадим",
                Balance = 2500
            };
            var Lord = new User
            {
                Id = Guid.NewGuid(),
                Login = "Lord",
                Password = "qwerty123",
                Name = "Олег",
                Balance = 1500
            };

            // Arrange.
            await UnitOfWork.UserRepository.AddUserAsync(Lord);
            await UnitOfWork.UserRepository.AddUserAsync(ghlov);

            await UnitOfWork.SaveChangesAsync();

            // Act.
            var foundUsers = UnitOfWork.UserRepository.GetAllUsers();

            // Assert.
            foundUsers.Should().NotBeNull();
        }
    }
}