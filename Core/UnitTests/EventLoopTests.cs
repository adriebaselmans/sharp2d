using System;
using System.Threading;
using EventLoop;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class EventLoopTests
    {
        [Test]
        public void GivenEventLoop_WhenBeginInvokingMethod_ThenMethodIsExecuted()
        {
            var eventLoopThread = new EventLoopThread("EventLoop");
            eventLoopThread.Start();

            var resetEvent = new AutoResetEvent(false);
            var action = new Action(() => resetEvent.Set());

            eventLoopThread.BeginInvoke(action);
            var result = resetEvent.WaitOne(TimeSpan.FromSeconds(1));

            Assert.That(result, Is.True);

            eventLoopThread.Join();
        }

        [Test]
        public void GivenEventLoop_WhenInvokingMethod_ThenMethodIsExecuted()
        {
            var eventLoopThread = new EventLoopThread("EventLoop");
            eventLoopThread.Start();

            var isExecuted = false;
            var action = new Action(() => isExecuted = true);

            eventLoopThread.Invoke(action);
     
            Assert.That(isExecuted, Is.True);

            eventLoopThread.Join();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void GivenEventLoop_WhenInvokingMethodBeforeStartIsCalled_ThenExceptionIsThrown()
        {
            var eventLoopThread = new EventLoopThread("EventLoop");

            var action = new Action(() => { });

            eventLoopThread.Invoke(action);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void GivenEventLoop_WhenBeginInvokingMethodBeforeStartIsCalled_ThenExceptionIsThrown()
        {
            var eventLoopThread = new EventLoopThread("EventLoop");

            var action = new Action(() => { });

            eventLoopThread.BeginInvoke(action);
        }
    }
}