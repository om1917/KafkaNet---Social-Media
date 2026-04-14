using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public CommentAddedEvent() : base(nameof(CommentAddedEvent))
        {
        }

        public Guid CommentID { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; }
    }
}