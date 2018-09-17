namespace Contact.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;
    using MediatR;
    using System;

    public class CreateContactIntegrationEvent : IntegrationEvent
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Source { get; set; }
        public string GroupId { get; set; }
        public string OwnerId { get; set; }
        public string Ownername { get; set; }
    }
}
