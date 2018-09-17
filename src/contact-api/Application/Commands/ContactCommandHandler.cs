namespace Contact.Commands
{
    using Contact.Domain;
    using LeadsPlus.Core;
    using MediatR;
    using Services;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    // Regular CommandHandler
    public class ContactCommandHandler
        : IRequestHandler<CreateContactCommand, bool>, 
        IRequestHandler<UpdateContactCommand, bool>,
        IRequestHandler<RemoveContactCommand, bool>
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public ContactCommandHandler(IMediator mediator, IRepository<Contact> contactRepository, IIdentityService identityService)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CreateContactCommand message, CancellationToken cancellationToken)
        {
            var contact = new Contact(message.OwnerId, message.OwnerName, message.Source, message.Firstname, message.Lastname, message.Email);

            await _contactRepository.AddAsync(contact);

            return true;
        }

        public async Task<bool> Handle(UpdateContactCommand message, CancellationToken cancellationToken)
        {
            //var contact = new Contact(message.OwnerId, message.Firstname, message.Lastname, message.Email);

            //await _contactRepository.AddAsync(contact);

            return true;
        }

        public async Task<bool> Handle(RemoveContactCommand message, CancellationToken cancellationToken)
        {
            //await _contactRepository.AddAsync(contact);

            return true;
        }
    }
}