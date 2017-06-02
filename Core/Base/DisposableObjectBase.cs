using System;

namespace Base
{
    public abstract class DisposableObjectBase : IDisposable
    {
        protected bool IsDisposed { get; private set; }

        ~DisposableObjectBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                IsDisposed = true;
            }
        }
    }
}
