using CQRS.Core.Events;
using System.Diagnostics.CodeAnalysis;

namespace Post.Common.Events
{
    public class CommentEditEvent : BaseEvent
    {
        [SetsRequiredMembers]
        public CommentEditEvent() : base(nameof(CommentEditEvent))
        {
        }
    }
}