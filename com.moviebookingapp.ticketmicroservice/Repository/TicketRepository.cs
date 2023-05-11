using com.moviebookingapp.ticketmicroservice.Collection;
using MongoDB.Driver;

namespace com.moviebookingapp.ticketmicroservice.Repository
{
    public class TicketRepository:ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;

        public TicketRepository(IMongoDatabase mongoDatabase)
        {
            _ticketCollection = mongoDatabase.GetCollection<Ticket>("Ticket");
        }
        public async Task BookTicket(Ticket ticket)
        {
            await _ticketCollection.InsertOneAsync(ticket);
        }
    }
}
