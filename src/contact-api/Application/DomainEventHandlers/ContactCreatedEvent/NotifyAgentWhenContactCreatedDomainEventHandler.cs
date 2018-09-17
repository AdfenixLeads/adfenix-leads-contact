namespace Contact.DomainEventHandlers.ContactCreatedEvent
{
    using Contact.Domain;
    using Contact.Domain.Events;
    using Contact.Services;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class NotifyAgentWhenContactCreatedDomainEventHandler
                        : INotificationHandler<ContactCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;

        public NotifyAgentWhenContactCreatedDomainEventHandler(
            ILoggerFactory logger,
            IRepository<Contact> contactRepository,
            IIdentityService identityService,
            IEventBus eventBus)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ContactCreatedDomainEvent contactCreatedDomainEvent, CancellationToken cancellationToken)
        {
           _logger.CreateLogger(nameof(NotifyAgentWhenContactCreatedDomainEventHandler)).LogTrace($"Agent xxx {contactCreatedDomainEvent.Email}.");
        }
    }
}
