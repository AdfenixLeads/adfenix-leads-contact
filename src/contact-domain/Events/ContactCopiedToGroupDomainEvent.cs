using MediatR;

namespace Contact.Domain.Events
{
    public class ContactCopiedToGroupDomainEvent : INotification
    {
        public string[] ContactIds { get; set; }
    }
}
