using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using com.moviebookingapp.moviemicroservice.Repository;
using com.moviebookingapp.moviemicroservice.Collection;

namespace com.moviebookingapp.tests.Repository
{
    [TestFixture]
    public class MovieRepositoryTests
    {
        private MockRepository mockRepository;

        private Mock<IMongoDatabase> mockMongoDatabase;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockMongoDatabase = this.mockRepository.Create<IMongoDatabase>();
        }

        private MovieRepository CreateMovieRepository()
        {
            return new MovieRepository(
                this.mockMongoDatabase.Object);
        }

        [Test]
        public async Task GetAllMovieAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();

            // Act
            var result = await movieRepository.GetAllMovieAsync();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetByMovienameAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();
            string moviename = null;

            // Act
            var result = await movieRepository.GetByMovienameAsync(
                moviename);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetByMovieandTheatrenameAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();
            string MovieName = null;
            string TheatreName = null;

            // Act
            var result = await movieRepository.GetByMovieandTheatrenameAsync(
                MovieName,
                TheatreName);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task BookTicket_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();
            Ticket ticket = null;

            // Act
            await movieRepository.BookTicket(
                ticket);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetBookedTicketDetails_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();
            string MovieName = null ;
            string TheatreName = null;

            // Act
            var result = await movieRepository.GetBookedTicketDetails(
                MovieName,
                TheatreName);
            //var expected = new Ticket { Id = "645b59ee8135026592aca8b9", Email = "keerthu@gmail.com",
            //    MovieName = "Paathan",
            //    TheatreName = "Inox",
            //    NumberOfTickets = 6,
            //    SeatNumber = { 1, 2, 3, 4, 5, 6 }
            //};

            // Assert
            //Assert.AreEqual(expected, result);
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateMovieDetails_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();
            Movie movie = null;

            // Act
            await movieRepository.UpdateMovieDetails(
                movie);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteMovieById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieRepository = this.CreateMovieRepository();
            string id = null;

            // Act
            await movieRepository.DeleteMovieById(
                id);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
