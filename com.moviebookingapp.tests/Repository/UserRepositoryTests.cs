using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using com.moviebookingapp.usermicroservice.Repository;
using com.moviebookingapp.usermicroservice.Collection;

namespace com.moviebookingapp.tests.Repository
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private MockRepository mockRepository;

        private Mock<IMongoDatabase> mockMongoDatabase;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockMongoDatabase = this.mockRepository.Create<IMongoDatabase>();
        }

        private UserRepository CreateUserRepository()
        {
            return new UserRepository(
                this.mockMongoDatabase.Object);
        }

        [Test]
        public async Task ValidateUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string userEmail = null;
            string userLoginId = null;

            // Act
            var result = await userRepository.ValidateUser(
                userEmail,
                userLoginId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task ValidateLoginUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string userEmail = null;
            string userLoginId = null;

            // Act
            var result = await userRepository.ValidateLoginUser(
                userEmail,
                userLoginId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task Register_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            Users users = null;

            // Act
            await userRepository.Register(
                users);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task ForgotPassword_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string userEmail = null;

            // Act
            var result = await userRepository.ForgotPassword(
                userEmail);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateResetToken_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            Users users = null;

            // Act
            await userRepository.UpdateResetToken(
                users);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task ValidateResetToken_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            string token = null;

            // Act
            var result = await userRepository.ValidateResetToken(
                token);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdatePassword_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRepository = this.CreateUserRepository();
            Users users = null;

            // Act
            await userRepository.UpdatePassword(
                users);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
