using System;
using System.Threading;

namespace PathFinder
{
    public class TimerHolder
    {
        private readonly double _interval;
        private readonly Action _callback; 
        private static CancellationTokenSource _cancellationTokenSource; 
        public TimerHolder(double interval, Action callBack)
        {
            try
            {
                _interval = interval;
                _callback = callBack;
                _cancellationTokenSource = new CancellationTokenSource(); 
            }
            catch (Exception)
            {
            }
        } 
        public void Start(bool IsInvokeAllowed=true)
        { 
            try
            {
                if (IsInvokeAllowed)
                {
                    CancellationTokenSource cts = _cancellationTokenSource; // safe copy
                    Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(_interval), () =>
                    {
                        if (cts.IsCancellationRequested)
                        {
                            return false;
                        }

                        _callback.Invoke();

                        return true; //true to continuous, false to single use
                    });
                }
                else
                {
                    Stop();
                } 
            }
            catch (Exception)
            {
            }

        }
        public void Stop()
        {
            try
            {
                Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();
            }
            catch (Exception)
            { 
            } 
        }
    }
}
