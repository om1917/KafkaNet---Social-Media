using CQRS.Core.Infrastructure;
using CQRS.Core.Messages;

namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();
        public CommandDispatcher()
        {
            _handlers = new Dictionary<Type, Func<BaseCommand, Task>>();
        }

        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"Handler for command type {typeof(T).Name} is already registered.");
            }
            _handlers.Add(typeof(T), (command) => handler((T)command)); 
        }

        public async Task SendAsync(BaseCommand command)
        {
            if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task> handler))
            {
                await handler(command);
            }
            else
            {
                throw new InvalidOperationException($"No handler registered for command type {command.GetType().Name}");
            }
        }
    }
}