using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class PostCreatedEvent : BaseEvent
    {

        // Constructor must call base constructor
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public PostCreatedEvent() : base(nameof(PostCreatedEvent))
        {
        }

        public new Guid Id {get;set;}
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Message { get; set; }
        public DateTime DatePosted { get; set; }
    }
}