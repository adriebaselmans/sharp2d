using System;
using System.Collections.Generic;
using System.Threading;

namespace EventLoop
{
    public class EventLoop : IEventLoop
    {
        private readonly Queue<IEventFrame> _queue;
        private readonly object _lockObject;

        private bool _shouldProcessQueue;

        public EventLoop()
        {
            _queue = new Queue<IEventFrame>();
            _shouldProcessQueue = true;
            _lockObject = new object();
        }

        public void Run()
        {
            while (_shouldProcessQueue)
            {
                IEventFrame frame = null;
                lock (_lockObject)
                {
                    while (_shouldProcessQueue && _queue.Count == 0)
                    {
                        Monitor.Wait(_lockObject);
                    }

                    if (_shouldProcessQueue) // in case of shutdown scenario
                    {
                        frame = _queue.Dequeue();
                    }
                }

                if (frame != null)
                {
                    frame.Execute();
                }
            }
        }

        public void BeginInvoke(Action action)
        {
            var frame = new EventFrameAsync(action);
            Enqeue(frame);
        }

        public void Invoke(Action action)
        {
            var frame = new EventFrame(action);
            Enqeue(frame);
            frame.WaitForCompletion();
        }

        public void ShutDown(bool processPendingFrames = true)
        {
            if (processPendingFrames)
            {
                BeginInvoke(Stop);
            }
            else
            {
                lock (_lockObject)
                {
                    Stop();
                    Monitor.Pulse(_lockObject);
                }    
            }
        }

        private void Stop()
        {
            _shouldProcessQueue = false;
        }

        private void Enqeue(IEventFrame frame)
        {
            lock (_lockObject)
            {
                _queue.Enqueue(frame);
                Monitor.Pulse(_lockObject);
            }           
        }
    }
}