using System;

namespace EventLoop
{
    public interface IEventLoop
    {
        void Run();
        void BeginInvoke(Action action);
        void Invoke(Action action);
        void ShutDown(bool processPendingFrames = true);
    }
}
