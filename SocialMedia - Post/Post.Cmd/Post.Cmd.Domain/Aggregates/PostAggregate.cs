using CQRS.Core.Domain;
using CQRS.Core.Events;
using Post.Common.Events;
namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        public bool _active;
        public string _author;
        private readonly Dictionary<Guid, Tuple<string,string>> _comments = new();
        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public PostAggregate()
        {
            
        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Type = nameof(PostCreatedEvent),
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _author = @event.Author;
            _active = true;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Post is not active");
            }
            if(string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"Message {nameof(message)} cannot be empty");
            }
            RaiseEvent(new MessageUpdatedEvent
            {
                Type = nameof(MessageUpdatedEvent),
                Id = _id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("Cannot like an inactive post");
            }
            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Cannot comment on an inactive post");
            }
            if(string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"Comment {nameof(comment)} cannot be empty");
            }
            RaiseEvent(new CommentAddedEvent
            {
                Type = nameof(CommentAddedEvent),
                Id = _id,
                CommentID = Guid.NewGuid(),
                Comment = comment,
                UserName = userName,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentID, new Tuple<string, string>(@event.Comment, @event.UserName));
        }

        public void EditComment(Guid commentId,string comment,string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Cannot edit comment on an inactive post");
            }

            if(!_comments[commentId].Item2.Equals(userName, StringComparison.OrdinalIgnoreCase)){
                throw new InvalidOperationException("Cannot edit another user's comment");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Type = nameof(CommentUpdatedEvent),
                Id = _id,
                CommentID = commentId,
                Comment = comment,
                UserName = userName
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentID] = new Tuple<string, string>(@event.Comment, @event.UserName);
        }

         public void DeleteComment(Guid commentId,string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Cannot delete comment on an inactive post");
            }

            if(!_comments[commentId].Item2.Equals(userName, StringComparison.OrdinalIgnoreCase)){
                throw new InvalidOperationException("Cannot delete another user's comment");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                Type = nameof(CommentRemovedEvent),
                Id = _id,
                CommentID = commentId
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentID);
        }

        public void DeletePost(string userName){
            if (!_active)
            {
                throw new InvalidOperationException("Post has already been removed");
            }
            if(!_author.Equals(userName, StringComparison.OrdinalIgnoreCase)){
                throw new InvalidOperationException("Cannot delete another user's post");
            }

            RaiseEvent(new PostRemovedEvent
            {
                Type = nameof(PostRemovedEvent),
                Id = _id
            });
        }

        public void Apply(PostRemovedEvent @event){
            _id = @event.Id;
            _active = false;
        }       

    }

}