using MediatR;

namespace Contact.Domain.Events
{
    public class GroupCreatedDomainEvent : INotification
    {
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }
}
