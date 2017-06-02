using System;
using System.Threading;
using Base;

namespace EventLoop
{
    public class EventFrame : IEventFrame
    {
        public bool IsAsync { get; set; }
        private readonly Action _action;
        private readonly AutoResetEvent _resetEvent;

        public EventFrame(Action action)
        {
            _action = action;
            _resetEvent = new AutoResetEvent(false);
        }

        public void Execute()
        {
            _action();
            _resetEvent.Set();
        }

        public void WaitForCompletion()
        {
            _resetEvent.WaitOne();
            _resetEvent.Close();
        }
    }
}