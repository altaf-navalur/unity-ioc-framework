using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XcelerateGames.IOC
{
    public class CommandBinder : ICommandBinder
    {
        public AbstractSignal mCurrentSignal = null;
        public readonly Dictionary<Type, object> _Bindings = new Dictionary<Type, object>();
        private readonly Dictionary<AbstractSignal, SignalSequencer> _SignalBindings = new Dictionary<AbstractSignal, SignalSequencer>();

        private AbstractSignal CurrentSignal
        {
            get { return mCurrentSignal; }
            set { mCurrentSignal = value; }
        }

        public ICommandBinder Dispatch<T>() where T : Signal, new()
        {
            if (CurrentSignal != null)
            {
                AddSignalToSequence<T>(mCurrentSignal);
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        public ICommandBinder Undo<T>() where T : AbstractCommand, new()
        {
            if (CurrentSignal != null)
            {
                RemoveCommandFromSequence(CurrentSignal, typeof(T));
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        #region Implementation
        public T GetSignal<T>() where T : AbstractSignal
        {
            Type type = typeof(T);
            if (_Bindings.ContainsKey(type))
                return _Bindings[type] as T;
            return default(T);
        }

        public T BindSignal<T>() where T : AbstractSignal, new()
        {
            Type type = typeof(T);
            _Bindings.Remove(type);
            _Bindings.Add(type, new T());
            return _Bindings[type] as T;
        }

        public T BindModel<T>() where T : class, new()
        {
            Type type = typeof(T);
            _Bindings.Remove(type);
            _Bindings.Add(type, new T());
            return _Bindings[type] as T;
        }

        public ICommandBinder On<T>() where T : AbstractSignal
        {
            CurrentSignal = GetSignal<T>();
            if (CurrentSignal == null)
                Debug.LogError("No binding found for " + typeof(T));
            return this;
        }

        public ICommandBinder Do<T>(params object[] executionParameters) where T : AbstractCommand, new()
        {
            if (CurrentSignal != null)
            {
                AddCommandToSequence(CurrentSignal, new T());
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        public ICommandBinder OnFinish<T>() where T : Command, new()
        {
            if (CurrentSignal != null)
            {
                if (_SignalBindings.ContainsKey(CurrentSignal))
                    _SignalBindings[CurrentSignal].OnFinish = (new T());
                else
                    Debug.LogError("You must add atlest one command before assigning Final command");
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        public ICommandBinder OnAbort<T>() where T : Command, new()
        {
            if (CurrentSignal != null)
            {
                if (_SignalBindings.ContainsKey(CurrentSignal))
                    _SignalBindings[CurrentSignal].OnFail = (new T());
                else
                    Debug.LogError("You must add atlest one command before assigning Fail command");
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        public ICommandBinder ContinueOnAbort()
        {
            if (CurrentSignal != null)
            {
                if (_SignalBindings.ContainsKey(CurrentSignal))
                    _SignalBindings[CurrentSignal].ContinueOnAbort();
                else
                    Debug.LogError("You must add atlest one command before assigning ContinueOnAbort");
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        public ICommandBinder ExecuteParallel()
        {
            if (CurrentSignal != null)
            {
                if (_SignalBindings.ContainsKey(CurrentSignal))
                    _SignalBindings[CurrentSignal].ExecuteInParallel();
                else
                    Debug.LogError("You must add atlest one command before assigning ExecuteParallel");
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        public ICommandBinder Once()
        {
            if (CurrentSignal != null)
            {
                if (_SignalBindings.ContainsKey(CurrentSignal))
                    _SignalBindings[CurrentSignal].Once();
                else
                    Debug.LogError("You must add atlest one command before calling Once");
            }
            else
                Debug.LogError("Current Signal is null");
            return this;
        }

        #endregion

        #region Private Methods
        internal void AddListeners()
        {
            foreach (KeyValuePair<AbstractSignal, SignalSequencer> pair in _SignalBindings)
            {
                pair.Key.AddReferencedListener(OnSignal);
            }
        }

        private void OnSignal(AbstractSignal signal)
        {
            SignalSequencer signalSequencer = null;
            if (_SignalBindings.TryGetValue(signal, out signalSequencer))
            {
                if(signal.Pooling)
                {
                    SignalSequencer sequencer = signalSequencer.Clone();
                    sequencer.Execute(signal, signal.Pooling);
                }
                else
                    signalSequencer.Execute(signal, signal.Pooling);
            }
        }

        private void RemoveCommandFromSequence(AbstractSignal signal, Type type)
        {
            if (_SignalBindings.ContainsKey(signal))
                _SignalBindings[signal].RemoveCommand(type);
        }

        private void AddCommandToSequence(AbstractSignal signal, AbstractCommand command)
        {
            if (!_SignalBindings.ContainsKey(signal))
                _SignalBindings.Add(signal, new SignalSequencer());
            _SignalBindings[signal].AddCommand(command);
        }

        private void AddSignalToSequence<T>(AbstractSignal signal) where T : AbstractSignal
        {
            if (!_SignalBindings.ContainsKey(signal))
                _SignalBindings.Add(signal, new SignalSequencer());
            AbstractSignal sigToDispatch = null;
            Type type = typeof(T);
            if (_Bindings.ContainsKey(type))
            {
                sigToDispatch = _Bindings[type] as T;
                _SignalBindings[signal].AddSignal(sigToDispatch);
            }
            else
                Debug.LogError($"Signal of {typeof(T)} must be bind before it can be added to Signal dispatch sequence.");
        }

        public ICommandBinder Mute<T>(params object[] executionParameters) where T : AbstractSignal, new()
        {
            GetSignal<T>().Mute();
            return this;
        }

        public ICommandBinder UnMute<T>(params object[] executionParameters) where T : AbstractSignal, new()
        {
            GetSignal<T>().UnMute();
            return this;
        }

        public ICommandBinder DoNotPool()
        {
            mCurrentSignal.DoNotPool();
            return this;
        }

        public void OnDestroy()
        {
            foreach(KeyValuePair<AbstractSignal, SignalSequencer> keyValuePair in _SignalBindings)
            {
                keyValuePair.Value.DestroyCommands();
            }
        }
        #endregion
    }
}
