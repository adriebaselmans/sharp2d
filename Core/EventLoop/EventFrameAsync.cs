using System;

namespace EventLoop
{
    public class EventFrameAsync : IEventFrame
    {
        private readonly Action _action;

        public EventFrameAsync(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action();
        }
    }
}