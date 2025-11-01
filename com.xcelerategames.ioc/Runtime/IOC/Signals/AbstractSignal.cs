using System;
using System.Collections.Generic;

namespace XcelerateGames.IOC
{
    /// <summary>
    /// Base class for all signals
    /// </summary>
    public class AbstractSignal
    {
        /// <summary>
        /// List of all binding by type.
        /// </summary>
        public readonly Dictionary<Type, object> InjectionBindings = new Dictionary<Type, object>();

        /// <summary>
        /// Mute invoking listeners when the signal is dispatched
        /// </summary>
        protected bool IsMuted = false;

        /// <summary>
        /// Whether to use pooling, By default pooling is off. Must be turned on only in cases when the signal is firing in same frame multiple times
        /// </summary>
        protected bool NoPooling = false;

        /// <summary>
        /// Enable debug logs, If enabled all activities of the signals are logged
        /// </summary>
        protected bool DebugLogs = false;

        /// <summary>
        /// Is Pooling Enabled?
        /// </summary>
        public bool Pooling => NoPooling;

        /// <summary>
        /// Are debug logs enabled?
        /// </summary>
        public bool DebugLogsOn => DebugLogs || XDebug.CanLog(XDebug.Mask.IOC);

        /// <summary>
        /// Callback for referenced signals
        /// </summary>
        Action<AbstractSignal> onTriggerReferenced;

        /// <summary>
        /// Add a binding of type T with its default value
        /// </summary>
        /// <typeparam name="T">type of the binding to add</typeparam>
        protected void AddInjectionBinding<T>()
        {
            Type type = typeof(T);
            if (InjectionBindings.ContainsKey(type)) { return; }
            InjectionBindings.Add(type, default(T));
        }

        /// <summary>
        /// Add a referenced listener
        /// </summary>
        /// <param name="handler">listener to remove</param>
        public void AddReferencedListener(Action<AbstractSignal> handler)
        {
            onTriggerReferenced += handler;
        }

        /// <summary>
        /// Remove a referenced listener
        /// </summary>
        /// <param name="handler">listener to remove</param>
        public void RemoveReferencedListener(Action<AbstractSignal> handler)
        {
            onTriggerReferenced -= handler;
        }

        /// <summary>
        /// Mute the signal. If signal is muted, dispatching the signal will not invoke listeners.
        /// </summary>
        public void Mute()
        {
            IsMuted = true;
        }

        /// <summary>
        /// Unmute the signal. When this signal is dispatched, listeners will be invoked.
        /// </summary>
        public void UnMute()
        {
            IsMuted = false;
        }

        /// <summary>
        /// Disable Pooling of Signals, By default, Pooling is disabled for performance & memory usage.
        /// Use pooling only in cases where we have a case that multiple signals are executed at same time frame
        /// </summary>
        public void DoNotPool()
        {
            NoPooling = true;
        }

        /// <summary>
        /// Enable logs for this signal
        /// </summary>
        /// <returns></returns>
        public AbstractSignal EnableDebugLogs()
        {
            DebugLogs = true;
            return this;
        }

        /// <summary>
        /// Disable logs for this signal
        /// </summary>
        /// <returns></returns>
        public AbstractSignal DisableDebugLogs()
        {
            DebugLogs = false;
            return this;
        }

        /// <summary>
        /// Called on Dispatch for reference
        /// </summary>
        protected void OnDispatch()
        {
            onTriggerReferenced?.Invoke(this);
        }
    }
}