using MediatR;

namespace Contact.Domain.Events
{
    public class ContactRemovedFromGroupDomainEvent : INotification
    {
        public string[] ContactIds { get; set; }
    }
}
