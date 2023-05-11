using com.moviebookingapp.ticketmicroservice.Collection;

namespace com.moviebookingapp.ticketmicroservice.Repository
{
    public interface ITicketRepository
    {
        Task BookTicket(Ticket ticket);
        Task<Ticket> GetTicketByMovieandTheatreName(string movieName, string theatreName);
    }
}
