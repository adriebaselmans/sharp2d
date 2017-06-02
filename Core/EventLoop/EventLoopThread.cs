using System;
using System.Threading;
using Base;

namespace EventLoop
{
    public class EventLoopThread : DisposableObjectBase
    {
        private readonly EventLoop _eventLoop;
        private readonly Thread _thread;
        private bool _started;

        public EventLoopThread(string name)
        {
            _eventLoop = new EventLoop();
            _thread = new Thread(() => _eventLoop.Run()) {Name = name};
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                Shutdown();
            }
            base.Dispose(disposing);
        }

        public IEventLoop EventLoop
        {
            get
            {
                return _eventLoop;
            }
        }

        public void Start()
        {
            if (!_started)
            {
                _started = true;
                _thread.Start();
            }
        }

        public void BeginInvoke(Action action)
        {
            CheckThreadState();
            _eventLoop.BeginInvoke(action);
        }
        public void Invoke(Action action)
        {
            CheckThreadState();
            _eventLoop.Invoke(action);
        }

        public void Join()
        {
            _eventLoop.ShutDown();
            _thread.Join();
        }

        public void Shutdown()
        {
            _eventLoop.ShutDown(false);
            _thread.Join();
        }

        private void CheckThreadState()
        {
            if (!_started) throw new InvalidOperationException("Please call Start() before queuing actions");
        }
    }
}