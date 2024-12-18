using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooManyUtils
{

    /// <summary>
    /// All actions are ran on a task thread.
    /// </summary>
    public class ThreadLockedEvent : IDisposable
    {
        private event Action? TheEvent;
        private bool wasDisposed = false;
        private readonly object theLock = new();

        public ThreadLockedEvent()
        {
        }
        public ThreadLockedEvent(Action action)
        {
            this.Subscribe(action);
        }

        public void SafeSubscribe(Action action) { Task.Run(() => this.Subscribe(action)).ConfigureAwait(false); }
        public void Subscribe(Action action)
        {
            lock (theLock)
            {
                if (!wasDisposed)
                    TheEvent += action;
            }
        }

        public void SafeUnubscribe(Action action) { Task.Run(() => this.Subscribe(action)).ConfigureAwait(false); }
        public void Unubscribe(Action action)
        {
            lock (theLock)
            {
                if (!wasDisposed)
                    TheEvent -= action;
            }
        }

        public void SafeTrigger() { Task.Run(this.Trigger).ConfigureAwait(false); }
        public void Trigger()
        {
            lock (theLock)
            {
                if (!wasDisposed)
                    foreach (var del in TheEvent?.GetInvocationList() ?? Array.Empty<Delegate>())
                        Task.Run(() => { del.DynamicInvoke(null); }).ConfigureAwait(false);
            }
        }

        public void SafeDispose() { Task.Run(this.Dispose).ConfigureAwait(false); }
        public void Dispose()
        {
            lock (theLock)
            {
                wasDisposed = true;
                TheEvent = null;
            }
        }
    }
}
