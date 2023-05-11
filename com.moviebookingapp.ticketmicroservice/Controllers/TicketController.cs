using com.moviebookingapp.moviemicroservice.Repository;
using com.moviebookingapp.ticketmicroservice.Collection;
using com.moviebookingapp.ticketmicroservice.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.moviebookingapp.ticketmicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _iticketRepository;
        private readonly IMovieRepository _imovieRepository;
        public TicketController(ITicketRepository ticketRepository)
        {
            _iticketRepository = ticketRepository;
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // Book Tickets
        [Route("/api/v1.0/moviebooking/moviename/add")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ticket ticket)
        {
            var movie = _imovieRepository.GetByMovienameAsync(ticket.MovieName);
            if(movie == null) 
            {
                return NotFound("No movie availabe with this name");
            }
            _iticketRepository.BookTicket(ticket);
            return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket);

        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
