namespace Contact.Domain.Events
{
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class Event
    {
        [BsonId]
        public string EventId { get; set; }
        public string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime EventDate { get; set; }
    }
}
