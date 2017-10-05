using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Toaster.Interfaces;

namespace Toaster.WebAPI
{
    /// <summary>
    /// Monitor the toaster status. Supports long polling. 
    /// </summary>
    public class ToasterStatusMonitor : IDisposable, IToasterStatusMonitor
    {
        /// <summary>
        /// We are watching the toaster status. 
        /// </summary>
        IToaster _toaster;

        /// <summary>
        /// Track if the status has changed since the last time the client retrieved it. 
        /// </summary>
        bool _changed;

        /// <summary>
        /// Status property names. 
        /// </summary>
        static readonly string[] _statusProperties = { "Setting", "Content", "Toasting", "Color" };

        /// <summary>
        /// Observable for toaster status property changes. 
        /// </summary>
        IObservable<string> _statusChanged;

        /// <summary>
        /// Subscription to status changed. 
        /// </summary>
        IDisposable _subscribeStatusChanged;

        /// <summary>
        /// Initialize the status monitor. 
        /// </summary>
        /// <param name="toaster"></param>
        public ToasterStatusMonitor(IToaster toaster)
        {
            _toaster = toaster;

            _statusChanged = FromPropertyChanged(_toaster)
                .Where(name => _statusProperties.Contains(name));

            _subscribeStatusChanged = _statusChanged
                //.Do(name => Console.WriteLine("Property {0} changed", name))
                .Do(name => { _changed = true; })
                .Subscribe();
        }

        /// <summary>
        /// Observe property changes. 
        /// </summary>
        /// <param name="source">Object to observe. </param>
        /// <returns>Observable sequence of property names. </returns>
        static IObservable<string> FromPropertyChanged(INotifyPropertyChanged source)
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => source.PropertyChanged += handler,
                handler => source.PropertyChanged -= handler)
                .Select(x => x.EventArgs.PropertyName);
        }

        /// <summary>
        /// Get the toaster status. 
        /// </summary>
        /// <returns>Toaster status. </returns>
        ToasterStatus GetToasterStatus()
        {
            return new ToasterStatus
            {
                setting = _toaster.Setting,
                content = _toaster.Content,
                toasting = _toaster.Toasting,
                color = _toaster.Color
            };
        }

        /// <summary>
        /// Get a task to wait for the toaster status. 
        /// </summary>
        /// <param name="timeout">Timeout in milliseconds. </param>
        /// <returns>Task to wait for the toaster status. </returns>
        public Task<ToasterStatus> GetToasterStatusAsync(int timeout)
        {
            if (_changed || (timeout <= 0))
            {
                //Console.WriteLine("Getting status immediately");
                _changed = false;
                return Task.FromResult(GetToasterStatus());
            }
            //Console.WriteLine("Waiting for status {0} msec", timeout);
            return _statusChanged
                .Throttle(TimeSpan.FromMilliseconds(5))
                .Timeout(TimeSpan.FromMilliseconds(timeout), Observable.Return("(timed out)"))
                //.Do(name => Console.WriteLine("Getting status for {0}", name))
                .Select(name => GetToasterStatus())
                .Take(1)
                .Do(status => { _changed = false; })
                .ToTask();
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects).
                if (_subscribeStatusChanged != null)
                {
                    _subscribeStatusChanged.Dispose();
                    _subscribeStatusChanged = null;
                }
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ObservableStatus() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}