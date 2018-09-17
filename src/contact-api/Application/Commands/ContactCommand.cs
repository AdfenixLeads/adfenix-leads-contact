namespace Contact.Commands
{
    using MediatR;
    using System;

    public class CreateContactCommand : Command, IRequest<bool>
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
        public string OwnerName { get; set; }
    }

    public class UpdateContactCommand : Command, IRequest<bool>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public string GroupId { get; set; }
        public string OwnerId { get; set; }
    }

    public class RemoveContactCommand : Command, IRequest<bool>
    {
        
    }

    public class CopyContactToGroup : Command, IRequest<bool>
    {
        public string[] ContactIds { get; set; }
    }
}
