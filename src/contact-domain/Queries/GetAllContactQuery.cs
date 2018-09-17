namespace Contact.Domain.Query
{
    using LeadsPlus.Core;
    using LeadsPlus.Core.Query;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GetAllContactQuery
    {

    }

    public class GetAllContactQueryHandler : IQueryHandler<GetAllContactQuery, List<Contact>>
    {
        private readonly IRepository<Contact> contactRepository;

        public GetAllContactQueryHandler(IRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<List<Contact>> Handle(GetAllContactQuery query)
        {
            return await contactRepository.Collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
