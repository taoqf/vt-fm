using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Victop.Frame.SyncOperation
{
    internal class UserTimeThread
    {
        private bool done = false;

        public bool Done
        {
            get { return done; }
        }
        private int time = 0;
        public UserTimeThread(int time)
        {
            this.time = time;
        }
        public void Sleep()
        {
            lock (this)
            {
                if (!done)
                {
                    try
                    {
                        Monitor.Wait(this, time);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        throw ex;
                    }
                }
            }
            Stop();
        }
        public void Stop()
        {
            lock (this)
            {
                if (!done)
                {
                    done = true;
                    Monitor.Pulse(this);
                }
            }
        }
    }
}
