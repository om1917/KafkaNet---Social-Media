using CQRS.Core.Events;
using System.Diagnostics.CodeAnalysis;

namespace Post.Common.Events
{
    public class PostLikedEvent : BaseEvent
    {
        [SetsRequiredMembers]
        public PostLikedEvent() : base(nameof(PostLikedEvent))
        {
        }
    }
}