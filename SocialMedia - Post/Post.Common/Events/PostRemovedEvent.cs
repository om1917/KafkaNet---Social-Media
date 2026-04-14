using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class PostRemovedEvent : BaseEvent
    {
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        public PostRemovedEvent() : base(nameof(PostRemovedEvent))
        {
        }
    }
}