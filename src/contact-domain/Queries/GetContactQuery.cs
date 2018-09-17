namespace Contact.Projection.Query
{
    using Contact.Domain;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Query;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;

    public class GetContactQuery
    {
        public string ContactId { get; set; }
    }

    public class GetContactQueryHandler : IQueryHandler<GetContactQuery, Contact>
    {
        private readonly IRepository<Contact> contactRepository;

        public GetContactQueryHandler(IRepository<Contact> contactQueries)
        {
            this.contactRepository = contactQueries;
        }

        public Task<Contact> Handle(GetContactQuery query)
        {
            var filter = Builders<Contact>.Filter.Eq("Id", query.ContactId);

            return contactRepository.Collection
                                .Find(filter)
                                .FirstOrDefaultAsync();
        }
    }
}
