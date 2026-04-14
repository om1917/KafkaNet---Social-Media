using CQRS.Core.Events;
using System.Diagnostics.CodeAnalysis;

namespace Post.Common.Events
{
    public class CommentRemovedEvent : BaseEvent
    {
        [SetsRequiredMembers]
        public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
        {
        }
        public Guid CommentID { get; set; }
    }
}