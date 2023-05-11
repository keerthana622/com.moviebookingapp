using com.moviebookingapp.moviemicroservice.Collection;
using com.moviebookingapp.moviemicroservice.Models;
using com.moviebookingapp.moviemicroservice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.moviebookingapp.moviemicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _imovieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            _imovieRepository = movieRepository;
        }

        //Get all movies
        [Route("/api/v1.0/moviebooking/all")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var movies = await _imovieRepository.GetAllMovieAsync();
            return Ok(movies);
        }

        // Search By Moviename
        [Route("/api/v1.0/moviebooking/movies/search/moviename")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string moviename)
        {
            var movie = await _imovieRepository.GetByMovienameAsync(moviename);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // Book Tickets
        [Route("/api/v1.0/moviebooking/moviename/add")]
        [HttpPost, Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            var movie = _imovieRepository.GetByMovienameAsync(ticket.MovieName);
            if (movie == null)
            {
                return NotFound("No movie availabe with this name");
            }
            await _imovieRepository.BookTicket(ticket);
            return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket);

        }

        // Update Movie Tickets
        [Route("/api/v1.0/moviebooking/moviename/update/ticket")]
        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] GetMovieRequest movie)
        {
            var movieDetails=await _imovieRepository.GetByMovieandTheatrenameAsync(movie.MovieName,movie.TheatreName);
            if (movieDetails == null)
            {
                return NotFound("No movie found");
            }
            var bookedMovieDetails =await _imovieRepository.GetBookedTicketDetails(movie.MovieName,movie.TheatreName);
            if(bookedMovieDetails==null)
            {
                return NotFound("Ticket for above is not booked");
            }
            movieDetails.NoOfSeatsAlloted -= bookedMovieDetails.NumberOfTickets;
            await _imovieRepository.UpdateMovieDetails(movieDetails);
            return Ok("Movie details updated successfully!!");
        }

        // DELETE MOVIE
        [Route("/api/v1.0/moviebooking/moviename/delete/id")]
        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromQuery] GetMovieRequest movie)
        {
            var movieDetails = await _imovieRepository.GetByMovieandTheatrenameAsync(movie.MovieName, movie.TheatreName);
            if (movieDetails == null)
            {
                return NotFound();
            }
            await _imovieRepository.DeleteMovieById(movieDetails.Id);
            return Ok("Movie Deleted Successfully");

        }
    }
}
