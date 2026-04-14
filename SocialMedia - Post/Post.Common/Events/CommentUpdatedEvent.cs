using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
        {
        }
        public Guid CommentID { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime EditDate { get; set; }   
    }
}