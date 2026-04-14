using CQRS.Core.Events;
using System.Diagnostics.CodeAnalysis;

namespace Post.Common.Events
{
    public class MessageUpdatedEvent : BaseEvent
    {
        [SetsRequiredMembers]
        public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent))
        {
        }

        public string Message { get; set; }
    }
}